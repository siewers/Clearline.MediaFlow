﻿namespace Clearline.MediaFlow;

using System.Diagnostics;
using System.Text.RegularExpressions;
using Events;
using Exceptions;
using Microsoft.Win32.SafeHandles;

// TODO: Rename to FFmpegProcess
internal partial class FFmpegWrapper
{
    private TimeSpan _totalTime;
    private bool _wasKilled;

    internal List<string> OutputLog { get; } = [];

    /// <summary>
    ///     Fires when FFmpeg progress changes
    /// </summary>
    internal event ConversionProgressEventHandler? OnProgress;

    /// <summary>
    ///     Fires when FFmpeg process print something
    /// </summary>
    internal event DataReceivedEventHandler? OnDataReceived;

    /// <summary>
    ///     Fires when FFmpeg process writes video data to stdout
    /// </summary>
    internal event VideoDataEventHandler? OnVideoDataReceived;

    internal Task<bool> RunProcess(string args, ProcessPriorityClass? priority, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(() =>
                                     {
                                         var pipedOutput = OnVideoDataReceived != null;
                                         var process = ProcessHelper.StartProcess(args, FFmpegPath.Value.FullName, priority, standardInput: true, pipedOutput, standardError: true);
                                         var processId = process.Id;

                                         using (process)
                                         {
                                             process.ErrorDataReceived += (_, e) => ProcessOutputData(e, args, processId);
                                             process.BeginErrorReadLine();

                                             if (pipedOutput)
                                             {
                                                 Task.Run(() => ProcessVideoData(process, cancellationToken), cancellationToken);
                                             }

                                             var ctr = cancellationToken.Register(state =>
                                                                                  {
                                                                                      var p = (Process)state!;

                                                                                      if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                                                                                      {
                                                                                          return;
                                                                                      }

                                                                                      try
                                                                                      {
                                                                                          p.StandardInput.Write('q');
                                                                                          Thread.Sleep(TimeSpan.FromSeconds(5));

                                                                                          if (process.HasExited)
                                                                                          {
                                                                                              return;
                                                                                          }

                                                                                          p.CloseMainWindow();
                                                                                          p.Kill();
                                                                                          _wasKilled = true;
                                                                                      }
                                                                                      catch (InvalidOperationException)
                                                                                      {
                                                                                      }
                                                                                  }, process
                                                                                 );

                                             using (ctr)
                                             {
                                                 using (var processEnded = new ManualResetEvent(false))
                                                 {
                                                     processEnded.SetSafeWaitHandle(new SafeWaitHandle(process.Handle, ownsHandle: false));
                                                     var index = WaitHandle.WaitAny([processEnded, cancellationToken.WaitHandle]);

                                                     if (!process.HasExited)
                                                     {
                                                         switch (index)
                                                         {
                                                             case 0:
                                                                 // Workaround for linux: https://github.com/dotnet/corefx/issues/35544
                                                                 process.WaitForExit();
                                                                 break;
                                                             case 1:
                                                                 // If the signal came from the caller cancellation token close the window
                                                                 process.CloseMainWindow();
                                                                 process.Kill();
                                                                 _wasKilled = true;
                                                                 break;
                                                         }
                                                     }
                                                 }

                                                 cancellationToken.ThrowIfCancellationRequested();

                                                 if (_wasKilled)
                                                 {
                                                     throw new ConversionException("Could not stop process. Killed it.", args);
                                                 }

                                                 if (cancellationToken.IsCancellationRequested)
                                                 {
                                                     return false;
                                                 }

                                                 var output = string.Join(Environment.NewLine, OutputLog);
                                                 FFmpegExceptionCatcher.CatchFFmpegErrors(output, args);

                                                 if (process.ExitCode != 0 && OutputLog.Count != 0 && !OutputLog.Last().Contains("dummy"))
                                                 {
                                                     throw new ConversionException(output, args);
                                                 }
                                             }
                                         }

                                         return true;
                                     },
                                     cancellationToken,
                                     TaskCreationOptions.LongRunning,
                                     TaskScheduler.Default
                                    );
    }

    private void ProcessOutputData(DataReceivedEventArgs eventArgs, string args, int processId)
    {
        if (eventArgs.Data == null)
        {
            return;
        }

        OnDataReceived?.Invoke(this, eventArgs);

        OutputLog.Add(eventArgs.Data);
        CalculateTime(eventArgs, args, processId);
    }

    private void ProcessVideoData(Process process, CancellationToken cancellationToken)
    {
        var br = new BinaryReader(process.StandardOutput.BaseStream);
        byte[] buffer;

        while ((buffer = br.ReadBytes(4096)).Length > 0)
        {
            var args = new VideoDataEventArgs(buffer);
            OnVideoDataReceived?.Invoke(this, args);

            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    private void CalculateTime(DataReceivedEventArgs e, string args, int processId)
    {
        var data = e.Data;

        if (string.IsNullOrWhiteSpace(data))
        {
            return;
        }

        if (data.Contains("Duration: N/A"))
        {
            return;
        }

        if (data.Contains("Duration"))
        {
            GetDuration(e, args);
        }
        else if (data.Contains("size"))
        {
            var match = TimeFormatRegex().Match(data);
            var ts = GetTimeSpanValue(match);

            if (ts.TotalMilliseconds > 0)
            {
                OnProgress?.Invoke(this, new ConversionProgressEventArgs(processId, ts, _totalTime));
            }
        }
    }

    private void GetDuration(DataReceivedEventArgs e, string args)
    {
        var t = GetArgumentValue("-t", args);

        if (!string.IsNullOrWhiteSpace(t) && t != "1")
        {
            _totalTime = TimeSpan.Parse(t);
            return;
        }

        if (e.Data is null)
        {
            return;
        }

        var match = TimeFormatRegex().Match(e.Data);

        if (!match.Success)
        {
            return;
        }

        _totalTime = _totalTime.Add(TimeSpan.Parse(match.Value));

        var ss = GetArgumentValue("-ss", args);

        if (!string.IsNullOrWhiteSpace(ss))
        {
            _totalTime -= TimeSpan.Parse(ss);
        }
    }

    private static string GetArgumentValue(string option, string args)
    {
        var words = args.Split(' ').ToList();
        var index = words.IndexOf(option);
        return index >= 0 ? words[index + 1] : string.Empty;
    }

    private static TimeSpan GetTimeSpanValue(Match match)
    {
        while (true)
        {
            if (!match.Success)
            {
                return TimeSpan.Zero;
            }

            if (TimeSpan.TryParse(match.Value, out var timeSpanValue))
            {
                return timeSpanValue;
            }

            match = match.NextMatch();
        }
    }

    [GeneratedRegex(@"\w\w:\w\w:\w\w", RegexOptions.Compiled)]
    private static partial Regex TimeFormatRegex();
}

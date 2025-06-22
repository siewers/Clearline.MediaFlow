namespace Clearline.MediaFlow.Probe;

using System.Diagnostics;
using Exceptions;
using Models;

public static class FFprobe
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    internal async static Task<ProbeModel> GetProbeModel(MediaLocation mediaLocation, CancellationToken cancellationToken)
    {
        if (!mediaLocation.Exists())
        {
            throw new InvalidInputException($"Input file {mediaLocation} doesn't exist.");
        }

        using var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cancellationTokenSource.CancelAfter(DefaultTimeout);

        var arguments = $"-v panic -print_format json -show_format -show_streams {mediaLocation.Escape()}";
        var probeResult = await StartProcess(arguments, cancellationTokenSource.Token);

        if (string.IsNullOrWhiteSpace(probeResult))
        {
            throw new ArgumentException($"Invalid file. Cannot load file {mediaLocation}.");
        }

        var probeData = FFprobeJsonDeserializer.Deserialize<ProbeModel>(probeResult);

        if (probeData is null)
        {
            throw new ArgumentException($"Invalid file. Cannot deserialize probe data {mediaLocation}.");
        }

        if (probeData.Format is null)
        {
            throw new ArgumentException($"Invalid file. No format found {mediaLocation}.");
        }

        if (probeData.Streams is null || probeData.Streams.Count == 0)
        {
            throw new ArgumentException($"Invalid file. No streams found {mediaLocation}.");
        }

        return probeData;
    }

    private async static Task<string> StartProcess(string args, CancellationToken cancellationToken)
    {
        return await Task.Factory.StartNew(() =>
                                           {
                                               using var process = ProcessHelper.StartProcess(args, FFprobePath.Value.FullName, priority: null, standardOutput: true);

                                               var processState = new ProcessState(process, ProcessExited: false);

                                               cancellationToken.Register(state => ((ProcessState)state!).Kill(), processState);

                                               var output = process.StandardOutput.ReadToEnd();
                                               processState.WaitForExit();
                                               return output;
                                           },
                                           cancellationToken,
                                           TaskCreationOptions.LongRunning,
                                           TaskScheduler.Default
                                          );
    }

    private record struct ProcessState(Process Process, bool ProcessExited)
    {
        public void WaitForExit()
        {
            if (ProcessExited)
            {
                return;
            }

            Process.WaitForExit();
            ProcessExited = true;
        }

        public void Kill()
        {
            if (ProcessExited || Process.HasExited)
            {
                return;
            }

            try
            {
                Process.Kill();
                Process.CloseMainWindow();
            }
            catch
            {
                // ignored, process might have already exited
            }
        }
    }
}

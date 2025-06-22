namespace Clearline.MediaFlow;

using System.Diagnostics;

internal static class ProcessHelper
{
    public static Process StartProcess
    (
        string args,
        string processPath,
        ProcessPriorityClass? priority,
        bool standardInput = false,
        bool standardOutput = false,
        bool standardError = false
    )
    {
        var startInfo = new ProcessStartInfo(processPath, args)
                        {
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            RedirectStandardInput = standardInput,
                            RedirectStandardOutput = standardOutput,
                            RedirectStandardError = standardError,
                        };

        var process = new Process
                      {
                          StartInfo = startInfo,
                          EnableRaisingEvents = true,
                      };

        process.Start();

        if (process is null)
        {
            throw new InvalidOperationException($"Failed to start process: {processPath} with arguments: {args}");
        }

        try
        {
            process.PriorityClass = priority ?? Process.GetCurrentProcess().PriorityClass;
        }
        catch (Exception)
        {
            // ignored
        }

        return process;
    }
}

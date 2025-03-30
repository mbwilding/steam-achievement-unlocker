using System.Diagnostics;
using Serilog;

namespace SteamAchievementUnlocker;

public class Agent
{
    public static async Task RunAsync(string app, string appId, string gameName, bool clear)
    {
        var dir = Clone(appId);

        var arguments = $"{string.Concat(string.Join(' ', gameName.Trim()))} {appId.Trim()} clear={clear}";

        var startInfo = new ProcessStartInfo
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = $"{dir}/{app}",
            Arguments = arguments,
            CreateNoWindow = true,
            UseShellExecute = false,
            WorkingDirectory = dir
        };

#if LINUX || MAC
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
#endif

        var agent = Process.Start(startInfo);
        if (agent != null)
        {
            await agent.WaitForExitAsync();
            if (agent.ExitCode == 0)
            {
                Log.Information("Agent success: {GameName} [{AppId}]", gameName, appId);
            }
            else
            {
                Log.Error("Agent failed: {GameName} [{AppId}]", gameName, appId);
            }
        }
        else
        {
            Log.Error("Agent failed to launch: {GameName} [{AppId}]", gameName, appId);
        }

        Directory.Delete(dir, true);
    }

    private static string Clone(string appId)
    {
        var current = Directory.GetCurrentDirectory();
        var targetRoot = Path.Combine(current, "Apps", appId);

        if (Directory.Exists(targetRoot))
            Directory.Delete(targetRoot, true);
        Directory.CreateDirectory(targetRoot);

        var files = Directory.EnumerateFiles(current, "*", SearchOption.AllDirectories)
            .Where(x => !x.Contains("Apps") &&
                        !x.Contains("Logs") &&
                        !(x.Contains("SteamAchievementUnlocker") && !x.Contains("SteamAchievementUnlockerAgent")));

        foreach (var file in files)
        {
            var relativePath = Path.GetRelativePath(current, file);
            var targetPath = Path.Combine(targetRoot, relativePath);
            var targetDir = Path.GetDirectoryName(targetPath);

            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir!);

            File.CreateSymbolicLink(targetPath, file);
        }

        return targetRoot;
    }
}

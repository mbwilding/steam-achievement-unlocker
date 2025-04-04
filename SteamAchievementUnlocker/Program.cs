using System.Diagnostics;
using System.Text.RegularExpressions;
using Common;
using Serilog;
using SteamAchievementUnlocker;

string exePath = Process.GetCurrentProcess().MainModule!.FileName;
string exeDirectory = Path.GetDirectoryName(exePath)!;
Directory.SetCurrentDirectory(exeDirectory);

Process.GetProcessesByName("SteamAchievementUnlockerAgent")
    .ToList()
    .ForEach(x => x.Kill());

Common.Serilog.Init("Achievement Unlocker");

#if WIN
var first = true;
while (Helpers.ReadRegistry(@"Software\Valve\Steam\ActiveProcess", "ActiveUser") == 0)
{
    if (first)
    {
        Log.Information("Waiting for you to log into Steam\n");
        first = false;
    }

    await Task.Delay(500);
}
var app = "SteamAchievementUnlockerAgent.exe";
#elif LINUX || MAC
Log.Information("Make sure Steam is running and logged in");
Log.Information("Otherwise the following will all fail\n");
string app = "SteamAchievementUnlockerAgent";
Environment.SetEnvironmentVariable("LD_PRELOAD", Path.Combine(Directory.GetCurrentDirectory(), "libsteam_api.so"));
#endif

const string clearString = "--clear";
var clearToggle = args.Contains(clearString);
var argsList = args.Where(x => !x.Contains(clearString)).ToList();

var options = new ParallelOptions { MaxDegreeOfParallelism = 1 };

if (argsList.Count != 0)
{
    await Parallel.ForEachAsync(argsList.Distinct(), options, async (appId, _) =>
    {
        if (uint.TryParse(appId, out var _))
            await Agent.RunAsync(app, appId, appId, clearToggle);
        else
            Log.Error("Please enter a numerical app ID: {Arg}", appId);
    });
}
else
{
    var games = await Helpers.GetGameListAsync();

    await Parallel.ForEachAsync(games, options, async (game, _) =>
    {
        var gameName = game.Key
            .Trim(Path.GetInvalidFileNameChars())
            .Trim(Path.GetInvalidPathChars());

        var appId = game.Value;

        var rgx = new Regex("[^a-zA-Z0-9 ()&$:_ -]");
        gameName = rgx.Replace(gameName, string.Empty);
        await Agent.RunAsync(app, appId, gameName, clearToggle);
    });
}

if (Directory.Exists("Apps"))
{
    Directory.Delete("Apps", true);
}

Console.WriteLine("\nPress any key to exit");
Console.ReadKey();

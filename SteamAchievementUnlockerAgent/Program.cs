using SteamAchievementUnlockerAgent;

#if LINUX || MAC
    Console.SetOut(TextWriter.Null);
#endif

var gameName = string.Empty;

if (args.Length < 3)
    Environment.Exit(1);

for (var i = 0; i < args.Length - 1; i++)
{
    gameName += $"{args[i]} ";
}
gameName = gameName.TrimEnd();
var appId = args[^2];
var clear = args[^1].Contains("clear=True");

var steam = new Steam(gameName, appId, clear);
var result = await steam.InitAsync();
steam.Dispose();
Environment.Exit(result);

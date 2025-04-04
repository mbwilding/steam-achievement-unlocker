using Serilog;
using Serilog.Events;

namespace Common;

public static class Serilog
{
    public static void Init(string name)
    {
        string dir = string.Empty;
        if (name.EndsWith("Agent")){
            dir = "../../";
        }

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Async(x => x.Console(LogEventLevel.Information))
            .WriteTo.Async(x => x.File($"{dir}Logs/{DateTime.Now:yyyyMMdd}/{name}.log"))
            .CreateLogger();
    }
}

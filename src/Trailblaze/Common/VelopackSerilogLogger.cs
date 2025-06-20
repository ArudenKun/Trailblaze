using Serilog.Events;
using Velopack.Logging;

namespace Trailblaze.Common;

public sealed class VelopackSerilogLogger : IVelopackLogger
{
    public void Log(VelopackLogLevel logLevel, string? message, Exception? exception)
    {
        if (exception is not null)
        {
            Serilog.Log.Logger.Write(
                MapVelopackLogLevel(logLevel),
                exception,
                "{Message}",
                message
            );
            return;
        }

        Serilog.Log.Logger.Write(MapVelopackLogLevel(logLevel), "{Message}", message);
    }

    private LogEventLevel MapVelopackLogLevel(VelopackLogLevel velopackLogLevel) =>
        velopackLogLevel switch
        {
            VelopackLogLevel.Trace => LogEventLevel.Verbose,
            VelopackLogLevel.Debug => LogEventLevel.Debug,
            VelopackLogLevel.Information => LogEventLevel.Information,
            VelopackLogLevel.Warning => LogEventLevel.Warning,
            VelopackLogLevel.Error => LogEventLevel.Error,
            VelopackLogLevel.Critical => LogEventLevel.Fatal,
            _ => throw new ArgumentOutOfRangeException(
                nameof(velopackLogLevel),
                velopackLogLevel,
                "Invalid log level specified"
            ),
        };
}

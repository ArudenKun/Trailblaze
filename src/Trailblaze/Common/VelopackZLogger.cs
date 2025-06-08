using System;
using Microsoft.Extensions.Logging;
using Velopack;
using Velopack.Logging;
using ZLogger;

namespace Trailblaze.Common;

public sealed class VelopackZLogger : IVelopackLogger
{
    private readonly ILoggerFactory _loggerFactory;

    public VelopackZLogger(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public void Log(VelopackLogLevel logLevel, string? message, Exception? exception)
    {
        var logger = _loggerFactory.CreateLogger<VelopackApp>();
        if (exception is not null)
        {
            logger.ZLog(MapLogLevel(logLevel), exception, $"{message}");
        }
        else
        {
            logger.ZLog(MapLogLevel(logLevel), $"{message}");
        }
    }

    private static LogLevel MapLogLevel(VelopackLogLevel logLevel) =>
        logLevel switch
        {
            VelopackLogLevel.Trace => LogLevel.Trace,
            VelopackLogLevel.Debug => LogLevel.Debug,
            VelopackLogLevel.Information => LogLevel.Information,
            VelopackLogLevel.Warning => LogLevel.Warning,
            VelopackLogLevel.Error => LogLevel.Error,
            VelopackLogLevel.Critical => LogLevel.Critical,
            _ => LogLevel.None,
        };
}

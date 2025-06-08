using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Trailblaze.Common;
using Velopack;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using ZLogger;

namespace Trailblaze;

public static class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        VelopackApp.Build().Run();
        var builder = Host.CreateApplicationBuilder(args);
        builder.AddAvaloniaHosting<App>(appBuilder => appBuilder.UsePlatformDetect().LogToTrace());
        builder.AddTrailblaze();

        var app = builder.Build();

        try
        {
            app.Run();
        }
        catch (Exception ex)
        {
            var logger = app.Services.GetRequiredService<ILogger<App>>();
            logger.ZLogError(ex, $"An unhandled exception occurred");
            _ = PInvoke.MessageBox(
                new HWND(0),
                ex.Message,
                $"{AppInformation.Name} Fatal Error",
                MESSAGEBOX_STYLE.MB_ICONERROR
            );
            throw;
        }
        finally
        {
            app.Dispose();
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<Application>().UsePlatformDetect().LogToTrace();
}

using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Trailblaze.Avalonia.Hosting;
using Trailblaze.Common;
using Trailblaze.Common.Helpers;
using Velopack;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Trailblaze;

public static class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
        VelopackApp.Build().SetArgs(args).SetLogger(new VelopackSerilogLogger()).Run();

        var builder = Host.CreateApplicationBuilder(args);
        builder.Configuration.AddJsonFile(PathHelper.SettingsPath, false);
        builder.AddAvaloniaHosting<App>(
            (sp, appBuilder) =>
                appBuilder
                    .UsePlatformDetect()
                    .UseR3(ex =>
                        sp.GetRequiredService<ILogger<App>>()
                            .LogError(ex, "An unhandled exception occurred")
                    )
                    .LogToTrace()
        );
        builder.AddTrailblaze();

        var app = builder.Build();

        try
        {
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "{Name} Fatal Error", AppHelper.Name);
            _ = PInvoke.MessageBox(
                new HWND(0),
                ex.Message,
                $"{AppHelper.Name} Fatal Error",
                MESSAGEBOX_STYLE.MB_ICONERROR
            );
            throw;
        }
        finally
        {
            app.Dispose();
            Log.CloseAndFlush();
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<Application>().UsePlatformDetect().LogToTrace();
}

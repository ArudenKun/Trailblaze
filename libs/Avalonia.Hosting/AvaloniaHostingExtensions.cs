using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Hosting.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Avalonia.Hosting;

/// <summary>
/// Provides extension methods for configuring Avalonia applications with the generic host.
/// </summary>
public static class AvaloniaHostingExtensions
{
    /// <summary>
    /// Adds Avalonia main window to the host's service collection,
    /// and a <see cref="AppBuilder"/> to create the Avalonia application.
    /// </summary>
    /// <param name="services">The IServiceCollection.</param>
    /// <param name="appBuilderConfiguration">The application builder</param>
    /// <returns>The updated host application builder.</returns>
    public static IServiceCollection AddAvaloniaHosting<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApp
    >(
        this IServiceCollection services,
        Action<IServiceProvider, AppBuilder> appBuilderConfiguration
    )
        where TApp : Application
    {
        services
            .AddSingleton<TApp>()
            .AddSingleton<Application>(sp => sp.GetRequiredService<TApp>())
            .AddSingleton(sp =>
            {
                var appBuilder = AppBuilder.Configure(sp.GetRequiredService<TApp>);
                appBuilderConfiguration(sp, appBuilder);
                return appBuilder;
            })
            .AddSingleton<IClassicDesktopStyleApplicationLifetime>(_ =>
                (IClassicDesktopStyleApplicationLifetime?)Application.Current?.ApplicationLifetime
                ?? throw new InvalidOperationException("Avalonia application lifetime is not set.")
            )
            .AddSingleton<AvaloniaThread>()
            .AddHostedService<AvaloniaHostedService>();

        return services;
    }

    /// <summary>
    /// Adds Avalonia main window to the host's service collection,
    /// and a <see cref="AppBuilder"/> to create the Avalonia application.
    /// </summary>
    /// <param name="builder">The host application builder.</param>
    /// <param name="appBuilder">The application builder, also used by the previewer.</param>
    /// <returns>The updated host application builder.</returns>
    public static IHostApplicationBuilder AddAvaloniaHosting<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApp
    >(this IHostApplicationBuilder builder, Action<IServiceProvider, AppBuilder> appBuilder)
        where TApp : Application
    {
        builder.Services.AddAvaloniaHosting<TApp>(appBuilder);
        return builder;
    }

    /// <summary>
    /// Adds Avalonia main window to the host's service collection,
    /// and a <see cref="AppBuilder"/> to create the Avalonia application.
    /// </summary>
    /// <param name="builder">The host application builder.</param>
    /// <param name="appBuilderAction">The application builder, also used by the previewer.</param>
    /// <returns>The updated host application builder.</returns>
    public static IHostApplicationBuilder AddAvaloniaHosting<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApp
    >(this IHostApplicationBuilder builder, Action<AppBuilder> appBuilderAction)
        where TApp : Application
    {
        builder.Services.AddAvaloniaHosting<TApp>((_, appBuilder) => appBuilderAction(appBuilder));
        return builder;
    }

    /// <summary>
    /// Adds Avalonia main window to the host's service collection,
    /// and a <see cref="AppBuilder"/> to create the Avalonia application.
    /// </summary>
    /// <param name="builder">The host application builder.</param>
    /// <param name="appBuilder">The application builder, also used by the previewer.</param>
    /// <returns>The updated host application builder.</returns>
    public static IHostBuilder ConfigureAvaloniaHosting<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApp
    >(this IHostBuilder builder, Action<IServiceProvider, AppBuilder> appBuilder)
        where TApp : Application
    {
        builder.ConfigureServices(services =>
        {
            services.AddAvaloniaHosting<TApp>(appBuilder);
        });
        return builder;
    }

    /// <summary>
    /// Adds Avalonia main window to the host's service collection,
    /// and a <see cref="AppBuilder"/> to create the Avalonia application.
    /// </summary>
    /// <param name="builder">The host application builder.</param>
    /// <param name="appBuilderAction">The application builder, also used by the previewer.</param>
    /// <returns>The updated host application builder.</returns>
    public static IHostBuilder ConfigureAvaloniaHosting<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApp
    >(this IHostBuilder builder, Action<AppBuilder> appBuilderAction)
        where TApp : Application
    {
        builder.ConfigureServices(services =>
        {
            services.AddAvaloniaHosting<TApp>((_, appBuilder) => appBuilderAction(appBuilder));
        });
        return builder;
    }
}

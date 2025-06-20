using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization.Metadata;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Replicant;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.AsyncFile;
using ServiceScan.SourceGenerator;
using Trailblaze.Common;
using Trailblaze.Common.Extensions;
using Trailblaze.Common.Helpers;
using Trailblaze.Common.Settings;
using Trailblaze.Core.HoyoPlay;
using Trailblaze.Services;
using Trailblaze.ViewModels;
using Trailblaze.Views.Abstractions;
using ZLinq;

namespace Trailblaze;

public static partial class DependencyInjection
{
    public static IHostApplicationBuilder AddTrailblaze(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOptions<AppSettings>().Bind(builder.Configuration);
        builder.Services.AddSingleton<AppSettings>(sp =>
            sp.GetRequiredService<IOptions<AppSettings>>().Value
        );
        builder.Services.AddSingleton(sp => new LoggingLevelSwitch(
            sp.GetRequiredService<AppSettings>().Logger.LogLevel
        ));
        builder.Services.AddSerilog(
            (sp, configuration) =>
                configuration
                    .ReadFrom.Services(sp)
                    .WriteTo.Console()
                    .WriteTo.AsyncFile(PathHelper.LogsDirectory.CombinePath("log.txt"))
                    .Enrich.FromLogContext()
        );
        builder
            .Services.AddViews()
            .AddViewModels()
            .AddSingleton<ViewLocator>()
            .AddSingleton<IJsonTypeInfoResolver>(AppJsonContext.Default)
            .AddSingleton(AppJsonContext.Default.Options)
            .AddSingleton<ViewModelFactory>()
            .AddSingleton<ThemeService>()
            .AddSingleton<ReplicantImageLoader>()
            .AddSingleton(sp => new HttpCache(
                PathHelper.CacheDirectory,
                () => sp.GetRequiredService<IHttpClientFactory>().CreateClient()
            ))
            .AddSingleton<IHttpCache>(sp => sp.GetRequiredService<HttpCache>());

        builder.Services.AddHttpClient();
        builder
            .Services.AddHttpClient<HoyoPlayClient>()
            .ConfigureHttpClient(httpClient =>
                httpClient.DefaultRequestVersion = HttpVersion.Version20
            )
            .ConfigurePrimaryHttpMessageHandler(() =>
                new HttpClientHandler { AutomaticDecompression = DecompressionMethods.All }
            );
        return builder;
    }

    [GenerateServiceRegistrations(
        AssignableTo = typeof(ISingletonViewModel),
        CustomHandler = nameof(AddViewModelsHandler)
    )]
    [GenerateServiceRegistrations(
        AssignableTo = typeof(ITransientViewModel),
        CustomHandler = nameof(AddViewModelsHandler)
    )]
    private static partial IServiceCollection AddViewModels(this IServiceCollection services);

    private static void AddViewModelsHandler<TViewModel>(this IServiceCollection services)
        where TViewModel : ViewModel
    {
        var type = typeof(TViewModel);
        var lifeTime = type.IsAssignableTo(typeof(ISingletonViewModel))
            ? ServiceLifetime.Singleton
            : ServiceLifetime.Transient;
        services.Add(new ServiceDescriptor(type, type, lifeTime));
        foreach (var baseType in type.EnumerateBaseTypes())
        {
            services.Add(
                new ServiceDescriptor(baseType, sp => sp.GetRequiredService(type), lifeTime)
            );
        }
    }

    [GenerateServiceRegistrations(
        AssignableTo = typeof(IView<>),
        CustomHandler = nameof(AddViewsHandler)
    )]
    private static partial IServiceCollection AddViews(this IServiceCollection services);

    private static void AddViewsHandler<
        [DynamicallyAccessedMembers(
            DynamicallyAccessedMemberTypes.Interfaces
                | DynamicallyAccessedMemberTypes.PublicConstructors
        )]
            TView
    >(this IServiceCollection services)
        where TView : Control
    {
        var type = typeof(TView);
        var viewInterfaces = type.GetInterfaces()
            .AsValueEnumerable()
            .Where(s => s.FullName!.StartsWith(typeof(IView).FullName!))
            .ToArray();

        if (viewInterfaces.Length > 2)
        {
            throw new InvalidOperationException(
                $"The view {type.FullName} implements more than two IView interfaces"
            );
        }

        services.AddTransient<TView>();
        foreach (var viewInterface in viewInterfaces)
        {
            services.AddTransient(viewInterface, sp => sp.GetRequiredService<TView>());
        }
    }
}

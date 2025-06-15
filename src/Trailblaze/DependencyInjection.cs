using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization.Metadata;
using Avalonia.Controls;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Replicant;
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
using ZLogger;
using ZLogger.Providers;

namespace Trailblaze;

public static partial class DependencyInjection
{
    public static IHostApplicationBuilder AddTrailblaze(this IHostApplicationBuilder builder)
    {
        builder.Logging.ClearProviders()
            .SetMinimumLevel(LogLevel.Debug)
            .AddZLoggerConsole(options => options.UseDefaultPlainTextFormatter())
            .AddZLoggerRollingFile((options) =>
            {
                options.FilePathSelector = (timestamp, sequenceNumber) =>
                    PathHelper.LogsDirectory.CombinePath(
                        $"{timestamp.ToLocalTime():yyyy-MM}_{sequenceNumber:000}.log"
                    );
                options.RollingInterval = RollingInterval.Month;
                options.RollingSizeKB = (int)2.Gigabytes().Kilobytes;
                options.UseDefaultPlainTextFormatter();
            });

        builder.Services
            .AddViews()
            .AddViewModels()
            .AddSingleton<ViewLocator>()
            .AddSingleton<IJsonTypeInfoResolver>(AppJsonContext.Default)
            .AddSingleton(AppJsonContext.Default.Options)
            .AddSingleton<AppSettings>()
            .AddSingleton<ViewModelFactory>()
            .AddSingleton<ReplicantImageLoader>()
            .AddSingleton(sp =>
                new HttpCache(PathHelper.CacheDirectory,
                    () => sp.GetRequiredService<IHttpClientFactory>().CreateClient()))
            .AddSingleton<IHttpCache>(sp => sp.GetRequiredService<HttpCache>());

        builder.Services.AddHttpClient();
        builder.Services.AddHttpClient<HoyoPlayClient>()
            .ConfigureHttpClient(httpClient => httpClient.DefaultRequestVersion = HttpVersion.Version20)
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.All
            });
        return builder;
    }

    private static void UseDefaultPlainTextFormatter(this ZLoggerOptions options)
    {
        options.UsePlainTextFormatter(formatter =>
        {
            formatter.SetPrefixFormatter($"[{0} {1,-11}] ",
                (in template, in info) => template.Format(info.Timestamp, info.LogLevel));
        });
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
            services.Add(new ServiceDescriptor(
                baseType,
                sp => sp.GetRequiredService(type),
                lifeTime
            ));
        }
    }

    [GenerateServiceRegistrations(
        AssignableTo = typeof(IView<>),
        CustomHandler = nameof(AddViewsHandler)
    )]
    private static partial IServiceCollection AddViews(this IServiceCollection services);

    private static void AddViewsHandler<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces |
                                    DynamicallyAccessedMemberTypes.PublicConstructors)]
        TView
    >(this IServiceCollection services)
        where TView : Control
    {
        var type = typeof(TView);
        var viewInterfaces = type.GetInterfaces().AsValueEnumerable()
            .Where(s => s.FullName!.StartsWith(typeof(IView).FullName!)).ToArray();

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
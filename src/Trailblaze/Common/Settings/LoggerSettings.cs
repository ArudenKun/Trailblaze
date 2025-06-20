using CommunityToolkit.Mvvm.ComponentModel;
using Serilog.Events;

namespace Trailblaze.Common.Settings;

public sealed partial class LoggerSettings : ObservableObject
{
    [ObservableProperty]
    public partial LogEventLevel LogLevel { get; set; } = LogEventLevel.Verbose;
}

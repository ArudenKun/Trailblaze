using System.ComponentModel;

namespace Trailblaze.Models;

public enum AppTheme
{
    [Description("Default from system")]
    Default,

    [Description("Light")]
    Light,

    [Description("Dark")]
    Dark,
}

using System.ComponentModel;

namespace Trailblaze.Models;

public enum ApplicationTheme
{
    [Description("Default from system")]
    Default,

    [Description("Light")]
    Light,

    [Description("Dark")]
    Dark,
}

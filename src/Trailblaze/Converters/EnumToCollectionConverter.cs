using System.Globalization;
using Avalonia.Data.Converters;
using Trailblaze.Common.Extensions;

namespace Trailblaze.Converters;

public class EnumToCollectionConverter : IValueConverter
{
    public object? Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    ) => value is not Enum @enum ? null : @enum.GetType().GetAllValues();

    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        var parameterString = parameter?.ToString();
        if (string.IsNullOrWhiteSpace(parameterString))
            return null;

        return Enum.TryParse(targetType, parameterString, true, out var result) ? result : null;
    }
}

using System;
using Avalonia.Data.Converters;
using Trailblaze.Common.Extensions;

namespace Trailblaze.Converters;

public class RawEnumToCollectionConverter : IValueConverter
{
    public object? Convert(
        object? value,
        Type targetType,
        object? parameter,
        System.Globalization.CultureInfo culture
    )
    {
        if (value is not Enum)
            return null;
        return value.GetType().GetAllValues();
    }

    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        System.Globalization.CultureInfo culture
    )
    {
        var parameterString = parameter?.ToString();
        if (string.IsNullOrWhiteSpace(parameterString))
            return null;
        return Enum.TryParse(targetType, parameterString, true, out var result) ? result : null;
    }
}

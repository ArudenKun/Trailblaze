using System.Globalization;
using Avalonia.Data.Converters;
using ZLinq;

namespace Trailblaze.Converters;

public class FromStringToEnumConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return null;
        var list = Enum.GetValues(value.GetType()).AsValueEnumerable().Cast<Enum>();
        return list.FirstOrDefault(vd => Equals(vd, value));
    }

    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        if (value is null)
            return null;
        var list = Enum.GetValues(value.GetType()).AsValueEnumerable().Cast<Enum>();
        return list.FirstOrDefault(vd => Equals(vd, value));
    }
}

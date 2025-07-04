using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Trailblaze.Converters;

public class RelayConverter : IValueConverter
{
    private readonly Func<object?, Type, object?, CultureInfo, object?> _convert;

    public RelayConverter(Func<object?, Type, object?, CultureInfo, object?> convert) =>
        _convert = convert;

    public RelayConverter(Func<object?, object?, object?> convert) =>
        _convert = (value, _, parameter, _) => convert(value, parameter);

    public RelayConverter(Func<object?, object?> convert) =>
        _convert = (value, _, _, _) => convert(value);

    #region IValueConverter Members

    public object? Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    ) => _convert(value, targetType, parameter, culture);

    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    ) => throw new NotImplementedException();

    #endregion

    [RequiresUnreferencedCode(
        "Conversion methods are required for type conversion, including op_Implicit, op_Explicit, Parse and TypeConverter."
    )]
    internal static object ConvertValue(Type targetType, object value)
    {
        if (targetType.IsInstanceOfType(value))
            return value;

        if (value is string str)
        {
            if (targetType == typeof(bool))
            {
                if (bool.TryParse(str, out var result))
                    return result;
                throw new InvalidOperationException(
                    "The requested bool value was not present in the provided type."
                );
            }

            if (targetType.IsEnum)
            {
                if (Enum.TryParse(targetType, str, out var result))
                    return result;

                throw new InvalidOperationException(
                    "The requested enum value was not present in the provided type."
                );
            }
        }

        return DefaultValueConverter.Instance.Convert(
                value,
                targetType,
                null,
                CultureInfo.CurrentCulture
            ) ?? value;
    }

    [RequiresUnreferencedCode(
        "Conversion methods are required for type conversion, including op_Implicit, op_Explicit, Parse and TypeConverter."
    )]
    internal static bool CompareValues(object? compare, object? value, Type? targetType)
    {
        if (compare == null || value == null)
            return compare == value;

        if (
            targetType == null
            || (targetType == compare.GetType() && targetType == value.GetType())
        )
            // Default direct object comparison or we're all the proper type
            return compare.Equals(value);

        if (compare.GetType() == targetType)
        {
            // If we have a TargetType and the first value is the right type
            // Then our 2nd value isn't, so convert to string and coerce.
            var valueBase2 = ConvertValue(targetType, value);

            return compare.Equals(valueBase2);
        }

        // Neither of our two values matches the type so
        // we'll convert both to a String and try and coerce it to the proper type.
        var compareBase = ConvertValue(targetType, compare);

        var valueBase = ConvertValue(targetType, value);

        return compareBase.Equals(valueBase);
    }
}

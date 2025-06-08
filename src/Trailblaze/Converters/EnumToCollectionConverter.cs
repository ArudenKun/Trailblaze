using System;
using Avalonia.Data.Converters;
using Trailblaze.Common.Extensions;

namespace Trailblaze.Converters;

public class EnumToCollectionConverter : IValueConverter
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
        var orderByName = System.Convert.ToBoolean(parameter);
        return value.GetType().GetAllValuesAndDescriptions(orderByName);
    }

    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        System.Globalization.CultureInfo culture
    )
    {
        return null;
        //string parameterString = parameter.ToString();
        //return Enum.Parse(targetType, parameterString);
    }
}

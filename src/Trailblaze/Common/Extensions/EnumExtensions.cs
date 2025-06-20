using System.Collections.Generic;
using System.Linq;
using Credfeto.Enumeration.Source.Generation.Attributes;
using SukiUI.Enums;

namespace Trailblaze.Common.Extensions;

[EnumText(typeof(SukiColor))]
public static partial class EnumExtensions
{
    public static IEnumerable<Enum> GetAllValues(this Type t, bool orderByName = false)
    {
        if (!t.IsEnum)
            throw new ArgumentException($"{nameof(t)} must be an enum type");

        return orderByName
            ? Enum.GetValues(t).Cast<Enum>().OrderBy(e => e?.ToString(), StringComparer.Ordinal)
            : Enum.GetValues(t).Cast<Enum>();
    }

    public static IEnumerable<TEnum> GetAllValues<TEnum>(bool orderByName = false)
        where TEnum : struct, Enum
    {
        var values = Enum.GetValues<TEnum>();
        return orderByName ? values.OrderBy(e => e.ToString(), StringComparer.Ordinal) : values;
    }

    /// <summary>
    /// Gets all the set flags of an enum value(s).
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="flags"></param>
    /// <returns></returns>
    /// <remarks>For enums with <see cref="FlagsAttribute"/>.<br/>
    /// If you have an enum value set to 0 it will always return. (Filter it before or after calling this function)<br/>
    /// If you have negative value it can return all flags as undesired effect.</remarks>
    public static IEnumerable<TEnum> GetSetFlags<TEnum>(this TEnum flags)
        where TEnum : struct, Enum
    {
        var flagsInt64 = Convert.ToInt64(flags);

        foreach (var flag in Enum.GetValues<TEnum>())
        {
            var flagInt64 = Convert.ToInt64(flag);
            if ((flagsInt64 & flagInt64) == flagInt64)
            {
                yield return flag;
            }
        }
    }

    /// <summary>
    /// Gets all the set flags of an enum value(s).
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="flags"></param>
    /// <param name="ignoreFlag">Set a flag to exclude from returning, eg: If you want to ignore or remove an always return 0 flag.</param>
    /// <returns></returns>
    /// <remarks>For enums with <see cref="FlagsAttribute"/>.<br/>
    /// If you have an enum value set to 0 it will always return. (Filter it before or after calling this function)<br/>
    /// If you have negative value it can return all flags as undesired effect.</remarks>
    public static IEnumerable<TEnum> GetSetFlagsIgnoring<TEnum>(this TEnum flags, TEnum ignoreFlag)
        where TEnum : struct, Enum
    {
        var flagsInt64 = Convert.ToInt64(flags);
        var ignoreFlagInt64 = Convert.ToInt64(ignoreFlag);

        flagsInt64 &= ~ignoreFlagInt64;

        foreach (var flag in Enum.GetValues<TEnum>())
        {
            var flagInt64 = Convert.ToInt64(flag);
            if ((flagsInt64 & flagInt64) == flagInt64)
            {
                yield return flag;
            }
        }
    }

    /// <summary>
    /// Gets all the set flags of an enum value(s).
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="flags"></param>
    /// <param name="ignoreFlags">Set flag(s) to exclude from returning, eg: If you want to ignore or remove an always return 0 flag.</param>
    /// <returns></returns>
    /// <remarks>For enums with <see cref="FlagsAttribute"/>.<br/>
    /// If you have an enum value set to 0 it will always return. (Filter it before or after calling this function)<br/>
    /// If you have negative value it can return all flags as undesired effect.</remarks>
    public static IEnumerable<TEnum> GetSetFlagsIgnoring<TEnum>(
        this TEnum flags,
        params IEnumerable<TEnum> ignoreFlags
    )
        where TEnum : struct, Enum
    {
        var flagsInt64 = Convert.ToInt64(flags);

        foreach (var ignoreFlag in ignoreFlags)
        {
            var ignoreFlagInt64 = Convert.ToInt64(ignoreFlag);
            flagsInt64 &= ~ignoreFlagInt64;
        }

        foreach (var flag in Enum.GetValues<TEnum>())
        {
            var flagInt64 = Convert.ToInt64(flag);
            if ((flagsInt64 & flagInt64) == flagInt64)
            {
                yield return flag;
            }
        }
    }
}

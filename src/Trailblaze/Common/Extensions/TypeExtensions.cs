using System.Collections.Generic;

namespace Trailblaze.Common.Extensions;

public static class TypeExtensions
{
    public static IEnumerable<Type> EnumerateBaseTypes(this Type t)
    {
        var baseType = t.BaseType;
        while (baseType is not null && baseType != typeof(object))
        {
            yield return baseType;
            baseType = baseType.BaseType;
        }
    }
}

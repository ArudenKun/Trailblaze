using System.Collections.Generic;
using System.Linq;

namespace Trailblaze.Common.Extensions;

public static class EnumerableExtensions
{
    public static List<T> AsList<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable is List<T> list)
        {
            return list;
        }

        return enumerable.ToList();
    }
}

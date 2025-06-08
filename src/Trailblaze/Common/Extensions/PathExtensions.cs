using System.Collections.Generic;
using System.IO;

namespace Trailblaze.Common.Extensions;

public static class PathExtensions
{
    public static string SanitizeFileName(this string source, char replacementChar = '_')
    {
        var blackList = new HashSet<char>(Path.GetInvalidFileNameChars()) { '"' }; // '"' not invalid in Linux, but causes problems
        var output = source.ToCharArray();
        for (int i = 0, ln = output.Length; i < ln; i++)
            if (blackList.Contains(output[i]))
                output[i] = replacementChar;

        return new string(output);
    }

    public static string CombinePath(this string path, params string[] parts)
    {
        var paths = new List<string> { path };
        paths.AddRange(parts);
        return Path.Combine([.. paths]);
    }
}

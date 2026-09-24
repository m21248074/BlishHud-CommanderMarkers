using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Manlaan.CommanderMarkers.Utils;

// Character-level Simplified -> Traditional conversion using OpenCC's STCharacters table (Apache-2.0,
// see assets/data/STCharacters.LICENSE.txt). Deliberately avoids a package dependency: Blish HUD hosts
// modules in one AppDomain with its own System.Text.Json, which breaks libraries built against newer versions.
public static class ChineseConverter
{
    private const string ResourceName = "STCharacters.txt";

    private static readonly Lazy<Dictionary<string, string>> Table = new(LoadTable);

    public static string ToTraditional(string? text)
    {
        if (string.IsNullOrEmpty(text))
            return text ?? string.Empty;

        var table = Table.Value;
        var sb = new StringBuilder(text!.Length);

        for (int i = 0; i < text.Length; i++)
        {
            int len = char.IsHighSurrogate(text[i]) && i + 1 < text.Length ? 2 : 1;
            string ch = text.Substring(i, len);
            sb.Append(table.TryGetValue(ch, out var traditional) ? traditional : ch);
            i += len - 1;
        }

        return sb.ToString();
    }

    private static Dictionary<string, string> LoadTable()
    {
        var table = new Dictionary<string, string>();

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(ResourceName);
        if (stream == null)
            return table;

        using var reader = new StreamReader(stream, Encoding.UTF8);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            var parts = line.Split('\t');
            if (parts.Length < 2)
                continue;

            // The first candidate is the preferred conversion.
            var candidate = parts[1].Split(' ')[0];
            if (parts[0].Length > 0 && candidate.Length > 0 && parts[0] != candidate)
                table[parts[0]] = candidate;
        }

        return table;
    }
}

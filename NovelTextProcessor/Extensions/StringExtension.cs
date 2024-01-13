namespace NovelTextProcessor.Extensions;

internal static class StringExtension
{
    public static string ReplaceFirst(this string value, string from, string to)
    {
        var position = value.IndexOf(from);
        if (position < 0) return value;
        return value.Substring(0, position) + to + value.Substring(position + from.Length);
    }
}
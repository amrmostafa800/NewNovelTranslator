namespace WebApp.Extensions;

public static class StringExtension
{
    public static int ToInt(this string value)
    {
        return int.Parse(value);
    }
}
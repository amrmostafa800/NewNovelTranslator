using System.Security.Cryptography;
using System.Text;

namespace NovelTextProcessor.Helpers;

internal class MD5Helper
{
    public static string NewMD5(string s)
    {
        var sb = new StringBuilder();

        // Initialize a MD5 hash object
        using (var md5 = MD5.Create())
        {
            // Compute the hash of the given string
            var hashValue = md5.ComputeHash(Encoding.UTF8.GetBytes(s));

            // Convert the byte array to string format
            foreach (var b in hashValue) sb.Append($"{b:X2}");
        }

        return sb.ToString().ToLower();
    }
}
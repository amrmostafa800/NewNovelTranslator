using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelTextProcessor.Extensions
{
    static class StringExtension
    {
        public static string ReplaceFirst(this string value, string from, string to)
        {
            int position = value.IndexOf(from);
            if (position < 0)
            {
                return value;
            }
            value = value.Substring(0, position) + to + value.Substring(position + from.Length);
            return value;
        }
    }
}

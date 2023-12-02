using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelTextProcessor.Extensions
{
    static class StringBuilderExtension
    {
        public static void ReplaceFromTwoIndexToNewText(this StringBuilder value, int fromIndex, int toIndex, string newText)
        {
            var textBetween2Index = value.ToString().Substring(fromIndex, toIndex - fromIndex);
            value.Replace(textBetween2Index, newText);
        }
    }
}

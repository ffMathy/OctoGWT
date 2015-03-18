using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoGWT.Extensions
{
    internal static class StringExtensions
    {
        public static IList<string> ExtractWords(this string input)
        {
            var result = new List<string>();

            var word = string.Empty;
            foreach(var character in input)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    if (character == ' ')
                    {
                        result.Add(word);
                        word = string.Empty;
                        continue;
                    }
                    else if (char.IsUpper(character))
                    {
                        result.Add(word);
                        word = string.Empty;
                    }
                }

                word += character;
            }

            if(!string.IsNullOrEmpty(word))
            {
                result.Add(word);
            }

            return result;
        }
    }
}

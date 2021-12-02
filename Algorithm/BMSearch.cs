using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    static class BMSearch
    {
        public static int IndexOf<T>(ReadOnlySpan<T> text, ReadOnlySpan<T> pattern) where T : IEquatable<T>
        {
            if (text.IsEmpty)
                throw new ArgumentNullException();

            if (pattern.Length == 0)
            {
                return 0;
            }

            var charTable = makeCharTable(pattern);
            var last = pattern.Length - 1;
            for (var pos = last; pos < text.Length; /* nop*/ )
            {
                var i = pos;
                var j = last;
                while (j >= 0)
                {
                    if (!text[i].Equals(pattern[j]))
                    {
                        break;
                    }

                    i--;
                    j--;
                }
                if (j < 0)
                    return i + 1;

                pos += charTable.TryGetValue(text[pos], out int offset) ? offset : pattern.Length;
            }
            return -1;
        }
        private static Dictionary<T, int> makeCharTable<T>(ReadOnlySpan<T> needle) where T : IEquatable<T>
        {
            var charTable = new Dictionary<T, int>();
            for (var i = 0; i < needle.Length - 1; i++)
            {
                charTable[needle[i]] = needle.Length - i - 1;
            }
            return charTable;
        }
    }
}
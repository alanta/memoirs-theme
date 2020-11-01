using System.Collections.Generic;
using System.Linq;

namespace MemoirsTheme.Helpers
{
    public static class TextExtensions
    {
        public static string GetMaxWords(this string input, int maxWords, string truncateWith = "…", string additionalSeparators = ",-_:")
        {
            int words = 1;
            bool IsSeparator(char c) => char.IsSeparator(c) || additionalSeparators.Contains(c);

            IEnumerable<char> IterateChars()
            {
                yield return input[0];

                for (int i = 1; i < input.Length; i++)
                {
                    if (IsSeparator(input[i]) && !IsSeparator(input[i - 1]))
                        if (words == maxWords)
                        {
                            foreach (char c in truncateWith)
                                yield return c;

                            break;
                        }
                        else
                            words++;

                    yield return input[i];
                }
            }

            return !string.IsNullOrEmpty(input)
                ? new string(IterateChars().ToArray())
                : string.Empty;
        }
    }
}
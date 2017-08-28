using System;
using System.Text.RegularExpressions;

namespace InstagramPhotos.Utility.IO
{
    public abstract class StringSearcher
    {
        // Methods

        public static StringSearcher GetStringSearcher(string searchWord, bool wholeMatch, bool caseSensitive)
        {
            if (wholeMatch)
            {
                if (caseSensitive)
                {
                    return new StringSearcher_WholeMatch_Y_CaseSensitive_Y(searchWord);
                }
                return new StringSearcher_WholeMatch_Y_CaseSensitive_N(searchWord);
            }
            if (caseSensitive)
            {
                return new StringSearcher_WholeMatch_N_CaseSensitive_Y(searchWord);
            }
            return new StringSearcher_WholeMatch_N_CaseSensitive_N(searchWord);
        }

        public virtual bool IsMatch(string input)
        {
            throw new NotImplementedException();
        }

        // Nested Types
        private sealed class StringSearcher_WholeMatch_N_CaseSensitive_N : StringSearcher
        {
            // Fields
            private readonly string m_searchWord;

            // Methods
            internal StringSearcher_WholeMatch_N_CaseSensitive_N(string searchWord)
            {
                m_searchWord = searchWord;
            }

            public override bool IsMatch(string input)
            {
                return (input.IndexOf(m_searchWord, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private sealed class StringSearcher_WholeMatch_N_CaseSensitive_Y : StringSearcher
        {
            // Fields
            private readonly string m_searchWord;

            // Methods
            internal StringSearcher_WholeMatch_N_CaseSensitive_Y(string searchWord)
            {
                m_searchWord = searchWord;
            }

            public override bool IsMatch(string input)
            {
                return (input.IndexOf(m_searchWord, StringComparison.Ordinal) >= 0);
            }
        }

        private sealed class StringSearcher_WholeMatch_Y_CaseSensitive_N : StringSearcher
        {
            // Fields
            private readonly Regex m_regex;

            // Methods
            internal StringSearcher_WholeMatch_Y_CaseSensitive_N(string searchWord)
            {
                m_regex = new Regex(@"\b" + searchWord + @"\b", RegexOptions.IgnoreCase);
            }

            public override bool IsMatch(string input)
            {
                return m_regex.IsMatch(input);
            }
        }

        private sealed class StringSearcher_WholeMatch_Y_CaseSensitive_Y : StringSearcher
        {
            // Fields
            private readonly Regex m_regex;

            // Methods
            internal StringSearcher_WholeMatch_Y_CaseSensitive_Y(string searchWord)
            {
                m_regex = new Regex(@"\b" + searchWord + @"\b", RegexOptions.None);
            }

            public override bool IsMatch(string input)
            {
                return m_regex.IsMatch(input);
            }
        }
    }
}
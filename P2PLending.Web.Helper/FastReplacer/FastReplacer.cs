using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Helper
{
    /// <summary>
    /// FastReplacer is a utility class similar to StringBuilder, with fast Replace function.
    /// FastReplacer is limited to replacing only properly formatted tokens.
    /// Use ToString() function to get the final text.
    /// </summary>
    public class FastReplacer
    {
        public readonly string TokenOpen;
        public readonly string TokenClose;

        /// <summary>
        /// All tokens that will be replaced must have same opening and closing delimiters, such as "{" and "}".
        /// </summary>
        /// <param name="tokenOpen">Opening delimiter for tokens.</param>
        /// <param name="tokenClose">Closing delimiter for tokens.</param>
        /// <param name="caseSensitive">Set caseSensitive to false to use case-insensitive search when replacing tokens.</param>
        public FastReplacer(string tokenOpen, string tokenClose) : this(tokenOpen, tokenClose, true)
        {
        }

        public FastReplacer(string tokenOpen, string tokenClose, bool caseSensitive)
        {
            if (string.IsNullOrEmpty(tokenOpen) || string.IsNullOrEmpty(tokenClose))
            {
                throw new ArgumentException("Token must have opening and closing delimiters, such as \"{\" and \"}\".");
            }

            TokenOpen = tokenOpen;
            TokenClose = tokenClose;

            var stringComparer = caseSensitive ? StringComparer.Ordinal : StringComparer.InvariantCultureIgnoreCase;
            _occurrencesOfToken = new Dictionary<string, List<TokenOccurrence>>(stringComparer);
        }

        private readonly FastReplacerSnippet _rootSnippet = new FastReplacerSnippet("");

        private class TokenOccurrence
        {
            public FastReplacerSnippet Snippet;
            public int Start; // Position of a token in the snippet.
            public int End; // Position of a token in the snippet.
        }

        private readonly Dictionary<string, List<TokenOccurrence>> _occurrencesOfToken;

        public void Append(string text)
        {
            var s = new FastReplacerSnippet(text);
            _rootSnippet.Append(s);
            ExtractTokens(s);
        }

        /// <returns>Returns true if the token was found, false if nothing was replaced.</returns>
        public bool Replace(string token, string text)
        {
            ValidateToken(token, text, false);

            if (_occurrencesOfToken.TryGetValue(token, out var occurrences))
            {
                _occurrencesOfToken.Remove(token);

                var s = new FastReplacerSnippet(text);

                foreach (var occurrence in occurrences)
                {
                    occurrence.Snippet.Replace(occurrence.Start, occurrence.End, s);
                }

                ExtractTokens(s);

                return occurrences.Count > 0;
            }

            return false;
        }

        /// <returns>Returns true if the token was found, false if nothing was replaced.</returns>
        public bool InsertBefore(string token, string text)
        {
            ValidateToken(token, text, false);

            if (!_occurrencesOfToken.TryGetValue(token, out var occurrences))
            {
                return false;
            }

            var s = new FastReplacerSnippet(text);

            foreach (var occurrence in occurrences)
            {
                occurrence.Snippet.InsertBefore(occurrence.Start, s);
            }

            ExtractTokens(s);

            return occurrences.Count > 0;

        }

        /// <returns>Returns true if the token was found, false if nothing was replaced.</returns>
        public bool InsertAfter(string token, string text)
        {
            ValidateToken(token, text, false);

            if (!_occurrencesOfToken.TryGetValue(token, out var occurrences))
            {
                return false;
            }

            var s = new FastReplacerSnippet(text);

            foreach (var occurrence in occurrences)
            {
                occurrence.Snippet.InsertAfter(occurrence.End, s);
            }

            ExtractTokens(s);

            return occurrences.Count > 0;

        }

        public bool Contains(string token)
        {
            ValidateToken(token, token, false);

            if (_occurrencesOfToken.TryGetValue(token, out var occurrences))
            {
                return occurrences.Count > 0;
            }

            return false;
        }

        private void ExtractTokens(FastReplacerSnippet snippet)
        {
            if (snippet.Text == null)
            {
                return;
            }

            var last = 0;

            while (last < snippet.Text.Length)
            {
                var start = snippet.Text.IndexOf(TokenOpen, last, StringComparison.Ordinal);

                if (start == -1)
                {
                    return;
                }

                var end = snippet.Text.IndexOf(TokenClose, start + TokenOpen.Length, StringComparison.Ordinal);

                if (end == -1)
                {
                    throw new ArgumentException(string.Format("Token is opened but not closed in text \"{0}\".", snippet.Text));
                }                    

                end += TokenClose.Length;

                var token = snippet.Text.Substring(start, end - start);
                var context = snippet.Text;

                ValidateToken(token, context, true);

                var tokenOccurrence = new TokenOccurrence { Snippet = snippet, Start = start, End = end };

                if (_occurrencesOfToken.TryGetValue(token, out var occurrences))
                {
                    occurrences.Add(tokenOccurrence);
                }
                else
                {
                    _occurrencesOfToken.Add(token, new List<TokenOccurrence> { tokenOccurrence });
                }

                last = end;
            }
        }

        private void ValidateToken(string token, string context, bool alreadyValidatedStartAndEnd)
        {
            if (!alreadyValidatedStartAndEnd)
            {
                if (!token.StartsWith(TokenOpen))
                {
                    throw new ArgumentException(string.Format("Token \"{0}\" shoud start with \"{1}\". Used with text \"{2}\".", token, TokenOpen, context));
                }

                var closePosition = token.IndexOf(TokenClose, StringComparison.Ordinal);

                if (closePosition == -1)
                {
                    throw new ArgumentException(string.Format("Token \"{0}\" should end with \"{1}\". Used with text \"{2}\".", token, TokenClose, context));
                }

                if (closePosition != token.Length - TokenClose.Length)
                {
                    throw new ArgumentException(string.Format("Token \"{0}\" is closed before the end of the token. Used with text \"{1}\".", token, context));
                }
            }

            if (token.Length == TokenOpen.Length + TokenClose.Length)
            {
                throw new ArgumentException(string.Format("Token has no body. Used with text \"{0}\".", context));
            }

            if (token.Contains("\n"))
            {
                throw new ArgumentException(string.Format("Unexpected end-of-line within a token. Used with text \"{0}\".", context));
            }

            if (token.IndexOf(TokenOpen, TokenOpen.Length, StringComparison.Ordinal) != -1)
            {
                throw new ArgumentException(string.Format("Next token is opened before a previous token was closed in token \"{0}\". Used with text \"{1}\".", token, context));
            }
        }

        public override string ToString()
        {
            var totalTextLength = _rootSnippet.GetLength();

            var sb = new StringBuilder(totalTextLength);

            _rootSnippet.ToString(sb);

            if (sb.Length != totalTextLength)
            {
                throw new ArgumentException(string.Format(
                    "Internal error: Calculated total text length ({0}) is different from actual ({1}).",
                    totalTextLength, sb.Length));
            }
                

            return sb.ToString();
        }
    }
}

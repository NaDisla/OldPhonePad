using System.Text;

namespace OldPhonePad
{
    /// <summary>
    /// Converts old mobile keypad (multi‑tap) sequences to text.
    /// - All whitespace acts as a "pause" (not only space), because real input can come from files or pastes.
    /// - Unknown digits like '1' are ignored (lenient), but this behavior is documented and easily toggled.
    /// </summary>
    public static class OldPhonePadConverter
    {
        private static readonly Dictionary<char, string> Map = new()
        {
            ['2'] = "ABC",
            ['3'] = "DEF",
            ['4'] = "GHI",
            ['5'] = "JKL",
            ['6'] = "MNO",
            ['7'] = "PQRS",
            ['8'] = "TUV",
            ['9'] = "WXYZ",
            ['0'] = " "
        };

        /// <summary>
        /// Public entry point with sensible defaults.
        /// </summary>
        public static string OldPhonePad(string input) =>
            OldPhonePad(input, new OldPhonePadOptions());

        /// <summary>
        /// Overload with options to make behavior explicit in tests or extensions.
        /// </summary>
        public static string OldPhonePad(string input, OldPhonePadOptions options)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            var output = new StringBuilder();
            char currentDigit = '\0';
            int count = 0;

            void FlushGroup()
            {
                if (currentDigit == '\0' || count == 0)
                    return;

                if (Map.TryGetValue(currentDigit, out var letters))
                {
                    var idx = (count - 1) % letters.Length;
                    output.Append(letters[idx]);
                }
                else if (options.ThrowOnUnknownDigit)
                {
                    throw new ArgumentException($"Unsupported digit: '{currentDigit}'");
                }

                currentDigit = '\0';
                count = 0;
            }

            foreach (var ch in input)
            {
                if (ch == '#')
                {
                    FlushGroup();
                    break;
                }

                if (ch == '*')
                {
                    FlushGroup();
                    if (output.Length > 0)
                        output.Remove(output.Length - 1, 1);
                    continue;
                }

                if (char.IsWhiteSpace(ch))
                {
                    FlushGroup();
                    continue;
                }

                if (Map.ContainsKey(ch))
                {
                    if (currentDigit == ch)
                    {
                        count++;
                    }
                    else
                    {
                        FlushGroup();
                        currentDigit = ch;
                        count = 1;
                    }
                    continue;
                }

                if (options.ZeroAsSpace && ch == '0')
                {
                    FlushGroup();
                    output.Append(' ');
                    continue;
                }

                if (options.ThrowOnUnknownDigit)
                    throw new ArgumentException($"Unsupported character: '{ch}'");
            }

            return output.ToString();
        }
    }

    /// <summary>
    /// Options to keep behavior explicit and testable.
    /// </summary>
    public sealed class OldPhonePadOptions
    {
        /// <summary>
        /// When true, '0' is emitted as a space in the output.
        /// </summary>
        public bool ZeroAsSpace { get; init; } = false;

        /// <summary>
        /// When true, encountering an unknown digit/char (e.g. '1') throws instead of ignoring.
        /// </summary>
        public bool ThrowOnUnknownDigit { get; init; } = false;
    }
}

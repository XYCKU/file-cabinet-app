using System;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Converts values.
    /// </summary>
    public static class InputConverter
    {
        private static readonly CultureInfo DefaultCulture = new CultureInfo("en-US");

        /// <summary>
        /// Converts to int.
        /// </summary>
        /// <param name="input">Input int.</param>
        /// <returns>Tuple with result.</returns>
        public static Tuple<bool, string, int> IntConverter(string input)
        {
            return new Tuple<bool, string, int>(int.TryParse(input, out int result), "Invalid int.", result);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Tuple with result.</returns>
        public static Tuple<bool, string, string> StringConverter(string input)
        {
            return new Tuple<bool, string, string>(true, string.Empty, input);
        }

        /// <summary>
        /// Converts to DateTime.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Tuple with result.</returns>
        public static Tuple<bool, string, DateTime> DateConverter(string input)
        {
            return new Tuple<bool, string, DateTime>(
                DateTime.TryParse(input, DefaultCulture, DateTimeStyles.None, out DateTime result),
                "Invalid date of birth.",
                result);
        }

        /// <summary>
        /// Converts to char.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Tuple with result.</returns>
        public static Tuple<bool, string, char> CharConverter(string input)
        {
            return new Tuple<bool, string, char>(char.TryParse(input, out char result), "Invalid char.", char.ToUpperInvariant(result));
        }

        /// <summary>
        /// Converts to short.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Tuple with result.</returns>
        public static Tuple<bool, string, short> ShortConverter(string input)
        {
            return new Tuple<bool, string, short>(short.TryParse(input, out short result), "Invalid short.", result);
        }

        /// <summary>
        /// Converts to decimal.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Tuple with result.</returns>
        public static Tuple<bool, string, decimal> DecimalConverter(string input)
        {
            return new Tuple<bool, string, decimal>(decimal.TryParse(input, out decimal result), "Invalid decimal.", result);
        }
    }
}

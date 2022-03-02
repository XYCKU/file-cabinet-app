using System;
using System.Globalization;

namespace FileCabinetApp
{
    /// <inheritdoc/>
    public class DefaultRecordPrinter : IRecordPrinter
    {
        private const string DateTimeFormat = "MM/dd/yyyy";

        /// <inheritdoc/>
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            foreach (var record in records)
            {
                Console.WriteLine(FormatRecord(record));
            }
        }

        private static string FormatRecord(FileCabinetRecord record) =>
            $"#{record.Id}, " +
            $"{record.FirstName}, " +
            $"{record.LastName}, " +
            $"{record.DateOfBirth.ToString(DateTimeFormat, CultureInfo.InvariantCulture)}, " +
            $"{record.CarAmount}, " +
            $"{record.Money}, " +
            $"{record.FavoriteChar}";
    }
}

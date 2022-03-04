using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Writes <see cref="FileCabinetRecord"/> to CSV file using <see cref="TextWriter"/>.
    /// </summary>
    public class FileCabinetRecordCsvWriter : IFileCabinetRecordWriter
    {
        private static readonly CultureInfo CultureInfo = new CultureInfo("en-US");
        private readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer"><see cref="TextWriter"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="writer"/> is <c>null</c>.</exception>
        public FileCabinetRecordCsvWriter(StreamWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            this.writer = writer;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="record"/> is <c>null</c>.</exception>
        public void Write(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            this.writer.WriteLine(FormatRecord(record));
        }

        private static string FormatRecord(FileCabinetRecord record) => $"{record.Id}," +
            $"{record.FirstName}," +
            $"{record.LastName}," +
            $"{record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo)}," +
            $"{record.CarAmount}," +
            $"{record.Money.ToString(CultureInfo.InvariantCulture)}," +
            $"{record.FavoriteChar}";
    }
}

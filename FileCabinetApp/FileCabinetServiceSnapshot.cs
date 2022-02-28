using System;
using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Snapshot of <see cref="FileCabinetMemoryService"/>.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">Array of <see cref="FileCabinetRecord"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="records"/> is <c>null</c>.</exception>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            this.records = records;
        }

        /// <summary>
        /// Gets <see cref="ReadOnlyCollection{FileCabinetRecord}"/> of records.
        /// </summary>
        /// <value>
        /// <see cref="ReadOnlyCollection{FileCabinetRecord}"/> of records.
        /// </value>
        public ReadOnlyCollection<FileCabinetRecord> Records => Array.AsReadOnly(this.records);

        /// <summary>
        /// Saves <see cref="FileCabinetServiceSnapshot"/> to CSV format.
        /// </summary>
        /// <param name="writer"><see cref="StreamWriter"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown whe <paramref name="writer"/> is <c>null</c>.</exception>
        public void SaveToCsv(StreamWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            IFileCabinetRecordWriter csvRecordWriter = new FileCabinetRecordCsvWriter(writer);

            const string FormatLine = "Id,First Name,Last Name,Date of Birth,Car Amount,Money,Favorite char";
            writer.WriteLine(FormatLine);

            for (int i = 0; i < this.records.Length; ++i)
            {
                csvRecordWriter.Write(this.records[i]);
            }
        }

        /// <summary>
        /// Saves <see cref="FileCabinetServiceSnapshot"/> to XML format.
        /// </summary>
        /// <param name="writer"><see cref="StreamWriter"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="writer"/> is <c>null</c>.</exception>
        public void SaveToXml(StreamWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            IFileCabinetRecordWriter xmlRecordWriter = new FileCabinetRecordXmlWriter(writer);

            for (int i = 0; i < this.records.Length; ++i)
            {
                xmlRecordWriter.Write(this.records[i]);
            }
        }

        /// <summary>
        /// Loads <see cref="FileCabinetRecord"/> array from csv.
        /// </summary>
        /// <param name="reader"><see cref="StreamReader"/> to read from.</param>
        public void LoadFromCsv(StreamReader reader)
        {
            var csvReader = new FileCabinetRecordCsvReader(reader);

            this.records = csvReader.ReadAll().ToArray();
        }

        /// <summary>
        /// Loads <see cref="FileCabinetRecord"/> array from xml.
        /// </summary>
        /// <param name="reader"><see cref="StreamReader"/> to read from.</param>
        public void LoadFromXml(StreamReader reader)
        {
            throw new NotSupportedException();
        }
    }
}

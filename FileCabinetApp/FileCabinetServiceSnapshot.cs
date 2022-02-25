using System;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// Snapshot of <see cref="FileCabinetService"/>.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private readonly FileCabinetRecord[] records;

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
        /// <exception cref="ArgumentNullException">Thrown whe <paramref name="writer"/> is <c>null</c>.</exception>
        public void SaveToXml(StreamWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            IFileCabinetRecordWriter xmlRecordWriter = new FileCabinetRecordXmlWriter(XmlWriter.Create(writer));

            for (int i = 0; i < this.records.Length; ++i)
            {
                xmlRecordWriter.Write(this.records[i]);
            }
        }
    }
}

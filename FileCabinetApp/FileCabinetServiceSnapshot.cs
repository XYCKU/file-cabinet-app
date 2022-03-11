using System;
using System.Collections.ObjectModel;
using FileCabinetApp.Validators.Input;

namespace FileCabinetApp
{
    /// <summary>
    /// Snapshot of <see cref="FileCabinetMemoryService"/>.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private readonly IInputValidator inputValidator = new DefaultInputValidator();
        private ReadOnlyCollection<FileCabinetRecord> records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">Array of <see cref="FileCabinetRecord"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="records"/> is <c>null</c>.</exception>
        public FileCabinetServiceSnapshot(ReadOnlyCollection<FileCabinetRecord> records)
        {
            this.records = records ?? throw new ArgumentNullException(nameof(records));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">Array of <see cref="FileCabinetRecord"/>.</param>
        /// <param name="inputValidator">Array of <see cref="IInputValidator"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="records"/> is <c>null</c>.</exception>
        public FileCabinetServiceSnapshot(ReadOnlyCollection<FileCabinetRecord> records, IInputValidator inputValidator)
            : this(records)
        {
            this.inputValidator = inputValidator ?? throw new ArgumentNullException(nameof(inputValidator));
        }

        /// <summary>
        /// Gets <see cref="ReadOnlyCollection{FileCabinetRecord}"/> of records.
        /// </summary>
        /// <value>
        /// <see cref="ReadOnlyCollection{FileCabinetRecord}"/> of records.
        /// </value>
        public ReadOnlyCollection<FileCabinetRecord> Records => this.records;

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

            const string formatLine = "Id,First Name,Last Name,Date of Birth,Car Amount,Money,Favorite char";
            writer.WriteLine(formatLine);

            for (int i = 0; i < this.records.Count; ++i)
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

            using (var xmlRecordWriter = new FileCabinetRecordXmlWriter(writer))
            {
                for (int i = 0; i < this.records.Count; ++i)
                {
                    xmlRecordWriter.Write(this.records[i]);
                }
            }
        }

        /// <summary>
        /// Loads <see cref="FileCabinetRecord"/> array from csv.
        /// </summary>
        /// <param name="reader"><see cref="StreamReader"/> to read from.</param>
        public void LoadFromCsv(StreamReader reader)
        {
            var csvReader = new FileCabinetRecordCsvReader(reader, this.inputValidator);

            this.records = new ReadOnlyCollection<FileCabinetRecord>(csvReader.ReadAll());
        }

        /// <summary>
        /// Loads <see cref="FileCabinetRecord"/> array from xml.
        /// </summary>
        /// <param name="reader"><see cref="StreamReader"/> to read from.</param>
        public void LoadFromXml(StreamReader reader)
        {
            var xmlReader = new FileCabinetRecordXmlReader(reader);

            this.records = new ReadOnlyCollection<FileCabinetRecord>(xmlReader.ReadAll());
        }
    }
}

using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <inheritdoc/>
    public class FileCabinetRecordXmlReader : IFileCabinetRecordReader
    {
        private const string DateTimeFormat = "MM/dd/yyyy";
        private static readonly IRecordValidator Validator = new DefaultValidator();
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(FileCabinetRecord));
        private readonly XmlReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="reader"><see cref="StreamReader"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="reader"/> is <c>null</c>.</exception>
        public FileCabinetRecordXmlReader(StreamReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            this.reader = XmlReader.Create(reader);
        }

        /// <inheritdoc/>
        public IList<FileCabinetRecord> ReadAll()
        {
            var records = new List<FileCabinetRecord>();
            this.reader.ReadToFollowing(nameof(FileCabinetRecord));
            do
            {
                var record = Serializer.Deserialize(this.reader) as FileCabinetRecord;

                if (record is null)
                {
                    continue;
                }

                records.Add(record);
            }
            while (this.reader.ReadToFollowing(nameof(FileCabinetRecord)));

            return records.ToList();
        }
    }
}

using System;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Writes <see cref="FileCabinetRecord"/> to XML file using <see cref="System.Xml.Serialization.XmlSerializer"/>.
    /// </summary>
    public class FileCabinetRecordXmlWriter : IFileCabinetRecordWriter
    {
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(FileCabinetRecord));
        private readonly XmlWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer"><see cref="XmlWriter"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="writer"/> is <c>null</c>.</exception>
        public FileCabinetRecordXmlWriter(StreamWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            this.writer = XmlWriter.Create(writer, new XmlWriterSettings() { Indent = true, NewLineOnAttributes = true });
            this.writer.WriteStartDocument();
            this.writer.WriteStartElement("Records");
        }

        /// <inheritdoc/>
        public void Write(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            Serializer.Serialize(this.writer, record);

            this.writer.Flush();
        }
    }
}

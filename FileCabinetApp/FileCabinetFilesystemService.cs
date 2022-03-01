using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <inheritdoc/>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private const int RecordSize = 4 + 120 + 120 + 4 + 4 + 4 + 2 + 8 + 1 + 1;
        private static readonly Encoding Encoding = Encoding.UTF8;
        private static readonly int[] DataSizes = { 1, 4, 120, 120, 4, 4, 4, 2, 8, 1 };
        private readonly FileStream fileStream;
        private int count;
        private int deleted;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream"><see cref="FileStream"/>.</param>
        /// <param name="validator"><see cref="IRecordValidator"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="fileStream"/> is <c>null</c>.</exception>
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator)
        {
            if (fileStream is null)
            {
                throw new ArgumentNullException(nameof(fileStream));
            }

            if (validator is null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            this.fileStream = fileStream;
            this.Validator = validator;

            this.count = this.GetStat().Item1;
        }

        /// <inheritdoc/>
        public IRecordValidator Validator { get; }

        /// <inheritdoc/>
        public int CreateRecord(FileCabinetData data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.Validator.ValidateParameters(data);

            var record = new FileCabinetRecord(this.count++, data);

            byte[] bytes = ToBytes(record);

            this.fileStream.Seek(0, SeekOrigin.End);

            this.fileStream.Write(bytes);
            this.fileStream.Flush();

            return record.Id;
        }

        /// <inheritdoc/>
        public void EditRecord(int id, FileCabinetData data)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "id cannot be less than 0");
            }

            this.Validator.ValidateParameters(data);

            int index = this.FindById(id);

            if (index < 0)
            {
                throw new ArgumentException($"Record #{id} is not found", nameof(id));
            }

            this.fileStream.Position = index * RecordSize;

            byte[] newByteData = ToBytes(data, id);

            this.fileStream.Seek(-RecordSize, SeekOrigin.Current);
            this.fileStream.Write(newByteData);
            this.fileStream.Seek(0, SeekOrigin.End);
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            return this.FindBy((FileCabinetRecord record) => record.DateOfBirth == dateOfBirth);
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            return this.FindBy((FileCabinetRecord record) => string.Equals(record.FirstName, firstName, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            return this.FindBy((FileCabinetRecord record) => string.Equals(record.LastName, lastName, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            byte[] bytes = new byte[RecordSize];
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();

            this.fileStream.Seek(0, SeekOrigin.Begin);

            for (int i = 0; i < this.count; ++i)
            {
                if (this.fileStream.Read(bytes, 0, RecordSize) > 0)
                {
                    if (bytes[0] == 1)
                    {
                        continue;
                    }

                    result.Add(ToRecord(bytes.AsSpan()[1..]));
                }
                else
                {
                    this.count = i;
                    break;
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        /// <inheritdoc/>
        public Tuple<int, int> GetStat()
        {
            return new Tuple<int, int>(this.count, this.deleted);
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.GetRecords().ToArray());
        }

        /// <inheritdoc/>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            FileCabinetRecord[] records = snapshot.Records.ToArray();

            this.count = records.Length;
            this.deleted = 0;

            this.fileStream.Position = 0;

            for (int i = 0; i < records.Length; ++i)
            {
                byte[] bytes = ToBytes(records[i]);

                this.fileStream.Write(bytes);
            }

            this.fileStream.Flush();
        }

        /// <inheritdoc/>
        public void RemoveRecord(int id)
        {
            int index = this.FindById(id);

            if (index == -1)
            {
                throw new ArgumentException($"Record #{id} is not found", nameof(id));
            }

            this.fileStream.Position = index * RecordSize;
            this.fileStream.Write(new byte[] { 1 });
            ++this.deleted;
        }

        /// <summary>
        /// Purges file records.
        /// </summary>
        public void PurgeRecords()
        {
            if (this.deleted == 0)
            {
                return;
            }

            FileCabinetServiceSnapshot snapshot = new FileCabinetServiceSnapshot(this.GetRecords().ToArray());
            this.Restore(snapshot);
        }

        /// <inheritdoc/>
        public override string ToString() => "file";

        private static FileCabinetRecord ToRecord(Span<byte> spanBytes)
        {
            int id = BitConverter.ToInt32(spanBytes[0..4]);

            string firstName = Encoding.GetString(spanBytes[4..124]);
            firstName = firstName[..firstName.IndexOf('\0', StringComparison.Ordinal)];

            string lastName = Encoding.GetString(spanBytes[124..244]);
            lastName = lastName[..lastName.IndexOf('\0', StringComparison.Ordinal)];

            int year = BitConverter.ToInt32(spanBytes[244..248]);
            int month = BitConverter.ToInt32(spanBytes[248..252]);
            int day = BitConverter.ToInt32(spanBytes[252..256]);

            DateTime dob = new DateTime(year, month, day);

            short carAmount = BitConverter.ToInt16(spanBytes[256..258]);

            decimal money = (decimal)BitConverter.ToDouble(spanBytes[258..266]);

            char favoriteChar = BitConverter.ToChar(new byte[] { spanBytes[266], 0 });

            var record = new FileCabinetRecord(id, firstName, lastName, dob, carAmount, money, favoriteChar);

            return record;
        }

        private static byte[] ToBytes(FileCabinetRecord record)
        {
            byte[] bytes = new byte[RecordSize];

            var dataInfo = new byte[][]
            {
                new byte[] { 0 },
                BitConverter.GetBytes(record.Id),
                Encoding.GetBytes(record.FirstName),
                Encoding.GetBytes(record.LastName),
                BitConverter.GetBytes(record.DateOfBirth.Year),
                BitConverter.GetBytes(record.DateOfBirth.Month),
                BitConverter.GetBytes(record.DateOfBirth.Day),
                BitConverter.GetBytes(record.CarAmount),
                BitConverter.GetBytes((double)record.Money),
                BitConverter.GetBytes(record.FavoriteChar),
            };

            int offset = 0;
            for (int i = 0; i < dataInfo.Length; ++i)
            {
                Array.Copy(dataInfo[i], 0, bytes, offset, Math.Min(DataSizes[i], dataInfo[i].Length));
                offset += DataSizes[i];
            }

            return bytes;
        }

        private static byte[] ToBytes(FileCabinetData data, int id)
        {
            byte[] bytes = new byte[RecordSize];

            var dataInfo = new byte[][]
            {
                new byte[] { 0 },
                BitConverter.GetBytes(id),
                Encoding.GetBytes(data.FirstName),
                Encoding.GetBytes(data.LastName),
                BitConverter.GetBytes(data.DateOfBirth.Year),
                BitConverter.GetBytes(data.DateOfBirth.Month),
                BitConverter.GetBytes(data.DateOfBirth.Day),
                BitConverter.GetBytes(data.CarAmount),
                BitConverter.GetBytes((double)data.Money),
                BitConverter.GetBytes(data.FavoriteChar),
            };

            int offset = 0;
            for (int i = 0; i < dataInfo.Length; ++i)
            {
                Array.Copy(dataInfo[i], 0, bytes, offset, Math.Min(DataSizes[i], dataInfo[i].Length));
                offset += DataSizes[i];
            }

            return bytes;
        }

        private ReadOnlyCollection<FileCabinetRecord> FindBy(Predicate<FileCabinetRecord> predicate)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();

            var allRecords = this.GetRecords();

            for (int i = 0; i < allRecords.Count; ++i)
            {
                if (predicate(allRecords[i]))
                {
                    result.Add(allRecords[i]);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        private int FindById(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "id cannot be less than 0");
            }

            byte[] bytes = new byte[RecordSize];

            int index = 0;
            this.fileStream.Position = 0;

            for (int i = 0; i < this.count; ++i, ++index)
            {
                if (this.fileStream.Read(bytes, 0, RecordSize) > 0)
                {
                    if (bytes[0] == 1)
                    {
                        continue;
                    }

                    var record = ToRecord(bytes.AsSpan()[1..]);

                    if (id == record.Id)
                    {
                        return index;
                    }
                }
            }

            return -1;
        }
    }
}

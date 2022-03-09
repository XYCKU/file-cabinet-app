using System.Collections.ObjectModel;
using System.Text;
using FileCabinetApp.Validators;
using FileCabinetApp.Validators.Input;

namespace FileCabinetApp
{
    /// <inheritdoc/>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private const int RecordSize = 4 + 120 + 120 + 4 + 4 + 4 + 2 + 8 + 1 + 1;

        private static readonly Encoding Encoding = Encoding.UTF8;
        private static readonly int[] DataSizes = { 1, 4, 120, 120, 4, 4, 4, 2, 8, 1 };

        private readonly FileStream fileStream;
        private readonly Dictionary<string, List<int>> firstNameDictionary = new Dictionary<string, List<int>>();
        private readonly Dictionary<string, List<int>> lastNameDictionary = new Dictionary<string, List<int>>();
        private readonly Dictionary<DateTime, List<int>> dateOfBirthDictionary = new Dictionary<DateTime, List<int>>();

        private int count;
        private int deleted;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream"><see cref="FileStream"/>.</param>
        /// <param name="validator"><see cref="IRecordValidator"/>.</param>
        /// <param name="inputValidator"><see cref="IInputValidator"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="fileStream"/> is <c>null</c>.</exception>
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator, IInputValidator inputValidator)
        {
            this.fileStream = fileStream ?? throw new ArgumentNullException(nameof(fileStream));
            this.Validator = validator ?? throw new ArgumentNullException(nameof(validator));
            this.InputValidator = inputValidator ?? throw new ArgumentNullException(nameof(inputValidator));

            this.ReadFile();
        }

        /// <inheritdoc/>
        public IRecordValidator Validator { get; }

        /// <inheritdoc/>
        public IInputValidator InputValidator { get; }

        /// <inheritdoc/>
        public int CreateRecord(FileCabinetData data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.Validator.ValidateParameters(data);

            var record = new FileCabinetRecord(this.count++, data);
            this.AddRecordToDictionaries(record, record.Id);

            byte[] bytes = ToBytes(record);

            this.fileStream.Position = RecordSize * record.Id;
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

            var record = this.ReadRecord(index);

            if (record == null)
            {
                throw new ArgumentException("Invalid record", nameof(id));
            }

            this.RemoveRecordFromDictionaries(record, index);

            // TODO: Add dictionary interaction to this and below methods.
            byte[] newByteData = ToBytes(data, id);

            this.fileStream.Position = index * RecordSize;
            this.fileStream.Seek(-RecordSize, SeekOrigin.Current);
            this.fileStream.Write(newByteData);

            this.AddRecordToDictionaries(new FileCabinetRecord(id, data), index);
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            return this.FindBy(this.dateOfBirthDictionary, dateOfBirth);
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            return this.FindBy(this.firstNameDictionary, firstName.ToUpperInvariant());
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            return this.FindBy(this.lastNameDictionary, lastName.ToUpperInvariant());
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var bytes = new byte[RecordSize];
            var result = new List<FileCabinetRecord>();

            this.fileStream.Position = 0;

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
            return new FileCabinetServiceSnapshot(this.GetRecords().ToArray(), this.InputValidator);
        }

        /// <inheritdoc/>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            FileCabinetRecord[] records = snapshot.Records.ToArray();

            this.count = records.Length;
            this.deleted = 0;
            this.ClearDictionaries();

            this.fileStream.Position = 0;

            for (int i = 0; i < records.Length; ++i)
            {
                byte[] bytes = ToBytes(records[i]);
                this.fileStream.Write(bytes);

                this.AddRecordToDictionaries(records[i], i);
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

            var record = this.ReadRecord(index);

            if (record != null)
            {
                this.RemoveRecordFromDictionaries(record, index);
            }
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

            FileCabinetServiceSnapshot snapshot = new FileCabinetServiceSnapshot(this.GetRecords().ToArray(), this.InputValidator);
            this.Restore(snapshot);
        }

        /// <inheritdoc/>
        public override string ToString() => "file";

        private static FileCabinetRecord ToRecord(Span<byte> spanBytes)
        {
            int id = BitConverter.ToInt32(spanBytes[..4]);

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

        private static void AddToDictionary<T>(Dictionary<T, List<int>> dictionary, T value, int offset)
            where T : notnull
        {
            if (!dictionary.ContainsKey(value))
            {
                dictionary[value] = new List<int>();
            }

            dictionary[value].Add(offset);
        }

        private static void RemoveFromDictionary<T>(Dictionary<T, List<int>> dictionary, T value, int offset)
            where T : notnull
        {
            if (!dictionary.ContainsKey(value))
            {
                return;
            }

            dictionary[value].Remove(offset);
        }

        private ReadOnlyCollection<FileCabinetRecord> FindBy<T>(Dictionary<T, List<int>> dictionary, T value)
            where T : notnull
        {
            if (!dictionary.ContainsKey(value))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(Array.Empty<FileCabinetRecord>());
            }

            var records = new List<FileCabinetRecord>();

            for (int i = 0; i < dictionary[value].Count; ++i)
            {
                var record = this.ReadRecord(dictionary[value][i]);
                if (record != null)
                {
                    records.Add(record);
                }
            }

            return records.AsReadOnly();
        }

        private void ReadFile()
        {
            var records = this.GetRecords();

            for (int i = 0; i < records.Count; ++i)
            {
                this.AddRecordToDictionaries(records[i], i);
            }

            this.count = records.Count;
        }

        private void AddRecordToDictionaries(FileCabinetRecord record, int offset)
        {
            AddToDictionary(this.firstNameDictionary, record.FirstName.ToUpperInvariant(), offset);
            AddToDictionary(this.lastNameDictionary, record.LastName.ToUpperInvariant(), offset);
            AddToDictionary(this.dateOfBirthDictionary, record.DateOfBirth, offset);
        }

        private void RemoveRecordFromDictionaries(FileCabinetRecord record, int offset)
        {
            RemoveFromDictionary(this.firstNameDictionary, record.FirstName, offset);
            RemoveFromDictionary(this.lastNameDictionary, record.LastName, offset);
            RemoveFromDictionary(this.dateOfBirthDictionary, record.DateOfBirth, offset);
        }

        private void ClearDictionaries()
        {
            this.dateOfBirthDictionary.Clear();
            this.firstNameDictionary.Clear();
            this.lastNameDictionary.Clear();
        }

        private int FindById(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "id cannot be less than 0");
            }

            const int bytesAmount = 5;
            byte[] bytes = new byte[bytesAmount];

            for (int i = 0; i < this.count; ++i)
            {
                this.fileStream.Position = RecordSize * i;

                if (this.fileStream.Read(bytes, 0, bytesAmount) > 0)
                {
                    if (bytes[0] == 1)
                    {
                        continue;
                    }

                    var recordId = BitConverter.ToInt32(bytes.AsSpan()[1..]);

                    if (id == recordId)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        private FileCabinetRecord? ReadRecord(int offset)
        {
            var bytes = new byte[RecordSize];

            this.fileStream.Position = offset * RecordSize;

            if (this.fileStream.Read(bytes, 0, RecordSize) > 0 && bytes[0] == 0)
            {
                return ToRecord(bytes.AsSpan()[1..]);
            }

            return null;
        }
    }
}

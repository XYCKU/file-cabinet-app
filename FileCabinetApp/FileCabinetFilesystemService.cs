using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <inheritdoc/>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private const int RecordSize = 4 + 120 + 120 + 4 + 4 + 4 + 2 + 8 + 1;
        private static readonly Encoding Encoding = Encoding.UTF8;
        private static readonly int[] DataSizes = { 4, 120, 120, 4, 4, 4, 2, 8, 1 };
        private readonly FileStream fileStream;
        private int count;

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

            var record = new FileCabinetRecord
            {
                Id = this.count++,
                FirstName = data.FirstName,
                LastName = data.LastName,
                DateOfBirth = data.DateOfBirth,
                CarAmount = data.CarAmount,
                Money = data.Money,
                FavoriteChar = data.FavoriteChar,
            };

            byte[] bytes = new byte[RecordSize];

            var dataInfo = new byte[][]
            {
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

            this.fileStream.Write(bytes);

            this.fileStream.Flush();

            return record.Id;
        }

        /// <inheritdoc/>
        public void EditRecord(int id, FileCabinetData data)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            byte[] bytes = new byte[RecordSize];
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();

            this.fileStream.Seek(0, SeekOrigin.Begin);

            while (this.fileStream.Read(bytes, 0, RecordSize) > 0)
            {
                var spanBytes = bytes.AsSpan();

                int id = BitConverter.ToInt32(spanBytes[0..4]);
                string firstName = Encoding.GetString(spanBytes[4..124]);
                string lastName = Encoding.GetString(spanBytes[124..244]);
                int year = BitConverter.ToInt32(spanBytes[244..248]);
                int month = BitConverter.ToInt32(spanBytes[248..252]);
                int day = BitConverter.ToInt32(spanBytes[252..256]);
                DateTime dob = new DateTime(year, month, day);
                short carAmount = BitConverter.ToInt16(spanBytes[256..258]);
                decimal money = (decimal)BitConverter.ToDouble(spanBytes[258..266]);
                char favoriteChar = BitConverter.ToChar(new byte[] { spanBytes[266], 0 });

                var record = new FileCabinetRecord()
                {
                    Id = id,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dob,
                    CarAmount = carAmount,
                    Money = money,
                    FavoriteChar = favoriteChar,
                };

                result.Add(record);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            return (int)(this.fileStream.Length / RecordSize);
        }

        /// <inheritdoc/>
        public override string ToString() => "file";
    }
}

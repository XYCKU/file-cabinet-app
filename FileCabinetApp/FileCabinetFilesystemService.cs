using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <inheritdoc/>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
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

            Encoding encoding = Encoding.UTF8;
            const int recordSize = 4 + 120 + 120 + 4 + 4 + 4 + 2 + 8 + 1;
            byte[] bytes = new byte[recordSize];

            var dataInfo = new[]
            {
                new { Size = 4, Bytes = BitConverter.GetBytes(record.Id) },
                new { Size = 120, Bytes = encoding.GetBytes(record.FirstName) },
                new { Size = 120, Bytes = encoding.GetBytes(record.LastName) },
                new { Size = 4, Bytes = BitConverter.GetBytes(record.DateOfBirth.Year) },
                new { Size = 4, Bytes = BitConverter.GetBytes(record.DateOfBirth.Month) },
                new { Size = 4, Bytes = BitConverter.GetBytes(record.DateOfBirth.Day) },
                new { Size = 2, Bytes = BitConverter.GetBytes(record.CarAmount) },
                new { Size = 8, Bytes = BitConverter.GetBytes((double)record.Money) },
                new { Size = 1, Bytes = BitConverter.GetBytes(record.FavoriteChar) },
            };

            int offset = 0;
            for (int i = 0; i < dataInfo.Length; ++i)
            {
                Array.Copy(dataInfo[i].Bytes, 0, bytes, offset, Math.Min(dataInfo[i].Size, dataInfo[i].Bytes.Length));
                offset += dataInfo[i].Size;
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
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override string ToString() => "file";
    }
}

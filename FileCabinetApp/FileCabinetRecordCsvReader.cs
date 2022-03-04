using FileCabinetApp.Validators.Input;

namespace FileCabinetApp
{
    /// <inheritdoc/>
    public class FileCabinetRecordCsvReader : IFileCabinetRecordReader
    {
        private static readonly IInputValidator InputValidator = new DefaultInputValidator();
        private readonly StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="reader"><see cref="StreamReader"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="reader"/> is <c>null</c>.</exception>
        public FileCabinetRecordCsvReader(StreamReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            this.reader = reader;
        }

        /// <inheritdoc/>
        public IList<FileCabinetRecord> ReadAll()
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            int recordNumber = 0;

            string line = this.reader.ReadLine() ?? string.Empty;

            while (!this.reader.EndOfStream)
            {
                ++recordNumber;
                line = this.reader.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] data = line.Split(',', 7, StringSplitOptions.RemoveEmptyEntries);

                if (data is null)
                {
                    continue;
                }

                const int FieldAmount = 7;

                if (data.Length != FieldAmount)
                {
                    continue;
                }

                int id;
                string firstName, lastName;
                DateTime dob;
                short carAmount;
                decimal money;
                char favoriteChar;

                data[5] = data[5].Replace('.', ',');

                Tuple<bool, string>[] validationResults =
                {
                    CheckInput(data[0], InputConverter.IntConverter, (int id) => new Tuple<bool, string>(id >= 0, $"Validation failed: {id} cannot be less than 0"), out id),
                    CheckInput(data[1], InputConverter.StringConverter, InputValidator.FirstNameValidator, out firstName),
                    CheckInput(data[2], InputConverter.StringConverter, InputValidator.LastNameValidator, out lastName),
                    CheckInput(data[3], InputConverter.DateConverter, InputValidator.DateOfBirthValidator, out dob),
                    CheckInput(data[4], InputConverter.ShortConverter, InputValidator.CarAmountValidator, out carAmount),
                    CheckInput(data[5], InputConverter.DecimalConverter, InputValidator.MoneyValidator, out money),
                    CheckInput(data[6], InputConverter.CharConverter, InputValidator.FavoriteCharValidator, out favoriteChar),
                };

                bool isOk = true;

                for (int i = 0; i < validationResults.Length; ++i)
                {
                    if (!validationResults[i].Item1)
                    {
                        isOk = false;
                        Console.WriteLine($"#{recordNumber}: {validationResults[i].Item2}");
                        break;
                    }
                }

                if (!isOk)
                {
                    continue;
                }

                result.Add(new FileCabinetRecord(id, firstName, lastName, dob, carAmount, money, favoriteChar));
            }

            return result;
        }

        private static Tuple<bool, string> CheckInput<T>(string input, Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator, out T value)
        {
            var conversionResult = converter(input);
            value = conversionResult.Item3;

            if (!conversionResult.Item1)
            {
                return new Tuple<bool, string>(false, $"Conversion failed: {conversionResult.Item2}.");
            }

            var validationResult = validator(value);
            if (!validationResult.Item1)
            {
                return new Tuple<bool, string>(false, $"Validation failed: {validationResult.Item2}.");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }
    }
}

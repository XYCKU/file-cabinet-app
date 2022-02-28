using System;
using System.Globalization;

namespace FileCabinetApp
{
    /// <inheritdoc/>
    public class FileCabinetRecordCsvReader : IFileCabinetRecordReader
    {
        private const string DateTimeFormat = "MM/dd/yyyy";
        private static readonly IRecordValidator Validator = new DefaultValidator();
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

                string[] data = line.Split(',', StringSplitOptions.RemoveEmptyEntries);

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

                Tuple<bool, string>[] validationResults =
                {
                    CheckInput(data[0], IdConverter, (int id) => new Tuple<bool, string>(id >= 0, $"Validation failed: {id} cannot be less than 0"), out id),
                    CheckInput(data[1], StringConverter, FirstNameValidator, out firstName),
                    CheckInput(data[2], StringConverter, FirstNameValidator, out lastName),
                    CheckInput(data[3], DateConverter, DateOfBirthValidator, out dob),
                    CheckInput(data[4], ShortConverter, CarAmountValidator, out carAmount),
                    CheckInput(data[5].Replace('.', ','), DecimalConverter, MoneyValidator, out money),
                    CheckInput(data[6], CharConverter, FavoriteCharValidator, out favoriteChar),
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

                result.Add(new FileCabinetRecord()
                {
                    Id = id,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dob,
                    CarAmount = carAmount,
                    Money = money,
                    FavoriteChar = favoriteChar,
                });
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

        private static Tuple<bool, string, string> StringConverter(string input)
        {
            return new Tuple<bool, string, string>(true, string.Empty, input);
        }

        private static Tuple<bool, string, int> IdConverter(string input)
        {
            return new Tuple<bool, string, int>(int.TryParse(input, out int result) && result >= 0, "Invalid id.", result);
        }

        private static Tuple<bool, string, DateTime> DateConverter(string input)
        {
            return new Tuple<bool, string, DateTime>(
                DateTime.TryParseExact(input, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result),
                "Invalid date of birth.",
                result);
        }

        private static Tuple<bool, string, char> CharConverter(string input)
        {
            return new Tuple<bool, string, char>(char.TryParse(input, out char result), "Invalid char.", char.ToUpperInvariant(result));
        }

        private static Tuple<bool, string, short> ShortConverter(string input)
        {
            return new Tuple<bool, string, short>(short.TryParse(input, out short result), input, result);
        }

        private static Tuple<bool, string, decimal> DecimalConverter(string input)
        {
            return new Tuple<bool, string, decimal>(decimal.TryParse(input, out decimal result), input, result);
        }

        private static Tuple<bool, string> FirstNameValidator(string firstName)
        {
            try
            {
                Validator.ValidateFirstName(firstName);
            }
            catch (Exception e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        private static Tuple<bool, string> LastNameValidator(string lastName)
        {
            try
            {
                Validator.ValidateLastName(lastName);
            }
            catch (Exception e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        private static Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
        {
            try
            {
                Validator.ValidateDateOfBirth(dateOfBirth);
            }
            catch (Exception e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        private static Tuple<bool, string> CarAmountValidator(short carAmount)
        {
            try
            {
                Validator.ValidateCarAmount(carAmount);
            }
            catch (Exception e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        private static Tuple<bool, string> MoneyValidator(decimal money)
        {
            try
            {
                Validator.ValidateMoney(money);
            }
            catch (Exception e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        private static Tuple<bool, string> FavoriteCharValidator(char favoriteChar)
        {
            try
            {
                Validator.ValidateFavoriteChar(favoriteChar);
            }
            catch (Exception e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }
    }
}

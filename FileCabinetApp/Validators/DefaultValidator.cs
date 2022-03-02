using System;
using System.Globalization;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Default validator for <see cref="FileCabinetData"/>.
    /// </summary>
    public class DefaultValidator : IRecordValidator
    {
        private const int MinNameLength = 2;
        private const int MaxNameLength = 60;
        private const char MinChar = 'A';
        private const char MaxChar = 'Z';
        private const short MinCarAmount = 0;
        private const short MaxCarAmount = 60;
        private const decimal MinMoney = 0;
        private const decimal MaxMoney = 9999999999;
        private static readonly DateTime MinDate = new DateTime(1950, 01, 01);
        private static readonly DateTime MaxDate = DateTime.Now;

        /// <summary>
        /// Validates given data parameters.
        /// </summary>
        /// <param name="data"><see cref="FileCabinetData"/> parameters.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="data.FirstName"/> or <paramref name="data.LastName"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.FirstName.Length"/> or <paramref name="data.LastName.Length"/> is less than 2 or greater than 60.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.DateOfBirth"/> is earlier than 01-01-1950 or later than <see cref="DateTime.Now"/>.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.CarAmount"/> or <paramref name="data.Money"/> is less than zero.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.FavoriteChar"/> is not a letter of english alphabet.</exception>
        public void ValidateParameters(FileCabinetData data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            new FirstNameValidator(MinNameLength, MaxNameLength).ValidateParameters(data);
            new LastNameValidator(MinNameLength, MaxNameLength).ValidateParameters(data);
            new DateOfBirthValidator(MinDate, MaxDate).ValidateParameters(data);
            new CarAmountValidator(MinCarAmount, MaxCarAmount).ValidateParameters(data);
            new MoneyValidator(MinMoney, MaxMoney).ValidateParameters(data);
            new FavoriteCharValidator(MinChar, MaxChar).ValidateParameters(data);
        }

        /// <inheritdoc/>
        public override string ToString() => $"default";
    }
}

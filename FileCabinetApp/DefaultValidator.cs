using System;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Default validator for <see cref="FileCabinetData"/>.
    /// </summary>
    public class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// Minimum length for name field.
        /// </summary>
        protected const int MinNameLength = 2;

        /// <summary>
        /// Maximum length for name field.
        /// </summary>
        protected const int MaxNameLength = 60;

        /// <summary>
        /// The earliest date that can DateOfBirth field has.
        /// </summary>
        protected static readonly DateTime EarliestDate = new DateTime(1950, 01, 01);

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

            this.ValidateFirstName(data.FirstName);
            this.ValidateLastName(data.LastName);
            this.ValidateDateOfBirth(data.DateOfBirth);
            this.ValidateCarAmount(data.CarAmount);
            this.ValidateMoney(data.Money);
            this.ValidateFavoriteChar(data.FavoriteChar);
        }

        /// <inheritdoc/>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="firstName"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="firstName.Length"/> is less than 2 or greater than 60.</exception>
        public void ValidateFirstName(string firstName)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            if (firstName.Length < MinNameLength || firstName.Length > MaxNameLength)
            {
                throw new ArgumentException($"firstName.Length is less than {MinNameLength} or greater than {MaxNameLength}", nameof(firstName));
            }
        }

        /// <inheritdoc/>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="lastName"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="lastName.Length"/> is less than 2 or greater than 60.</exception>
        public void ValidateLastName(string lastName)
        {
            if (lastName is null)
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            if (lastName.Length < MinNameLength || lastName.Length > MaxNameLength)
            {
                throw new ArgumentException($"firstName.Length is less than {MinNameLength} or greater than {MaxNameLength}", nameof(lastName));
            }
        }

        /// <inheritdoc/>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="dateOfBirth"/> is earlier than 01-01-1950 or later than <see cref="DateTime.Now"/>.</exception>
        public void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth < EarliestDate || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"dateOfBirth is earlier than {EarliestDate.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)} or later than DateTime.Now", nameof(dateOfBirth));
            }
        }

        /// <inheritdoc/>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="carAmount"/> is less than zero.</exception>
        public void ValidateCarAmount(decimal carAmount)
        {
            if (carAmount < 0)
            {
                throw new ArgumentException("carAmount is less than zero", nameof(carAmount));
            }
        }

        /// <inheritdoc/>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="money"/> is less than zero.</exception>
        public void ValidateMoney(decimal money)
        {
            if (money < 0)
            {
                throw new ArgumentException("money is less than zero", nameof(money));
            }
        }

        /// <inheritdoc/>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="favoriteChar"/> is not a letter of english alphabet.</exception>
        public void ValidateFavoriteChar(char favoriteChar)
        {
            if (!char.IsLetter(favoriteChar))
            {
                throw new ArgumentException($"favoriteChar {favoriteChar} is not a letter", nameof(favoriteChar));
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"default";
    }
}

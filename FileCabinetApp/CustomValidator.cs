using System;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Default validator for <see cref="FileCabinetData"/>.
    /// </summary>
    public class CustomValidator : IRecordValidator
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
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.FirstName"/>.Length or <paramref name="data.LastName"/>.Length is less than 2 or greater than 60.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.DateOfBirth"/> is earlier than 01-01-1950 or later than <see cref="DateTime"/>.Now.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.CarAmount"/> or <paramref name="data.Money"/> is less than zero.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.FavoriteChar"/> is not a digit.</exception>
        public void ValidateParameters(FileCabinetData data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.FirstName is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.LastName is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.FirstName.Length < MinNameLength || data.FirstName.Length > MaxNameLength)
            {
                throw new ArgumentException($"firstName.Length is less than {MinNameLength} or greater than {MaxNameLength}", nameof(data));
            }

            if (data.LastName.Length < MinNameLength || data.LastName.Length > MaxNameLength)
            {
                throw new ArgumentException($"lastName.Length is less than {MinNameLength} or greater than {MaxNameLength}", nameof(data));
            }

            if (data.DateOfBirth < EarliestDate || data.DateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"dateOfBirth is earlier than {EarliestDate.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)} or later than DateTime.Now", nameof(data));
            }

            if (data.CarAmount < 0)
            {
                throw new ArgumentException("carAmount is less than zero", nameof(data));
            }

            if (data.Money < 0)
            {
                throw new ArgumentException("money is less than zero", nameof(data));
            }

            if (!char.IsDigit(data.FavoriteChar))
            {
                throw new ArgumentException($"favoriteChar {data.FavoriteChar} is not a digit", nameof(data));
            }
        }
    }
}

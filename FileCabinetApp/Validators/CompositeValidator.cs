namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validator for <see cref="FileCabinetData"/>.
    /// </summary>
    public class CompositeValidator : IRecordValidator
    {
        private readonly IEnumerable<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator"/> class.
        /// </summary>
        /// <param name="validators">Collection of validators.</param>
        public CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            if (validators is null)
            {
                throw new ArgumentNullException(nameof(validators));
            }

            this.validators = validators;
        }

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

            foreach (var validator in this.validators)
            {
                validator.ValidateParameters(data);
            }
        }
    }
}
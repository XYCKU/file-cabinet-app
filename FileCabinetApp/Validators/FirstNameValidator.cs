namespace FileCabinetApp.Validators
{
    /// <inheritdoc/>
    public class FirstNameValidator : IRecordValidator
    {
        private readonly int minNameLength;
        private readonly int maxNameLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstNameValidator"/> class.
        /// </summary>
        /// <param name="from">Min length.</param>
        /// <param name="to">Max length.</param>
        public FirstNameValidator(int from, int to)
        {
            if (from < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(from), "from is less than zero.");
            }

            if (to < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(to), "to is less than zero.");
            }

            if (from > to)
            {
                throw new ArgumentException($"from ({from}) is greater than to({to})", nameof(from));
            }

            this.minNameLength = from;
            this.maxNameLength = to;
        }

        /// <inheritdoc/>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="data.FirstName"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.FirstName.Length"/> is less than MinNameLength or greater than MaxNameLength.</exception>
        public void ValidateParameters(FileCabinetData data)
        {
            if (data.FirstName is null)
            {
                throw new ArgumentNullException(nameof(data), "data.FirstName is null.");
            }

            if (data.FirstName.Length < this.minNameLength || data.FirstName.Length > this.maxNameLength)
            {
                throw new ArgumentException($"firstName.Length is less than {this.minNameLength} or greater than {this.maxNameLength}", nameof(data));
            }
        }
    }
}
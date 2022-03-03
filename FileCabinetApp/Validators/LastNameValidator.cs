namespace FileCabinetApp.Validators
{
    /// <inheritdoc/>
    public class LastNameValidator : IRecordValidator
    {
        private readonly int minNameLength;
        private readonly int maxNameLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameValidator"/> class.
        /// </summary>
        /// <param name="from">Min length.</param>
        /// <param name="to">Max length.</param>
        public LastNameValidator(int from, int to)
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
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="data.LastName"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.LastName.Length"/> is less than MinNameLength or greater than MaxNameLength.</exception>
        public void ValidateParameters(FileCabinetData data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.LastName.Length < this.minNameLength || data.LastName.Length > this.maxNameLength)
            {
                throw new ArgumentException($"lastName.Length is less than {this.minNameLength} or greater than {this.maxNameLength}", nameof(data));
            }
        }
    }
}
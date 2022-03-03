using System.Globalization;

namespace FileCabinetApp.Validators
{
    /// <inheritdoc/>
    public class DateOfBirthValidator : IRecordValidator
    {
        private readonly DateTime minDate;
        private readonly DateTime maxDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateOfBirthValidator"/> class.
        /// </summary>
        /// <param name="from">Min date.</param>
        /// <param name="to">Max date.</param>
        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            if (from > to)
            {
                throw new ArgumentException($"from ({from.ToShortDateString()}) is greater than to({to.ToShortDateString()})", nameof(from));
            }

            this.minDate = from;
            this.maxDate = to;
        }

        /// <inheritdoc/>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.DateOfBirth"/> is earlier than EarliestDate or later than LatestDate.</exception>
        public void ValidateParameters(FileCabinetData data)
        {
            if (data.DateOfBirth < this.minDate || data.DateOfBirth > this.maxDate)
            {
                throw new ArgumentException($"dateOfBirth is earlier than {this.minDate.ToShortDateString()} or later than {this.maxDate.ToShortDateString()}", nameof(data));
            }
        }
    }
}
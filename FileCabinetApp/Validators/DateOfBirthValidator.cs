using System.Globalization;

namespace FileCabinetApp.Validators
{
    /// <inheritdoc/>
    public class DateOfBirthValidator : IRecordValidator
    {
        private readonly DateTime earliestDate;
        private readonly DateTime latestDate;

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

            this.earliestDate = from;
            this.latestDate = to;
        }

        /// <inheritdoc/>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.DateOfBirth"/> is earlier than EarliestDate or later than LatestDate.</exception>
        public void ValidateParameters(FileCabinetData data)
        {
            if (data.DateOfBirth < this.earliestDate || data.DateOfBirth > this.latestDate)
            {
                throw new ArgumentException($"dateOfBirth is earlier than {this.earliestDate.ToShortDateString()} or later than {this.latestDate.ToShortDateString()}", nameof(data));
            }
        }
    }
}
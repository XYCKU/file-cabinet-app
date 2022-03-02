namespace FileCabinetApp.Validators
{
    /// <inheritdoc/>
    public class CarAmountValidator : IRecordValidator
    {
        private readonly short minCarAmount;
        private readonly short maxCarAmount;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarAmountValidator"/> class.
        /// </summary>
        /// <param name="from">Min car amount.</param>
        /// <param name="to">Max car amount.</param>
        public CarAmountValidator(short from, short to)
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

            this.minCarAmount = from;
            this.maxCarAmount = to;
        }

        /// <inheritdoc/>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.CarAmount"/> is less than zero.</exception>
        public void ValidateParameters(FileCabinetData data)
        {
            if (data.CarAmount < this.minCarAmount || data.CarAmount > this.maxCarAmount)
            {
                throw new ArgumentException($"carAmount is less than {this.minCarAmount} or greater than {this.maxCarAmount}", nameof(data));
            }
        }
    }
}
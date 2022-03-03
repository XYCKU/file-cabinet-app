namespace FileCabinetApp.Validators
{
    /// <inheritdoc/>
    public class MoneyValidator : IRecordValidator
    {
        private readonly decimal minMoney;
        private readonly decimal maxMoney;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoneyValidator"/> class.
        /// </summary>
        /// <param name="from">Min car amount.</param>
        /// <param name="to">Max car amount.</param>
        public MoneyValidator(decimal from, decimal to)
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

            this.minMoney = from;
            this.maxMoney = to;
        }

        /// <inheritdoc/>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.CarAmount"/> is less than zero.</exception>
        public void ValidateParameters(FileCabinetData data)
        {
            if (data.CarAmount < this.minMoney || data.CarAmount > this.maxMoney)
            {
                throw new ArgumentException($"money is less than {this.minMoney} or greater than {this.maxMoney}", nameof(data));
            }
        }
    }
}
namespace FileCabinetApp.Validators
{
    /// <inheritdoc/>
    public class InputValidatorBase : IInputValidator
    {
        private readonly int minNameLength;
        private readonly int maxNameLength;
        private readonly char minChar;
        private readonly char maxChar;
        private readonly short minCarAmount;
        private readonly short maxCarAmount;
        private readonly decimal minMoney;
        private readonly decimal maxMoney;
        private readonly DateTime minDate;
        private readonly DateTime maxDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputValidatorBase"/> class.
        /// </summary>
        /// <param name="minNameLength">.</param>
        /// <param name="maxNameLength">..</param>
        /// <param name="minChar">...</param>
        /// <param name="maxChar">....</param>
        /// <param name="minCarAmount">.....</param>
        /// <param name="maxCarAmount">......</param>
        /// <param name="minMoney">.,..</param>
        /// <param name="maxMoney">.,.</param>
        /// <param name="minDate">.,,.</param>
        /// <param name="maxDate">...,.</param>
        protected InputValidatorBase(int minNameLength, int maxNameLength, char minChar, char maxChar, short minCarAmount, short maxCarAmount, decimal minMoney, decimal maxMoney, DateTime minDate, DateTime maxDate)
        {
            this.minNameLength = minNameLength;
            this.maxNameLength = maxNameLength;
            this.minChar = minChar;
            this.maxChar = maxChar;
            this.minCarAmount = minCarAmount;
            this.maxCarAmount = maxCarAmount;
            this.minMoney = minMoney;
            this.maxMoney = maxMoney;
            this.minDate = minDate;
            this.maxDate = maxDate;
        }

        /// <inheritdoc/>
        public Tuple<bool, string> CarAmountValidator(short carAmount)
        {
            if (carAmount < this.minCarAmount || carAmount > this.maxCarAmount)
            {
                return new Tuple<bool, string>(false, $"carAmount is less than {this.minCarAmount} or greater than {this.maxCarAmount}");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <inheritdoc/>
        public Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
        {
            if (dateOfBirth < this.minDate || dateOfBirth > this.maxDate)
            {
                return new Tuple<bool, string>(false, $"dateOfBirth is earlier than {this.minDate.ToShortDateString()} or later than {this.maxDate.ToShortDateString()}");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <inheritdoc/>
        public Tuple<bool, string> FavoriteCharValidator(char favoriteChar)
        {
            if (favoriteChar < this.minChar || favoriteChar > this.maxChar)
            {
                return new Tuple<bool, string>(false, $"favoriteChar {favoriteChar} is not between {this.minChar} and {this.maxChar}");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <inheritdoc/>
        public Tuple<bool, string> FirstNameValidator(string firstName)
        {
            if (firstName is null)
            {
                return new Tuple<bool, string>(false, "firstName is null.");
            }

            if (firstName.Length < this.minNameLength || firstName.Length > this.maxNameLength)
            {
                return new Tuple<bool, string>(false, $"firstName.Length is less than {this.minNameLength} or greater than {this.maxNameLength}");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <inheritdoc/>
        public Tuple<bool, string> LastNameValidator(string lastName)
        {
            if (lastName is null)
            {
                return new Tuple<bool, string>(false, "lastName is null.");
            }

            if (lastName.Length < this.minNameLength || lastName.Length > this.maxNameLength)
            {
                return new Tuple<bool, string>(false, $"lastName.Length is less than {this.minNameLength} or greater than {this.maxNameLength}");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <inheritdoc/>
        public Tuple<bool, string> MoneyValidator(decimal money)
        {
            if (money < this.minMoney || money > this.maxMoney)
            {
                return new Tuple<bool, string>(false, $"money is less than {this.minMoney} or greater than {this.maxMoney}");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }
    }
}
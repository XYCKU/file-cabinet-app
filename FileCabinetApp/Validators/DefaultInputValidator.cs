using System;

namespace FileCabinetApp.Validators
{
    /// <inheritdoc/>
    public class DefaultInputValidator : IInputValidator
    {
        private readonly int minNameLength = 2;
        private readonly int maxNameLength = 60;
        private readonly char minChar = 'A';
        private readonly char maxChar = 'Z';
#pragma warning disable CA1805 // Do not initialize unnecessarily
        private readonly short minCarAmount = 0;
#pragma warning restore CA1805 // Do not initialize unnecessarily
        private readonly short maxCarAmount = 60;
        private readonly decimal minMoney = 0;
        private readonly decimal maxMoney = 9999999999;
        private readonly DateTime minDate = new DateTime(1950, 01, 01);
        private readonly DateTime maxDate = DateTime.Now;

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
        public Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
        {
            if (dateOfBirth < this.minDate || dateOfBirth > this.maxDate)
            {
                return new Tuple<bool, string>(false, $"dateOfBirth is earlier than {this.minDate.ToShortDateString()} or later than {this.maxDate.ToShortDateString()}");
            }

            return new Tuple<bool, string>(true, string.Empty);
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
        public Tuple<bool, string> MoneyValidator(decimal money)
        {
            if (money < this.minMoney || money > this.maxMoney)
            {
                return new Tuple<bool, string>(false, $"money is less than {this.minMoney} or greater than {this.maxMoney}");
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
    }
}

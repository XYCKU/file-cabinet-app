using FileCabinetApp.Validators.Config;

namespace FileCabinetApp.Validators.Input
{
    /// <inheritdoc/>
    public class InputValidatorBase : IInputValidator
    {
        private readonly ConfigurationData config;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputValidatorBase"/> class.
        /// </summary>
        /// <param name="config">Configuration info.</param>
        protected InputValidatorBase(ConfigurationData config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            this.config = config;
        }

        /// <inheritdoc/>
        public Tuple<bool, string> CarAmountValidator(short carAmount)
        {
            if (carAmount < this.config.MinCarAmount || carAmount > this.config.MaxCarAmount)
            {
                return new Tuple<bool, string>(false, $"carAmount is less than {this.config.MinCarAmount} or greater than {this.config.MaxCarAmount}");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <inheritdoc/>
        public Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
        {
            if (dateOfBirth < this.config.MinDate || dateOfBirth > this.config.MaxDate)
            {
                return new Tuple<bool, string>(false, $"dateOfBirth is earlier than {this.config.MinDate.ToString("MM/dd/yyyy", Program.DefaultCulture)} or later than {this.config.MaxDate.ToString("MM/dd/yyyy", Program.DefaultCulture)}");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <inheritdoc/>
        public Tuple<bool, string> FavoriteCharValidator(char favoriteChar)
        {
            if (favoriteChar < this.config.MinChar || favoriteChar > this.config.MaxChar)
            {
                return new Tuple<bool, string>(false, $"favoriteChar {favoriteChar} is not between {this.config.MinChar} and {this.config.MaxChar}");
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

            if (firstName.Length < this.config.MinFirstNameLength || firstName.Length > this.config.MaxFirstNameLength)
            {
                return new Tuple<bool, string>(false, $"firstName.Length is less than {this.config.MinFirstNameLength} or greater than {this.config.MaxFirstNameLength}");
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

            if (lastName.Length < this.config.MinLastNameLength || lastName.Length > this.config.MaxLastNameLength)
            {
                return new Tuple<bool, string>(false, $"lastName.Length is less than {this.config.MinLastNameLength} or greater than {this.config.MaxLastNameLength}");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <inheritdoc/>
        public Tuple<bool, string> MoneyValidator(decimal money)
        {
            if (money < this.config.MinMoney || money > this.config.MaxMoney)
            {
                return new Tuple<bool, string>(false, $"money is less than {this.config.MinMoney} or greater than {this.config.MaxMoney}");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }
    }
}
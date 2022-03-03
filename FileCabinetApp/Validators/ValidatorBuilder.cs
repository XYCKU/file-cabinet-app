using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validator builder.
    /// </summary>
    public class ValidatorBuilder
    {
        private readonly List<IRecordValidator> validators = new List<IRecordValidator>();

        /// <summary>
        /// Creates <see cref="FirstNameValidator"/>.
        /// </summary>
        /// <param name="min">Min length.</param>
        /// <param name="max">Max length.</param>
        /// <returns>This <see cref="ValidatorBuilder"/>.</returns>
        public ValidatorBuilder ValidateFirstName(int min, int max)
        {
            this.validators.Add(new FirstNameValidator(min, max));
            return this;
        }

        /// <summary>
        /// Creates <see cref="LastNameValidator"/>.
        /// </summary>
        /// <param name="min">Min length.</param>
        /// <param name="max">Max length.</param>
        /// <returns>This <see cref="ValidatorBuilder"/>.</returns>
        public ValidatorBuilder ValidateLastName(int min, int max)
        {
            this.validators.Add(new LastNameValidator(min, max));
            return this;
        }

        /// <summary>
        /// Creates <see cref="DateOfBirthValidator"/>.
        /// </summary>
        /// <param name="min">Min date.</param>
        /// <param name="max">Max date.</param>
        /// <returns>This <see cref="ValidatorBuilder"/>.</returns>
        public ValidatorBuilder ValidateDateOfBirth(DateTime min, DateTime max)
        {
            this.validators.Add(new DateOfBirthValidator(min, max));
            return this;
        }

        /// <summary>
        /// Creates <see cref="CarAmountValidator"/>.
        /// </summary>
        /// <param name="min">Min car amount.</param>
        /// <param name="max">Max car amount.</param>
        /// <returns>This <see cref="ValidatorBuilder"/>.</returns>
        public ValidatorBuilder ValidateCarAmount(short min, short max)
        {
            this.validators.Add(new CarAmountValidator(min, max));
            return this;
        }

        /// <summary>
        /// Creates <see cref="MoneyValidator"/>.
        /// </summary>
        /// <param name="min">Min money.</param>
        /// <param name="max">Max money.</param>
        /// <returns>This <see cref="ValidatorBuilder"/>.</returns>
        public ValidatorBuilder ValidateMoney(decimal min, decimal max)
        {
            this.validators.Add(new MoneyValidator(min, max));
            return this;
        }

        /// <summary>
        /// Creates <see cref="FavoriteCharValidator"/>.
        /// </summary>
        /// <param name="min">Min char.</param>
        /// <param name="max">Max char.</param>
        /// <returns>This <see cref="ValidatorBuilder"/>.</returns>
        public ValidatorBuilder ValidateFavoriteChar(char min, char max)
        {
            this.validators.Add(new FavoriteCharValidator(min, max));
            return this;
        }

        /// <summary>
        /// Creates composite validator based on validators.
        /// </summary>
        /// <returns>Composite validator.</returns>
        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }
    }
}

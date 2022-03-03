using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Extension for <see cref="ValidatorBuilder"/>.
    /// </summary>
    public static class ValidatorBuilderExtension
    {
        /// <summary>
        /// Creates default validator.
        /// </summary>
        /// <param name="builder">Builder.</param>
        /// <returns>Default validator.</returns>
        public static IRecordValidator CreateDefault(this ValidatorBuilder builder)
        {
            const int MinNameLength = 2;
            const int MaxNameLength = 60;
            const char MinChar = 'A';
            const char MaxChar = 'Z';
            const short MinCarAmount = 0;
            const short MaxCarAmount = 60;
            const decimal MinMoney = 0;
            const decimal MaxMoney = 9999999999;
            DateTime minDate = new DateTime(1950, 01, 01);
            DateTime maxDate = DateTime.Now;

            var validator = new ValidatorBuilder().ValidateFirstName(MinNameLength, MaxNameLength)
                    .ValidateLastName(MinNameLength, MaxNameLength)
                    .ValidateDateOfBirth(minDate, maxDate)
                    .ValidateCarAmount(MinCarAmount, MaxCarAmount)
                    .ValidateMoney(MinMoney, MaxMoney)
                    .ValidateFavoriteChar(MinChar, MaxChar)
                    .Create();

            return validator;
        }

        /// <summary>
        /// Creates custom validator.
        /// </summary>
        /// <param name="builder">Builder.</param>
        /// <returns>Custom validator.</returns>
        public static IRecordValidator CreateCustom(this ValidatorBuilder builder)
        {
            const int MinNameLength = 2;
            const int MaxNameLength = 60;
            const char MinChar = '0';
            const char MaxChar = '9';
            const short MinCarAmount = 0;
            const short MaxCarAmount = 60;
            const decimal MinMoney = 0;
            const decimal MaxMoney = 9999999999;
            DateTime minDate = new DateTime(1920, 01, 01);
            DateTime maxDate = new DateTime(2000, 01, 01);

            var validator = new ValidatorBuilder().ValidateFirstName(MinNameLength, MaxNameLength)
                    .ValidateLastName(MinNameLength, MaxNameLength)
                    .ValidateDateOfBirth(minDate, maxDate)
                    .ValidateCarAmount(MinCarAmount, MaxCarAmount)
                    .ValidateMoney(MinMoney, MaxMoney)
                    .ValidateFavoriteChar(MinChar, MaxChar)
                    .Create();

            return validator;
        }
    }
}

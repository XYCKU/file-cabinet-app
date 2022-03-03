using System;
using System.Globalization;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Default validator for <see cref="FileCabinetData"/>.
    /// </summary>
    public class DefaultValidator : CompositeValidator
    {
        private const int MinNameLength = 2;
        private const int MaxNameLength = 60;
        private const char MinChar = 'A';
        private const char MaxChar = 'Z';
        private const short MinCarAmount = 0;
        private const short MaxCarAmount = 60;
        private const decimal MinMoney = 0;
        private const decimal MaxMoney = 9999999999;
        private static readonly DateTime MinDate = new DateTime(1950, 01, 01);
        private static readonly DateTime MaxDate = DateTime.Now;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValidator"/> class.
        /// </summary>
        public DefaultValidator()
            : base(new IRecordValidator[]
            {
                new FirstNameValidator(MinNameLength, MaxNameLength),
                new LastNameValidator(MinNameLength, MaxNameLength),
                new DateOfBirthValidator(MinDate, MaxDate),
                new CarAmountValidator(MinCarAmount, MaxCarAmount),
                new MoneyValidator(MinMoney, MaxMoney),
                new FavoriteCharValidator(MinChar, MaxChar),
            })
        {
        }

        /// <inheritdoc/>
        public override string ToString() => $"default";
    }
}

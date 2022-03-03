using System;
using System.Globalization;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Default validator for <see cref="FileCabinetData"/>.
    /// </summary>
    public class CustomValidator : CompositeValidator
    {
        private const int MinNameLength = 2;
        private const int MaxNameLength = 60;
        private const char MinChar = '0';
        private const char MaxChar = '9';
        private const short MinCarAmount = 0;
        private const short MaxCarAmount = 60;
        private const decimal MinMoney = 0;
        private const decimal MaxMoney = 9999999999;
        private static readonly DateTime MinDate = new DateTime(1920, 01, 01);
        private static readonly DateTime MaxDate = new DateTime(2000, 01, 01);

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomValidator"/> class.
        /// </summary>
        public CustomValidator()
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
        public override string ToString() => $"custom";
    }
}

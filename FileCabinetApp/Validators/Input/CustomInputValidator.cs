using System;

namespace FileCabinetApp.Validators
{
    /// <inheritdoc/>
    public class CustomInputValidator : InputValidatorBase
    {
        private static readonly int MinNameLength = 2;
        private static readonly int MaxNameLength = 60;
        private static readonly char MinChar = '0';
        private static readonly char MaxChar = '9';
#pragma warning disable CA1805 // Do not initialize unnecessarily
        private static readonly short MinCarAmount = 0;
#pragma warning restore CA1805 // Do not initialize unnecessarily
        private static readonly short MaxCarAmount = 60;
        private static readonly decimal MinMoney = 0;
        private static readonly decimal MaxMoney = 9999999999;
        private static readonly DateTime MinDate = new DateTime(1920, 01, 01);
        private static readonly DateTime MaxDate = new DateTime(2000, 01, 01);

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomInputValidator"/> class.
        /// </summary>
        public CustomInputValidator()
            : base(MinNameLength, MaxNameLength, MinChar, MaxChar, MinCarAmount, MaxCarAmount, MinMoney, MaxMoney, MinDate, MaxDate)
        {
        }
    }
}

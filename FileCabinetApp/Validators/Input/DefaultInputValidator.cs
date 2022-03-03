using System;

namespace FileCabinetApp.Validators
{
    /// <inheritdoc/>
    public class DefaultInputValidator : InputValidatorBase
    {
        private static readonly int MinNameLength = 2;
        private static readonly int MaxNameLength = 60;
        private static readonly char MinChar = 'A';
        private static readonly char MaxChar = 'Z';
#pragma warning disable CA1805 // Do not initialize unnecessarily
        private static readonly short MinCarAmount = 0;
#pragma warning restore CA1805 // Do not initialize unnecessarily
        private static readonly short MaxCarAmount = 60;
        private static readonly decimal MinMoney = 0;
        private static readonly decimal MaxMoney = 9999999999;
        private static readonly DateTime MinDate = new DateTime(1950, 01, 01);
        private static readonly DateTime MaxDate = DateTime.Now;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultInputValidator"/> class.
        /// </summary>
        public DefaultInputValidator()
            : base(MinNameLength, MaxNameLength, MinChar, MaxChar, MinCarAmount, MaxCarAmount, MinMoney, MaxMoney, MinDate, MaxDate)
        {
        }
    }
}

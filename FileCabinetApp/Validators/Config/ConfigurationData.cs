using System;

namespace FileCabinetApp.Validators.Config
{
    /// <summary>
    /// Stores config for validators.
    /// </summary>
    public class ConfigurationData
    {
#pragma warning disable SA1600 // Elements should be documented
        public int MinFirstNameLength { get; set; }

        public int MaxFirstNameLength { get; set; }

        public int MinLastNameLength { get; set; }

        public int MaxLastNameLength { get; set; }

        public DateTime MinDate { get; set; }

        public DateTime MaxDate { get; set; }

        public short MinCarAmount { get; set; }

        public short MaxCarAmount { get; set; }

        public decimal MinMoney { get; set; }

        public decimal MaxMoney { get; set; }

        public char MinChar { get; set; }

        public char MaxChar { get; set; }
#pragma warning restore SA1600 // Elements should be documented
    }
}

using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Data class for storing record information.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets id of <see cref="FileCabinetRecord"/>.
        /// </summary>
        /// <value>The id of the <see cref="FileCabinetRecord"/>.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name of <see cref="FileCabinetRecord"/>.
        /// </summary>
        /// <value>The fisrt name of the <see cref="FileCabinetRecord"/>.</value>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets last name of <see cref="FileCabinetRecord"/>.
        /// </summary>
        /// <value>The last name of the <see cref="FileCabinetRecord"/>.</value>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets date of birth of <see cref="FileCabinetRecord"/>.
        /// </summary>
        /// <value>Date of birth of the <see cref="FileCabinetRecord"/>.</value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets car amount of <see cref="FileCabinetRecord"/>.
        /// </summary>
        /// <value>Car amount of the <see cref="FileCabinetRecord"/>.</value>
        public short CarAmount { get; set; }

        /// <summary>
        /// Gets or sets money amount of <see cref="FileCabinetRecord"/>.
        /// </summary>
        /// <value>Amount of money of the <see cref="FileCabinetRecord"/>.</value>
        public decimal Money { get; set; }

        /// <summary>
        /// Gets or sets favorite char of <see cref="FileCabinetRecord"/>.
        /// </summary>
        /// <value>The favorite char of the <see cref="FileCabinetRecord"/>.</value>
        public char FavoriteChar { get; set; }
    }
}

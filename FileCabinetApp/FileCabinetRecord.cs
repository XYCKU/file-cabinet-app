using System;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Data class for storing record information.
    /// </summary>
    public class FileCabinetRecord
    {
        private const string DateTimeFormat = "MM/dd/yyyy";

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class.
        /// </summary>
        /// <param name="id">Id of a new record.</param>
        /// <param name="data"><see cref="FileCabinetData"/>.</param>
        public FileCabinetRecord(int id, FileCabinetData data)
            : this(id, data.FirstName, data.LastName, data.DateOfBirth, data.CarAmount, data.Money, data.FavoriteChar)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class.
        /// </summary>
        /// <param name="id">Id of a record.</param>
        /// <param name="firstName">First name of a record.</param>
        /// <param name="lastName">Last name of a record.</param>
        /// <param name="dateOfBirth">Date of birth of a record.</param>
        /// <param name="carAmount">Car amount of a record.</param>
        /// <param name="money">Money of a record.</param>
        /// <param name="favoriteChar">Favorite char of a record.</param>
        public FileCabinetRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short carAmount, decimal money, char favoriteChar)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.CarAmount = carAmount;
            this.Money = money;
            this.FavoriteChar = favoriteChar;
        }

        /// <summary>
        /// Gets or sets id of <see cref="FileCabinetRecord"/>.
        /// </summary>
        /// <value>The id of the <see cref="FileCabinetRecord"/>.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name of <see cref="FileCabinetRecord"/>.
        /// </summary>
        /// <value>The fisrt name of the <see cref="FileCabinetRecord"/>.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name of <see cref="FileCabinetRecord"/>.
        /// </summary>
        /// <value>The last name of the <see cref="FileCabinetRecord"/>.</value>
        public string LastName { get; set; }

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

        /// <inheritdoc/>
        public override string ToString() => $"#{this.Id}, " +
            $"{this.FirstName}, " +
            $"{this.LastName}, " +
            $"{this.DateOfBirth.ToString(DateTimeFormat, CultureInfo.InvariantCulture)}, " +
            $"{this.CarAmount}, " +
            $"{this.Money}, " +
            $"{this.FavoriteChar}";

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="pastAction">Action.</param>
        /// <returns>A string that represents the current object.</returns>
        public string ToString(string pastAction) => $"First name: {this.FirstName}{Environment.NewLine}" +
            $"Last name: {this.LastName}{Environment.NewLine}" +
            $"Date of birth: {this.DateOfBirth.ToString(DateTimeFormat, CultureInfo.InvariantCulture)}{Environment.NewLine}" +
            $"Car amount: {this.CarAmount}{Environment.NewLine}" +
            $"Money: {this.Money}{Environment.NewLine}" +
            $"Favorite char: {this.Money}{Environment.NewLine}" +
            $"Record #{this.Id} was {pastAction}.";
    }
}

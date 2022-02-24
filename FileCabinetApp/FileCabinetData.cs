namespace FileCabinetApp
{
    /// <summary>
    /// Represents data for FileCabinetRecord.
    /// </summary>
    public class FileCabinetData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetData"/> class.
        /// </summary>
        /// <param name="firstName">FirstName data.</param>
        /// <param name="lastName">LastName data.</param>
        /// <param name="dateOfBirth">DateOfBirth data.</param>
        /// <param name="carAmount">CarAmount data.</param>
        /// <param name="money">Money data.</param>
        /// <param name="favoriteChar">FavoriteChar data.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="firstName"/> or <paramref name="lastName"/> is <c>null</c>.</exception>
        public FileCabinetData(string firstName, string lastName, DateTime dateOfBirth, short carAmount, decimal money, char favoriteChar)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            if (lastName is null)
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.CarAmount = carAmount;
            this.Money = money;
            this.FavoriteChar = favoriteChar;
        }

        /// <summary>
        /// Gets first name data.
        /// </summary>
        /// <value>First name <see cref="string"/>.</value>
        public string FirstName { get; } = string.Empty;

        /// <summary>
        /// Gets last name data.
        /// </summary>
        /// <value>Last name <see cref="string"/>.</value>
        public string LastName { get; } = string.Empty;

        /// <summary>
        /// Gets date of birth data.
        /// </summary>
        /// <value>Date of birth <see cref="DateTime"/>.</value>
        public DateTime DateOfBirth { get; }

        /// <summary>
        /// Gets car amount data.
        /// </summary>
        /// <value>Car amount <see cref="int"/>.</value>
        public short CarAmount { get; }

        /// <summary>
        /// Gets money data.
        /// </summary>
        /// <value>Money <see cref="decimal"/>.</value>
        public decimal Money { get; }

        /// <summary>
        /// Gets favorite char data.
        /// </summary>
        /// <value>Favorite char <see cref="char"/>.</value>
        public char FavoriteChar { get; }
    }
}

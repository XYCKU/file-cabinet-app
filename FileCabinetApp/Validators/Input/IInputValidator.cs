namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Input validator.
    /// </summary>
    public interface IInputValidator
    {
        /// <summary>
        /// Validates given parameter.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <returns>Returns is input valid.</returns>
        Tuple<bool, string> FirstNameValidator(string firstName);

        /// <summary>
        /// Validates given parameter.
        /// </summary>
        /// <param name="lastName">Last name.</param>
        /// <returns>Returns is input valid.</returns>
        Tuple<bool, string> LastNameValidator(string lastName);

        /// <summary>
        /// Validates given parameter.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth.</param>
        /// <returns>Returns is input valid.</returns>
        Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth);

        /// <summary>
        /// Validates given parameter.
        /// </summary>
        /// <param name="carAmount">Car amount.</param>
        /// <returns>Returns is input valid.</returns>
        Tuple<bool, string> CarAmountValidator(short carAmount);

        /// <summary>
        /// Validates given parameter.
        /// </summary>
        /// <param name="money">Money.</param>
        /// <returns>Returns is input valid.</returns>
        Tuple<bool, string> MoneyValidator(decimal money);

        /// <summary>
        /// Validates given parameter.
        /// </summary>
        /// <param name="favoriteChar">Favorite char.</param>
        /// <returns>Returns is input valid.</returns>
        Tuple<bool, string> FavoriteCharValidator(char favoriteChar);
    }
}
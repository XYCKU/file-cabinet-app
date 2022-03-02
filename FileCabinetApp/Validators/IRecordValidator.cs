using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Record Validator interface.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validates given data parameters.
        /// </summary>
        /// <param name="data"><see cref="FileCabinetData"/> parameters.</param>
        public void ValidateParameters(FileCabinetData data);

        /// <summary>
        /// Validates given <paramref name="firstName"/> parameter.
        /// </summary>
        /// <param name="firstName">First name of a person.</param>
        public void ValidateFirstName(string firstName);

        /// <summary>
        /// Validates given <paramref name="lastName"/> parameter.
        /// </summary>
        /// <param name="lastName">Last name of a person.</param>
        public void ValidateLastName(string lastName);

        /// <summary>
        /// Validates given <paramref name="dateOfBirth"/> parameter.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth of a person.</param>
        public void ValidateDateOfBirth(DateTime dateOfBirth);

        /// <summary>
        /// Validates given <paramref name="carAmount"/> parameter.
        /// </summary>
        /// <param name="carAmount">Car amount of a person.</param>
        public void ValidateCarAmount(decimal carAmount);

        /// <summary>
        /// Validates given <paramref name="money"/> parameter.
        /// </summary>
        /// <param name="money">Money amount of a person.</param>
        public void ValidateMoney(decimal money);

        /// <summary>
        /// Validates given <paramref name="favoriteChar"/> parameter.
        /// </summary>
        /// <param name="favoriteChar">Favorite char of a person.</param>
        public void ValidateFavoriteChar(char favoriteChar);
    }
}

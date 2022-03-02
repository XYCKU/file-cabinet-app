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
    }
}

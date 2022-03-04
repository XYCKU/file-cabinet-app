using System;
using System.Collections.ObjectModel;
using FileCabinetApp.Validators;
using FileCabinetApp.Validators.Input;

namespace FileCabinetApp
{
    /// <summary>
    /// Interface for FileCabinetService.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Gets validator for given data.
        /// </summary>
        /// <value>Validator for given data.</value>
        public IRecordValidator Validator { get; }

        /// <summary>
        /// Gets validator for input data.
        /// </summary>
        /// <value>Validator for input data.</value>
        public IInputValidator InputValidator { get; }

        /// <summary>
        /// Creates a new <see cref="FileCabinetRecord"/> instance.
        /// </summary>
        /// <param name="data">Data of a person.</param>
        /// <returns>Id of a new <see cref="FileCabinetRecord"/> instance, created with given parameters.</returns>
        public int CreateRecord(FileCabinetData data);

        /// <summary>
        /// Edits existing <see cref="FileCabinetRecord"/> instance.
        /// </summary>
        /// <param name="id">Id of an instance in list.</param>
        /// <param name="data"><see cref="FileCabinetData"/> with new <see cref="FileCabinetRecord"/> information.</param>
        public void EditRecord(int id, FileCabinetData data);

        /// <summary>
        /// Searching for existing <see cref="FileCabinetRecord"/>s with given <paramref name="firstName"/>.
        /// </summary>
        /// <param name="firstName">Search parameter.</param>
        /// <returns>Array of <see cref="FileCabinetRecord"/> instances, matching the search parameter.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Searching for existing <see cref="FileCabinetRecord"/>s with given <paramref name="lastName"/>.
        /// </summary>
        /// <param name="lastName">Search parameter.</param>
        /// <returns>Array of <see cref="FileCabinetRecord"/> instances, matching the search parameter.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Searching for existing <see cref="FileCabinetRecord"/>s with given <paramref name="dateOfBirth"/>.
        /// </summary>
        /// <param name="dateOfBirth">Search parameter.</param>
        /// <returns>Array of <see cref="FileCabinetRecord"/> instances, matching the search parameter.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth);

        /// <summary>
        /// Get all <see cref="FileCabinetRecord"/>s.
        /// </summary>
        /// <returns>Array of all <see cref="FileCabinetRecord"/> instances.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Get amount of <see cref="FileCabinetRecord"/>s.
        /// </summary>
        /// <returns>Amount of <see cref="FileCabinetRecord"/> instances in list.</returns>
        public Tuple<int, int> GetStat();

        /// <summary>
        /// Creates an instance of <see cref="FileCabinetServiceSnapshot"/>.
        /// </summary>
        /// <returns><see cref="FileCabinetServiceSnapshot"/> of file cabinet service.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// Restores data from <paramref name="snapshot"/>.
        /// </summary>
        /// <param name="snapshot"><see cref="FileCabinetServiceSnapshot"/> with data.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot);

        /// <summary>
        /// Removes record from service.
        /// </summary>
        /// <param name="id">Id of a <see cref="FileCabinetRecord"/>.</param>
        public void RemoveRecord(int id);
    }
}

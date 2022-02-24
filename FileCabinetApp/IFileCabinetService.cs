using System;
using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Interface for FileCabinetService.
    /// </summary>
    public interface IFileCabinetService
    {
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
        public int GetStat();
    }
}

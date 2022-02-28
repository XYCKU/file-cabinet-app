using System;

namespace FileCabinetApp
{
    /// <summary>
    /// FileCabinetRecordReader.
    /// </summary>
    public interface IFileCabinetRecordReader
    {
        /// <summary>
        /// Reads all records.
        /// </summary>
        /// <returns>List of <see cref="FileCabinetRecord"/>.</returns>
        public IList<FileCabinetRecord> ReadAll();
    }
}

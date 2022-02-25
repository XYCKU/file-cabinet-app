namespace FileCabinetApp
{
    /// <summary>
    /// Writes <see cref="FileCabinetRecord"/> to file.
    /// </summary>
    public interface IFileCabinetRecordWriter
    {
        /// <summary>
        /// Writes <see cref="FileCabinetRecord"/> <paramref name="record"/> to file.
        /// </summary>
        /// <param name="record"><see cref="FileCabinetRecord"/> instance.</param>
        public void Write(FileCabinetRecord record);
    }
}
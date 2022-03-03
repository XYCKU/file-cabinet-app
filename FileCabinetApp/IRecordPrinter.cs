namespace FileCabinetApp
{
    /// <summary>
    /// Info printer interface.
    /// </summary>
    public interface IRecordPrinter
    {
        /// <summary>
        /// Prints info.
        /// </summary>
        /// <param name="records">Info to print.</param>
        public void Print(IEnumerable<FileCabinetRecord> records);
    }
}
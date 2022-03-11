namespace FileCabinetApp
{
    /// <summary>
    /// Record iterator interface.
    /// </summary>
    public interface IRecordIterator
    {
        /// <summary>
        /// Gets next item.
        /// </summary>
        /// <returns>Next item.</returns>
        public FileCabinetRecord GetNext();

        /// <summary>
        /// Checks if collection has more elements.
        /// </summary>
        /// <returns>Whether collection has more elements.</returns>
        public bool HasMore();
    }
}

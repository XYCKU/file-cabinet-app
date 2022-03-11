using System.Collections;
using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <inheritdoc/>
    public class FilesystemIterator : IEnumerable<FileCabinetRecord>
    {
        private readonly ReadOnlyCollection<FileCabinetRecord> records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesystemIterator"/> class.
        /// </summary>
        /// <param name="records">Records collection.</param>
        public FilesystemIterator(ReadOnlyCollection<FileCabinetRecord> records)
        {
            this.records = records ?? throw new ArgumentNullException(nameof(records));
        }

        /// <inheritdoc/>
        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            foreach (var record in this.records)
            {
                yield return record;
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

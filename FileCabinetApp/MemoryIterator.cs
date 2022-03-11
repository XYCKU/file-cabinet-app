using System.Collections;
using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <inheritdoc/>
    public class MemoryIterator : IEnumerable<FileCabinetRecord>
    {
        private readonly ReadOnlyCollection<FileCabinetRecord> records;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryIterator"/> class.
        /// </summary>
        /// <param name="records">Records collection.</param>
        public MemoryIterator(ReadOnlyCollection<FileCabinetRecord> records)
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

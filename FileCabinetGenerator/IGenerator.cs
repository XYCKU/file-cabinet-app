using FileCabinetApp;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Generator <see cref="FileCabinetRecord"/>.
    /// </summary>
    public interface IGenerator
    {
        /// <summary>
        /// Generates random <see cref="FileCabinetRecord"/>.
        /// </summary>
        /// <param name="amount">Amount of records to generate.</param>
        /// <param name="startId">Start id of records.</param>
        /// <returns>Array of generated <see cref="FileCabinetRecord"/>.</returns>
        public FileCabinetRecord[] Generate(int amount, int startId = 0);
    }
}

using FileCabinetApp;

namespace FileCabinetGenerator
{
    /// <inheritdoc/>
    public class FileCabinetRecordGenerator : IGenerator
    {
        private const decimal MaxMoney = 99999;
        private const short MaxCars = 55;

        private static readonly Random Random = new Random();

        private static readonly string[] FirstNames =
        {
            "John", "Jack", "Bill", "Arthur", "Abigail", "Mary", "Sarah", "Sadie", "Robert",
            "Franklin", "Lamar", "Trevor", "Michael", "Patrick", "Olivia", "Amelia", "Mia", "Isabella",
        };

        private static readonly string[] LastNames =
        {
            "Smith", "William", "Brown", "Adler", "Morgan", "Bell", "Martinez", "Gonzales", "Lee",
            "Robinson", "Lewis", "King", "Allen", "Hall", "Roberts", "Turner", "Collins", "Cook",
        };

        /// <inheritdoc/>
        public FileCabinetRecord[] Generate(int amount, int startId = 0)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "amount is less than 0.");
            }

            if (startId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startId), "startId is less than 0.");
            }

            var result = new FileCabinetRecord[amount];

            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = Generate(startId + i);
            }

            return result;
        }

        private static T GetRandom<T>(T[] array)
        {
            return array[Random.Next(array.Length)];
        }

        private static DateTime GetRandomDate()
        {
            DateTime minDate = DefaultValidator.EarliestDate;
            int range = (DateTime.Today - minDate).Days;
            return minDate.AddDays(Random.Next(range));
        }

        private static FileCabinetRecord Generate(int id)
        {
            return new FileCabinetRecord(
                id,
                GetRandom(FirstNames),
                GetRandom(LastNames),
                GetRandomDate(),
                (short)Random.Next(MaxCars),
                Math.Round((decimal)Random.NextDouble() * MaxMoney, 2),
                (char)('A' + (char)Random.Next('Z' - 'A')));
        }
    }
}

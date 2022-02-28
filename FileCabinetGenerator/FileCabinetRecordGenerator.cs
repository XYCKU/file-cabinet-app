﻿using FileCabinetApp;

namespace FileCabinetGenerator
{
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

        public FileCabinetRecord[] Generate(int amount, int startId = 0)
        {
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
            return new FileCabinetRecord
            {
                Id = id,
                FirstName = GetRandom(FirstNames),
                LastName = GetRandom(LastNames),
                DateOfBirth = GetRandomDate(),
                CarAmount = (short)Random.Next(MaxCars),
                Money = Math.Round((decimal)Random.NextDouble() * MaxMoney, 2),
                FavoriteChar = (char)('A' + (char)Random.Next('Z' - 'A')),
            };
        }
    }
}
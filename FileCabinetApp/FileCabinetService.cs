using System;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short carAmount, decimal money, char favoriteChar)
        {
            if (firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException("firstName.Length is less than 2 or greater than 60", nameof(firstName));
            }

            if (lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException("lastName.Length is less than 2 or greater than 60", nameof(lastName));
            }

            if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("dateOfBirth is earlier than 01-Jan-1950 or greater than DateTime.Now", nameof(dateOfBirth));
            }

            if (carAmount < 0)
            {
                throw new ArgumentException("carAmount is less than zero", nameof(carAmount));
            }

            if (money < 0)
            {
                throw new ArgumentException("money is less than zero", nameof(money));
            }

            if (char.IsLetter(favoriteChar))
            {
                throw new ArgumentException("favoriteChar is not a letter", nameof(favoriteChar));
            }

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                CarAmount = carAmount,
                Money = money,
                FavoriteChar = favoriteChar,
            };

            this.list.Add(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }
    }
}

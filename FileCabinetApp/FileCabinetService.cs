using System;
using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<string, List<FileCabinetRecord>>();

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

            if (!char.IsLetter(favoriteChar))
            {
                throw new ArgumentException($"favoriteChar {favoriteChar} is not a letter", nameof(favoriteChar));
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

            if (!this.firstNameDictionary.ContainsKey(record.FirstName))
            {
                this.firstNameDictionary[record.FirstName] = new List<FileCabinetRecord>();
            }

            if (!this.lastNameDictionary.ContainsKey(record.LastName))
            {
                this.lastNameDictionary[record.LastName] = new List<FileCabinetRecord>();
            }

            string stringDate = record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

            if (!this.dateOfBirthDictionary.ContainsKey(stringDate))
            {
                this.dateOfBirthDictionary[stringDate] = new List<FileCabinetRecord>();
            }

            this.firstNameDictionary[record.FirstName].Add(record);
            this.lastNameDictionary[record.LastName].Add(record);
            this.dateOfBirthDictionary[stringDate].Add(record);

            return record.Id;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short carAmount, decimal money, char favoriteChar)
        {
            if (id < 0)
            {
                throw new ArgumentException("id cannot be less than 0", nameof(id));
            }

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

            if (!char.IsLetter(favoriteChar))
            {
                throw new ArgumentException($"favoriteChar {favoriteChar} is not a letter", nameof(favoriteChar));
            }

            if (id >= this.list.Count)
            {
                throw new ArgumentException($"#{id} record is not found.", nameof(id));
            }

            this.list[id].FirstName = firstName;
            this.list[id].LastName = lastName;
            this.list[id].DateOfBirth = dateOfBirth;
            this.list[id].CarAmount = carAmount;
            this.list[id].Money = money;
            this.list[id].FavoriteChar = favoriteChar;
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            List<FileCabinetRecord> result = new List<FileCabinetRecord>();

            for (int i = 0; i < this.list.Count; ++i)
            {
                if (string.Equals(this.list[i].FirstName, firstName, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(this.list[i]);
                }
            }

            return result.ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            List<FileCabinetRecord> result = new List<FileCabinetRecord>();

            for (int i = 0; i < this.list.Count; ++i)
            {
                if (string.Equals(this.list[i].LastName, lastName, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(this.list[i]);
                }
            }

            return result.ToArray();
        }

        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();

            for (int i = 0; i < this.list.Count; ++i)
            {
                if (DateTime.Equals(this.list[i].DateOfBirth, dateOfBirth))
                {
                    result.Add(this.list[i]);
                }
            }

            return result.ToArray();
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

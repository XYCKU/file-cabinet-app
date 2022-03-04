using System.Collections.ObjectModel;
using System.Globalization;
using FileCabinetApp.Validators;
using FileCabinetApp.Validators.Input;

namespace FileCabinetApp
{
    /// <summary>
    /// Service for creating, editing and storing <see cref="FileCabinetRecord"/>s.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        /// <summary>
        /// The earliest date that can DateOfBirth field has.
        /// </summary>
        protected static readonly DateTime EarliestDate = new DateTime(1950, 01, 01);

        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        private int lastId;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class with validator.
        /// </summary>
        /// <param name="validator">Validator.</param>
        /// <param name="inputValidator">Input validator.</param>
        public FileCabinetMemoryService(IRecordValidator validator, IInputValidator inputValidator)
        {
            if (validator is null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            if (inputValidator is null)
            {
                throw new ArgumentNullException(nameof(inputValidator));
            }

            this.Validator = validator;
            this.InputValidator = inputValidator;
        }

        /// <inheritdoc/>
        public IRecordValidator Validator { get; }

        /// <inheritdoc/>
        public IInputValidator InputValidator { get; }

        /// <summary>
        /// Creates a new <see cref="FileCabinetRecord"/> instance.
        /// </summary>
        /// <param name="data">Data of a person.</param>
        /// <returns>Id of a new <see cref="FileCabinetRecord"/> instance, created with given parameters.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="data.FirstName"/> or <paramref name="data.LastName"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.FirstName"/>.Length or <paramref name="data.LastName"/>.Length is less than 2 or greater than 60.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.DateOfBirth"/> is earlier than 01-01-1950 or later than <see cref="DateTime"/>.Now.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.CarAmount"/> or <paramref name="data.Money"/> is less than zero.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.FavoriteChar"/> is not a letter of english alphaber.</exception>
        public int CreateRecord(FileCabinetData data)
        {
            this.Validator.ValidateParameters(data);
            var record = new FileCabinetRecord(this.lastId++, data);

            this.list.Add(record);

            AddToDictionary(this.firstNameDictionary, record.FirstName.ToUpperInvariant(), record);
            AddToDictionary(this.lastNameDictionary, record.LastName.ToUpperInvariant(), record);
            AddToDictionary(this.dateOfBirthDictionary, record.DateOfBirth, record);

            return record.Id;
        }

        /// <summary>
        /// Edits existing <see cref="FileCabinetRecord"/> instance.
        /// </summary>
        /// <param name="id">Id of a record in list.</param>
        /// <param name="data"><see cref="FileCabinetData"/> with new <see cref="FileCabinetRecord"/> information.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="data.FirstName"/> or <paramref name="data.LastName"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="id"/>is less than zero or record doesn't exist.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.FirstName"/>.Length or <paramref name="data.LastName"/>.Length is less than 2 or greater than 60.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.DateOfBirth"/> is earlier than 01-01-1950 or later than <see cref="DateTime"/>.Now.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.CarAmount"/> or <paramref name="data.Money"/> is less than zero.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.FavoriteChar"/> is not a letter of english alphaber.</exception>
        public void EditRecord(int id, FileCabinetData data)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "id cannot be less than 0.");
            }

            int index = this.FindById(id);

            if (index == -1)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "record is not found.");
            }

            this.Validator.ValidateParameters(data);

            if (!string.Equals(this.list[index].FirstName, data.FirstName, StringComparison.OrdinalIgnoreCase))
            {
                RemoveFromDictionary(this.firstNameDictionary, this.list[index].FirstName.ToUpperInvariant(), this.list[index]);
                this.list[index].FirstName = data.FirstName;
                AddToDictionary(this.firstNameDictionary, this.list[index].FirstName.ToUpperInvariant(), this.list[index]);
            }

            if (!string.Equals(this.list[index].LastName, data.LastName, StringComparison.OrdinalIgnoreCase))
            {
                RemoveFromDictionary(this.lastNameDictionary, this.list[index].LastName.ToUpperInvariant(), this.list[index]);
                this.list[index].LastName = data.LastName;
                AddToDictionary(this.lastNameDictionary, this.list[index].LastName.ToUpperInvariant(), this.list[index]);
            }

            if (this.list[index].DateOfBirth != data.DateOfBirth)
            {
                RemoveFromDictionary(this.dateOfBirthDictionary, this.list[index].DateOfBirth, this.list[index]);
                this.list[index].DateOfBirth = data.DateOfBirth;
                AddToDictionary(this.dateOfBirthDictionary, this.list[index].DateOfBirth, this.list[index]);
            }

            this.list[index].CarAmount = data.CarAmount;
            this.list[index].Money = data.Money;
            this.list[index].FavoriteChar = data.FavoriteChar;
        }

        /// <summary>
        /// Searching for existing <see cref="FileCabinetRecord"/>s with given <paramref name="firstName"/>.
        /// </summary>
        /// <param name="firstName">Search parameter.</param>
        /// <returns>Array of <see cref="FileCabinetRecord"/> instances, matching the search parameter.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="firstName"/> is <c>null</c> or whitespace.</exception>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            return FindBy(this.firstNameDictionary, firstName.ToUpperInvariant());
        }

        /// <summary>
        /// Searching for existing <see cref="FileCabinetRecord"/>s with given <paramref name="lastName"/>.
        /// </summary>
        /// <param name="lastName">Search parameter.</param>
        /// <returns>Array of <see cref="FileCabinetRecord"/> instances, matching the search parameter.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="lastName"/> is <c>null</c> or whitespace.</exception>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            return FindBy(this.lastNameDictionary, lastName.ToUpperInvariant());
        }

        /// <summary>
        /// Searching for existing <see cref="FileCabinetRecord"/>s with given <paramref name="dateOfBirth"/>.
        /// </summary>
        /// <param name="dateOfBirth">Search parameter.</param>
        /// <returns>Array of <see cref="FileCabinetRecord"/> instances, matching the search parameter.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="dateOfBirth"/> is earlier than 01-01-1950 or later than <see cref="DateTime"/>.Now.</exception>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth < EarliestDate || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"dateOfBirth is earlier than {EarliestDate.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)} or greater than DateTime.Now", nameof(dateOfBirth));
            }

            return FindBy(this.dateOfBirthDictionary, dateOfBirth);
        }

        /// <summary>
        /// Get all <see cref="FileCabinetRecord"/>s.
        /// </summary>
        /// <returns>Array of all <see cref="FileCabinetRecord"/> instances.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return new ReadOnlyCollection<FileCabinetRecord>(this.list);
        }

        /// <summary>
        /// Get amount of <see cref="FileCabinetRecord"/>s.
        /// </summary>
        /// <returns>Amount of <see cref="FileCabinetRecord"/> instances in list.</returns>
        public Tuple<int, int> GetStat()
        {
            return new Tuple<int, int>(this.list.Count, 0);
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.list.ToArray());
        }

        /// <inheritdoc/>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.list.Clear();
            this.list.AddRange(snapshot.Records.ToList());

            this.firstNameDictionary.Clear();
            this.lastNameDictionary.Clear();
            this.dateOfBirthDictionary.Clear();

            for (int i = 0; i < this.list.Count; ++i)
            {
                AddToDictionary(this.firstNameDictionary, this.list[i].FirstName, this.list[i]);
                AddToDictionary(this.lastNameDictionary, this.list[i].LastName, this.list[i]);
                AddToDictionary(this.dateOfBirthDictionary, this.list[i].DateOfBirth, this.list[i]);
            }

            if (this.list.Count > 0)
            {
                this.lastId = this.list[^1].Id;
            }
            else
            {
                this.lastId = 0;
            }
        }

        /// <inheritdoc/>
        public void RemoveRecord(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "id cannot be less than 0.");
            }

            int index = this.FindById(id);

            if (index != -1)
            {
                RemoveFromDictionary(this.firstNameDictionary, this.list[index].FirstName.ToLowerInvariant(), this.list[index]);
                RemoveFromDictionary(this.lastNameDictionary, this.list[index].LastName.ToLowerInvariant(), this.list[index]);
                RemoveFromDictionary(this.dateOfBirthDictionary, this.list[index].DateOfBirth, this.list[index]);

                this.list.RemoveAt(index);
                return;
            }

            throw new ArgumentException($"Record #{id} doesn't exist", nameof(id));
        }

        /// <inheritdoc/>
        public override string ToString() => "memory";

        private static ReadOnlyCollection<FileCabinetRecord> FindBy<T>(Dictionary<T, List<FileCabinetRecord>> dictionary, T parameter)
            where T : notnull
        {
            if (!dictionary.ContainsKey(parameter))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(Array.Empty<FileCabinetRecord>());
            }

            return new ReadOnlyCollection<FileCabinetRecord>(dictionary[parameter]);
        }

        private static void AddToDictionary<T>(Dictionary<T, List<FileCabinetRecord>> dictionary, T value, FileCabinetRecord record)
            where T : notnull
        {
            if (!dictionary.ContainsKey(value))
            {
                dictionary[value] = new List<FileCabinetRecord>();
            }

            dictionary[value].Add(record);
        }

        private static void RemoveFromDictionary<T>(Dictionary<T, List<FileCabinetRecord>> dictionary, T value, FileCabinetRecord record)
            where T : notnull
        {
            if (!dictionary.ContainsKey(value))
            {
                return;
            }

            dictionary[value].Remove(record);
        }

        private int FindById(int id)
        {
            if (id < 0 || id > this.lastId)
            {
                return -1;
            }

            for (int i = 0; i < this.list.Count && id <= this.list[i].Id; ++i)
            {
                if (id == this.list[i].Id)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}

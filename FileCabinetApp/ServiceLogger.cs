using System.Collections.ObjectModel;
using System.Globalization;
using FileCabinetApp.Validators;
using FileCabinetApp.Validators.Input;

namespace FileCabinetApp
{
    /// <inheritdoc/>
    public class ServiceLogger : IFileCabinetService
    {
        private readonly IFileCabinetService fileCabinetService;
        private readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogger"/> class.
        /// </summary>
        /// <param name="fileCabinetService">FileCabinetService.</param>
        /// <param name="writer">TextWriter.</param>
        public ServiceLogger(IFileCabinetService fileCabinetService, TextWriter writer)
        {
            this.fileCabinetService = fileCabinetService ?? throw new ArgumentNullException(nameof(fileCabinetService));
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        /// <inheritdoc/>
        public IRecordValidator Validator => this.fileCabinetService.Validator;

        /// <inheritdoc/>
        public IInputValidator InputValidator => this.fileCabinetService.InputValidator;

        /// <inheritdoc/>
        public int CreateRecord(FileCabinetData data)
        {
            this.Log($"Calling CreateRecord() with {data}");
            var result = this.fileCabinetService.CreateRecord(data);
            this.Log($"CreateRecord() returned '{result}'");
            return result;
        }

        /// <inheritdoc/>
        public void EditRecord(int id, FileCabinetData data)
        {
            this.Log($"Calling EditRecord() with {id} and {data}");
            this.fileCabinetService.EditRecord(id, data);
            this.Log($"EditRecord() edited #{id} record");
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            this.Log($"Calling FindByDateOfBirth() with {dateOfBirth}");
            var result = this.fileCabinetService.FindByDateOfBirth(dateOfBirth);
            this.Log($"FindByDateOfBirth() returned '{result.Count}' records");
            return result;
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.Log($"Calling FindByFirstName() with {firstName}");
            var result = this.fileCabinetService.FindByFirstName(firstName);
            this.Log($"FindByFirstName() returned '{result.Count}' records");
            return result;
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.Log($"Calling FindByLastName() with {lastName}");
            var result = this.fileCabinetService.FindByLastName(lastName);
            this.Log($"FindByLastName() returned '{result.Count}' records");
            return result;
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.Log($"Calling GetRecords()");
            var result = this.fileCabinetService.GetRecords();
            this.Log($"GetRecords() returned '{result.Count}' records");
            return result;
        }

        /// <inheritdoc/>
        public Tuple<int, int> GetStat()
        {
            this.Log($"Calling GetStat()");
            var result = this.fileCabinetService.GetStat();
            this.Log($"GetStat() returned '{result.Item1}' and '{result.Item2}'");
            return result;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            this.Log($"Calling MakeSnapshot()");
            var result = this.fileCabinetService.MakeSnapshot();
            this.Log($"MakeSnapshot() returned snapshot with '{result.Records.Count}' records");
            return result;
        }

        /// <inheritdoc/>
        public void RemoveRecord(int id)
        {
            this.Log($"Calling RemoveRecord() with {id}");
            this.fileCabinetService.RemoveRecord(id);
            this.Log($"RemoveRecord() removed record with #{id} id");
        }

        /// <inheritdoc/>
        public override string ToString() => this.fileCabinetService.ToString() ?? string.Empty;

        /// <inheritdoc/>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.Log($"Calling Restore()");
            this.fileCabinetService.Restore(snapshot);
            this.Log($"Restore() restored snapshot with {snapshot.Records.Count} records");
        }

        private void Log(string line)
        {
            this.writer.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture)} - {line}");
        }
    }
}

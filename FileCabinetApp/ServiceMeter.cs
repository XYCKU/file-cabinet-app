using System.Collections.ObjectModel;
using System.Diagnostics;
using FileCabinetApp.Validators;
using FileCabinetApp.Validators.Input;

namespace FileCabinetApp
{
    /// <inheritdoc/>
    public class ServiceMeter : IFileCabinetService
    {
        private readonly IFileCabinetService fileCabinetService;
        private readonly Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMeter"/> class.
        /// </summary>
        /// <param name="service">FileCabinetService.</param>
        public ServiceMeter(IFileCabinetService service)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            this.fileCabinetService = service;
        }

        /// <inheritdoc/>
        public IRecordValidator Validator => this.fileCabinetService.Validator;

        /// <inheritdoc/>
        public IInputValidator InputValidator => this.fileCabinetService.InputValidator;

        /// <inheritdoc/>
        public int CreateRecord(FileCabinetData data)
        {
            this.stopwatch.Restart();
            var result = this.fileCabinetService.CreateRecord(data);
            this.stopwatch.Stop();
            Console.WriteLine($"CreateRecord method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <inheritdoc/>
        public void EditRecord(int id, FileCabinetData data)
        {
            this.stopwatch.Restart();
            this.fileCabinetService.EditRecord(id, data);
            this.stopwatch.Stop();
            Console.WriteLine($"EditRecord method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            this.stopwatch.Restart();
            var result = this.fileCabinetService.FindByDateOfBirth(dateOfBirth);
            this.stopwatch.Stop();
            Console.WriteLine($"FindByDateOfBirth method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.stopwatch.Restart();
            var result = this.fileCabinetService.FindByFirstName(firstName);
            this.stopwatch.Stop();
            Console.WriteLine($"FindByFirstName method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.stopwatch.Restart();
            var result = this.fileCabinetService.FindByLastName(lastName);
            this.stopwatch.Stop();
            Console.WriteLine($"FindByLastName method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.stopwatch.Restart();
            var result = this.fileCabinetService.GetRecords();
            this.stopwatch.Stop();
            Console.WriteLine($"GetRecords method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <inheritdoc/>
        public Tuple<int, int> GetStat()
        {
            this.stopwatch.Restart();
            var result = this.fileCabinetService.GetStat();
            this.stopwatch.Stop();
            Console.WriteLine($"GetStat method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            this.stopwatch.Restart();
            var result = this.fileCabinetService.MakeSnapshot();
            this.stopwatch.Stop();
            Console.WriteLine($"MakeSnapshot method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        /// <inheritdoc/>
        public void RemoveRecord(int id)
        {
            this.stopwatch.Restart();
            this.fileCabinetService.RemoveRecord(id);
            this.stopwatch.Stop();
            Console.WriteLine($"RemoveRecord method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
        }

        /// <inheritdoc/>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.stopwatch.Restart();
            this.fileCabinetService.Restore(snapshot);
            this.stopwatch.Stop();
            Console.WriteLine($"Restore method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
        }

        /// <inheritdoc/>
        public override string ToString() => this.fileCabinetService.ToString() ?? string.Empty;
    }
}

using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service.</param>
        public CreateCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string Command { get; } = "create";

        /// <inheritdoc/>
        protected override void Action(AppCommandRequest commandRequest)
        {
            int recordId;

            Console.Write("First name: ");
            string firstName = Program.ReadInput(InputConverter.StringConverter, Program.InputValidator.FirstNameValidator);

            Console.Write("Last name: ");
            string lastName = Program.ReadInput(InputConverter.StringConverter, Program.InputValidator.LastNameValidator);

            Console.Write("Date of birth: ");
            DateTime dt = Program.ReadInput(InputConverter.DateConverter, Program.InputValidator.DateOfBirthValidator);

            Console.Write("Car amount: ");
            short carAmount = Program.ReadInput(InputConverter.ShortConverter, Program.InputValidator.CarAmountValidator);

            Console.Write("Money: ");
            decimal money = Program.ReadInput(InputConverter.DecimalConverter, Program.InputValidator.MoneyValidator);

            Console.Write("Favorite char: ");
            char favoriteChar = Program.ReadInput(InputConverter.CharConverter, Program.InputValidator.FavoriteCharValidator);

            var data = new FileCabinetData(firstName, lastName, dt, carAmount, money, favoriteChar);

            recordId = this.Service.CreateRecord(data);

            Console.WriteLine(Program.LongFormatRecord(data, recordId, "created"));
        }
    }
}

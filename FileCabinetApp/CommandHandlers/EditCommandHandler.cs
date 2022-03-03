using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Service.</param>
        public EditCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string Command { get; } = "edit";

        /// <inheritdoc/>
        protected override void Action(AppCommandRequest commandRequest)
        {
            if (string.IsNullOrWhiteSpace(commandRequest.Parameters))
            {
                Console.WriteLine("Invalid arguments");
                return;
            }

            int id;
            if (!int.TryParse(commandRequest.Parameters, out id))
            {
                Console.WriteLine("Invalid id format");
                return;
            }

            if (id < 0)
            {
                Console.WriteLine($"{id} id is invalid.");
                return;
            }

            if (id >= this.Service.GetStat().Item1)
            {
                Console.WriteLine($"#{id} record is not found.");
                return;
            }

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

            this.Service.EditRecord(id, data);

            Console.WriteLine(Program.LongFormatRecord(data, id, "updated"));
        }
    }
}

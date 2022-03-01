using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <inheritdoc/>
    public class CreateCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        protected override string Command { get; } = "create";

        /// <inheritdoc/>
        protected override void Action(AppCommandRequest commandRequest)
        {
            int recordId;

            Console.Write("First name: ");
            string firstName = Program.ReadInput(Program.StringConverter, Program.FirstNameValidator);

            Console.Write("Last name: ");
            string lastName = Program.ReadInput(Program.StringConverter, Program.LastNameValidator);

            Console.Write("Date of birth: ");
            DateTime dt = Program.ReadInput(Program.DateConverter, Program.DateOfBirthValidator);

            Console.Write("Car amount: ");
            short carAmount = Program.ReadInput(Program.ShortConverter, Program.CarAmountValidator);

            Console.Write("Money: ");
            decimal money = Program.ReadInput(Program.DecimalConverter, Program.MoneyValidator);

            Console.Write("Favorite char: ");
            char favoriteChar = Program.ReadInput(Program.CharConverter, Program.FavoriteCharValidator);

            var data = new FileCabinetData(firstName, lastName, dt, carAmount, money, favoriteChar);

            recordId = Program.FileCabinetService.CreateRecord(data);

            Console.WriteLine(Program.LongFormatRecord(data, recordId, "created"));
        }
    }
}

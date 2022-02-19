using System;

namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public short CarAmount { get; set; }

        public decimal Money { get; set; }

        public char FavoriteChar { get; set; }
    }
}

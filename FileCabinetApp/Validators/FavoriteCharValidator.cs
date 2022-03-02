namespace FileCabinetApp.Validators
{
    /// <inheritdoc/>
    public class FavoriteCharValidator : IRecordValidator
    {
        private readonly char minCharacter;
        private readonly char maxCharacter;

        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteCharValidator"/> class.
        /// </summary>
        /// <param name="from">Min character.</param>
        /// <param name="to">Max character.</param>
        public FavoriteCharValidator(char from, char to)
        {
            if (from > to)
            {
                throw new ArgumentException($"from ({from}) is greater than to({to})", nameof(from));
            }

            this.minCharacter = from;
            this.maxCharacter = to;
        }

        /// <inheritdoc/>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.FavoriteChar"/> is not a letter of english alphabet.</exception>
        public void ValidateParameters(FileCabinetData data)
        {
            if (data.FavoriteChar < this.minCharacter || data.FavoriteChar > this.maxCharacter)
            {
                throw new ArgumentException($"favoriteChar {data.FavoriteChar} is not between {this.minCharacter} and {this.maxCharacter}.", nameof(data));
            }
        }
    }
}
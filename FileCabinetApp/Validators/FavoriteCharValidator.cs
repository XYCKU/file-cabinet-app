namespace FileCabinetApp.Validators
{
    /// <inheritdoc/>
    public class FavoriteCharValidator : IRecordValidator
    {
        private readonly char minChar;
        private readonly char maxChar;

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

            this.minChar = from;
            this.maxChar = to;
        }

        /// <inheritdoc/>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="data.FavoriteChar"/> is not a letter of english alphabet.</exception>
        public void ValidateParameters(FileCabinetData data)
        {
            if (data.FavoriteChar < this.minChar || data.FavoriteChar > this.maxChar)
            {
                throw new ArgumentException($"favoriteChar {data.FavoriteChar} is not between {this.minChar} and {this.maxChar}", nameof(data));
            }
        }
    }
}
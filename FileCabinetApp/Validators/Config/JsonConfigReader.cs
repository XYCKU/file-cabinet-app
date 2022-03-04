using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FileCabinetApp.Validators.Config
{
    /// <inheritdoc/>
    public class JsonConfigReader : IConfigReader
    {
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
        {
            Culture = new CultureInfo("en-US"),
        };

        private readonly StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConfigReader"/> class.
        /// </summary>
        /// <param name="reader">StreamReader.</param>
        public JsonConfigReader(StreamReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            this.reader = reader;
        }

        /// <inheritdoc/>
        public Dictionary<string, ConfigurationData> ReadAllConfigs()
        {
            var definition = new
            {
                firstName = new { min = 0, max = 0 },
                lastName = new { min = 0, max = 0 },
                dateOfBirth = new { from = DateTime.Today, to = DateTime.Today },
                carAmount = new { min = short.MinValue, max = short.MaxValue },
                money = new { min = decimal.Zero, max = decimal.MaxValue },
                favoriteChar = new { min = 'A', max = 'Z' },
            };

            var dataSet = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(this.reader.ReadToEnd());

            var result = new Dictionary<string, ConfigurationData>();

            if (dataSet is null)
            {
                return result;
            }

            foreach (var data in dataSet)
            {
                var item = JsonConvert.DeserializeAnonymousType(data.Value.ToString(), definition, JsonSettings);

                if (item is null)
                {
                    continue;
                }

                result.Add(data.Key, new ConfigurationData
                {
                    MinFirstNameLength = item.firstName.min,
                    MaxFirstNameLength = item.firstName.max,
                    MinLastNameLength = item.lastName.min,
                    MaxLastNameLength = item.lastName.max,
                    MinDate = item.dateOfBirth.from,
                    MaxDate = item.dateOfBirth.to,
                    MinCarAmount = item.carAmount.min,
                    MaxCarAmount = item.carAmount.max,
                    MinMoney = item.money.min,
                    MaxMoney = item.money.max,
                    MinChar = item.favoriteChar.min,
                    MaxChar = item.favoriteChar.max,
                });
            }

            return result;
        }
    }
}

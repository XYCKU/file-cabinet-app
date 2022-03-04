using System;

namespace FileCabinetApp.Validators.Config
{
    /// <summary>
    /// Interface for reading config files.
    /// </summary>
    public interface IConfigReader
    {
        /// <summary>
        /// Reads all configs from json file.
        /// </summary>
        /// <returns>Dictionary of config name and datas.</returns>
        public Dictionary<string, ConfigurationData> ReadAllConfigs();
    }
}

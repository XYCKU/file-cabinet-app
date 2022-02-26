using FileCabinetApp;

namespace FileCabinetGenerator
{
    public interface IExporter
    {
        public void ExportRecords(FileCabinetRecord[] records);
    }
}

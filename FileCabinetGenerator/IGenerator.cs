using FileCabinetApp;

namespace FileCabinetGenerator
{
    public interface IGenerator
    {
        public FileCabinetRecord[] Generate(int amount, int startId = 0);
    }
}

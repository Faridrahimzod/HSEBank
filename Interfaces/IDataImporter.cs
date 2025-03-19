using Models;

namespace Interfaces
{
    public interface IDataImporter
    {
        (
            List<BankAccount>,
            List<Category>,
            List<Operation>
        ) Import(string filePath);
    }
}
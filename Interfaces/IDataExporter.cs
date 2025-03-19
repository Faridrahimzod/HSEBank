using Models;

namespace Interfaces
{
    public interface IDataExporter
    {
        void Export(
            IEnumerable<BankAccount> accounts,
            IEnumerable<Category> categories,
            IEnumerable<Operation> operations,
            string filePath
        );
    }
}
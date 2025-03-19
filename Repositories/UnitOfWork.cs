using ImportExport;
using Analytics;
using Interfaces;
using Models;
using Repositories;

namespace Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IRepository<BankAccount> Accounts { get; }
        public IRepository<Category> Categories { get; }
        public IRepository<Operation> Operations { get; }
        public IFinancialAnalyticsService Analytics { get; }
        public BalanceRecalculationService BalanceRecalculator { get; }

        public UnitOfWork()
        {
            Accounts = new AccountRepository();
            Categories = new CategoryRepository();
            Operations = new OperationRepository();
            Analytics = new FinancialAnalyticsService(Operations);
            BalanceRecalculator = new BalanceRecalculationService(Accounts, Operations);
        }

        public void Commit() { }

        public void ExportData(ExportFormat format, string path)
        {
            var exporter = ImportExportFactory.CreateExporter(format);
            exporter.Export(
                Accounts.GetAll(),
                Categories.GetAll(),
                Operations.GetAll(),
                path
            );
        }

        public void ImportData(ExportFormat format, string path)
        {
            var importer = ImportExportFactory.CreateImporter(format);
            var (accounts, categories, operations) = importer.Import(path);

            foreach (var a in accounts) Accounts.Add(a);
            foreach (var c in categories) Categories.Add(c);
            foreach (var o in operations) Operations.Add(o);
        }
    }
}


using Analytics;
using ImportExport;
using Interfaces;
using Models;
using Repositories;
using System;
using System.Globalization;
using System.Linq;

namespace HSEBankApp.ConsoleUI
{
    class Program
    {
        private static UnitOfWork _unitOfWork = new UnitOfWork();
        private static FinancialAnalyticsService _analyticsService;
        private static BalanceRecalculationService _balanceService;

        static void Main(string[] args)
        {
            _analyticsService = new FinancialAnalyticsService(_unitOfWork.Operations);
            _balanceService = new BalanceRecalculationService(_unitOfWork.Accounts, _unitOfWork.Operations);

            InitializeSampleData();

            while (true)
            {
                Console.Clear();
                ShowMainMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageAccounts();
                        break;
                    case "2":
                        ManageCategories();
                        break;
                    case "3":
                        ManageOperations();
                        break;
                    case "4":
                        ShowAnalytics();
                        break;
                    case "5":
                        ImportExportData();
                        break;
                    case "6":
                        RecalculateBalances();
                        break;
                    case "0":
                        return;
                    default:
                        ShowError("Неверный выбор!");
                        break;
                }
            }
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("=== Учет финансов ===");
            Console.WriteLine("1. Управление счетами");
            Console.WriteLine("2. Управление категориями");
            Console.WriteLine("3. Управление операциями");
            Console.WriteLine("4. Аналитика");
            Console.WriteLine("5. Импорт/Экспорт");
            Console.WriteLine("6. Пересчет балансов");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");
        }

        static void ManageAccounts()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Управление счетами ===");
                Console.WriteLine("1. Создать счет");
                Console.WriteLine("2. Список счетов");
                Console.WriteLine("3. Назад");
                Console.Write("Выберите действие: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        CreateAccount();
                        break;
                    case "2":
                        ShowAccounts();
                        break;
                    case "3":
                        return;
                    default:
                        ShowError("Неверный выбор!");
                        break;
                }
            }
        }

        static void CreateAccount()
        {
            Console.Write("Введите название счета: ");
            var name = Console.ReadLine();

            var account = new BankAccount(name);
            _unitOfWork.Accounts.Add(account);
            _unitOfWork.Commit();

            ShowSuccess($"Счет '{name}' создан! ID: {account.Id}");
        }

        static void ShowAccounts()
        {
            Console.WriteLine("\nСписок счетов:");
            foreach (var account in _unitOfWork.Accounts.GetAll())
            {
                Console.WriteLine($"[{account.Id}] {account.Name} - {account.Balance:C}");
            }
            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void ManageCategories()
        {
            // Аналогичная реализация для категорий
        }

        static void ManageOperations()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Управление операциями ===");
                Console.WriteLine("1. Добавить операцию");
                Console.WriteLine("2. История операций");
                Console.WriteLine("3. Назад");
                Console.Write("Выберите действие: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddOperation();
                        break;
                    case "2":
                        ShowOperationHistory();
                        break;
                    case "3":
                        return;
                    default:
                        ShowError("Неверный выбор!");
                        break;
                }
            }
        }

        static void AddOperation()
        {
            try
            {
                Console.Write("Тип операции (1 - Доход, 2 - Расход): ");
                var typeInput = Console.ReadLine();
                var type = typeInput == "1" ? TransactionType.Income : TransactionType.Expense;

                Console.Write("Сумма: ");
                var amount = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

                Console.Write("Дата операции (гггг-мм-дд): ");
                var date = DateTime.Parse(Console.ReadLine());

                Console.Write("Описание: ");
                var description = Console.ReadLine();

                ShowAccounts();
                Console.Write("ID счета: ");
                var accountId = Guid.Parse(Console.ReadLine());

                var categories = _unitOfWork.Categories.GetAll()
                    .Where(c => c.Type == type)
                    .ToList();

                Console.WriteLine("Доступные категории:");
                foreach (var cat in categories)
                {
                    Console.WriteLine($"[{cat.Id}] {cat.Name}");
                }

                Console.Write("ID категории: ");
                var categoryId = Guid.Parse(Console.ReadLine());

                var operation = new Operation(
                    type,
                    accountId,
                    amount,
                    date,
                    categoryId,
                    description
                );

                _unitOfWork.Operations.Add(operation);
                _unitOfWork.Commit();

                ShowSuccess("Операция добавлена!");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка: {ex.Message}");
            }
        }

        static void ShowOperationHistory()
        {
            Console.WriteLine("\nИстория операций:");
            foreach (var op in _unitOfWork.Operations.GetAll())
            {
                var account = _unitOfWork.Accounts.GetById(op.BankAccountId);
                var category = _unitOfWork.Categories.GetById(op.CategoryId);

                Console.WriteLine(
                    $"[{op.Date:yyyy-MM-dd}] {account.Name} | " +
                    $"{category.Name} | " +
                    $"{(op.Type == TransactionType.Income ? "+" : "-")}{op.Amount:C} | " +
                    $"{op.Description}"
                );
            }
            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void ShowAnalytics()
        {
            Console.Clear();
            Console.WriteLine("=== Финансовая аналитика ===");
            Console.Write("Начальная дата (гггг-мм-дд): ");
            var start = DateTime.Parse(Console.ReadLine());

            Console.Write("Конечная дата (гггг-мм-дд): ");
            var end = DateTime.Parse(Console.ReadLine());

            var difference = _analyticsService.GetIncomeExpenseDifference(start, end);
            Console.WriteLine($"\nБаланс за период: {difference:C}");

            var grouping = new CategoryGroupingStrategy().Group(
                _unitOfWork.Operations.GetAll().Where(o => o.Date >= start && o.Date <= end),
                _unitOfWork.Categories
            );

            Console.WriteLine("\nРасходы по категориям:");
            foreach (var item in grouping.Where(g => g.Key.Equals(TransactionType.Expense)))
            {
                Console.WriteLine($"{item.Key.PadRight(20)} {item.Value:C}");
            }

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void ImportExportData()
        {
            Console.Clear();
            Console.WriteLine("=== Импорт/Экспорт ===");
            Console.WriteLine("1. Экспорт из CSV");
            Console.WriteLine("2. Экспорт в JSON");
            Console.WriteLine("3. Экспорт в YAML");

            Console.WriteLine("4. Импорт из CSV");
            Console.WriteLine("5. Импорт из JSON");
            Console.WriteLine("6. Импорт из YAML");

            Console.WriteLine("7. Назад");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();
            if (choice == "1")
            {
                var exporter = ImportExportFactory.CreateExporter(ExportFormat.Csv);
                exporter.Export(
                    _unitOfWork.Accounts.GetAll(),
                    _unitOfWork.Categories.GetAll(),
                    _unitOfWork.Operations.GetAll(),
                    "export.csv"
                );
                ShowSuccess("Данные экспортированы в export.csv");
            }
            else if (choice == "2")
            {
                var exporter = ImportExportFactory.CreateExporter(ExportFormat.Json);
                exporter.Export(
                    _unitOfWork.Accounts.GetAll(),
                    _unitOfWork.Categories.GetAll(),
                    _unitOfWork.Operations.GetAll(),
                    "export.json"
                );
                ShowSuccess("Данные экспортированы в export.json");
            }
            else if (choice == "3")
            {
                var exporter = ImportExportFactory.CreateExporter(ExportFormat.Yaml);
                exporter.Export(
                    _unitOfWork.Accounts.GetAll(),
                    _unitOfWork.Categories.GetAll(),
                    _unitOfWork.Operations.GetAll(),
                    "export.yaml"
                );
                ShowSuccess("Данные экспортированы в export.yaml");
            }
            else if (choice == "4")
            {
                var importer = ImportExportFactory.CreateImporter(ExportFormat.Csv);
                var (accounts, categories, operations) = importer.Import("export.csv");

                foreach (var a in accounts) _unitOfWork.Accounts.Add(a);
                foreach (var c in categories) _unitOfWork.Categories.Add(c);
                foreach (var o in operations) _unitOfWork.Operations.Add(o);

                ShowSuccess("Данные импортированы из export.csv");
            }
            else if (choice == "5")
            {
                var importer = ImportExportFactory.CreateImporter(ExportFormat.Json);
                var (accounts, categories, operations) = importer.Import("export.json");

                foreach (var a in accounts) _unitOfWork.Accounts.Add(a);
                foreach (var c in categories) _unitOfWork.Categories.Add(c);
                foreach (var o in operations) _unitOfWork.Operations.Add(o);

                ShowSuccess("Данные импортированы из export.json");
            }
            else if (choice == "6")
            {
                var importer = ImportExportFactory.CreateImporter(ExportFormat.Yaml);
                var (accounts, categories, operations) = importer.Import("export.yaml");

                foreach (var a in accounts) _unitOfWork.Accounts.Add(a);
                foreach (var c in categories) _unitOfWork.Categories.Add(c);
                foreach (var o in operations) _unitOfWork.Operations.Add(o);

                ShowSuccess("Данные импортированы из export.yaml");
            }
        }

        static void RecalculateBalances()
        {
            _balanceService.RecalculateAllBalances();
            _unitOfWork.Commit();
            ShowSuccess("Балансы всех счетов пересчитаны!");
        }

        static void InitializeSampleData()
        {
            // Добавление тестовых данных для демонстрации
            var salaryCategory = new Category(TransactionType.Income, "Зарплата");
            var foodCategory = new Category(TransactionType.Expense, "Продукты");

            _unitOfWork.Categories.Add(salaryCategory);
            _unitOfWork.Categories.Add(foodCategory);

            var account = new BankAccount("Основной");
            _unitOfWork.Accounts.Add(account);

            _unitOfWork.Operations.Add(new Operation(
                TransactionType.Income,
                account.Id,
                100000m,
                DateTime.Now.AddDays(-30),
                salaryCategory.Id,
                "Зарплата"
            ));

            _unitOfWork.Commit();
        }

        static void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.ReadKey();
        }

        static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
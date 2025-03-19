using Interfaces;
using Models;
using System.Globalization;

namespace ImportExport
{
    public class CsvImporter : IDataImporter
    {
        public (List<BankAccount>, List<Category>, List<Operation>) Import(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            var accounts = ParseSection<BankAccount>(lines, "ACCOUNTS", ParseAccount);
            var categories = ParseSection<Category>(lines, "CATEGORIES", ParseCategory);
            var operations = ParseSection<Operation>(lines, "OPERATIONS", ParseOperation);

            return (accounts, categories, operations);
        }

        private List<T> ParseSection<T>(
            string[] lines,
            string sectionName,
            Func<string[], T> parser)
        {
            var sectionStart = Array.FindIndex(lines, l => l.StartsWith(sectionName));
            if (sectionStart == -1) return new List<T>();

            var end = Array.FindIndex(lines, sectionStart + 1, l => l.StartsWith("\n") || string.IsNullOrEmpty(l));
            if (end == -1) end = lines.Length;

            return lines
                .Skip(sectionStart + 2)
                .Take(end - sectionStart - 2)
                .Select(l => parser(l.Split(',')))
                .ToList();
        }

        private BankAccount ParseAccount(string[] parts) => new(parts[1])
        {
            Id = Guid.Parse(parts[0]),
            Balance = decimal.Parse(parts[2], CultureInfo.InvariantCulture)
        };

        private Category ParseCategory(string[] parts) => new(
            (TransactionType)int.Parse(parts[1]),
            parts[2]
        )
        {
            Id = Guid.Parse(parts[0])
        };

        private Operation ParseOperation(string[] parts) => new(
            (TransactionType)int.Parse(parts[1]),
            Guid.Parse(parts[2]),
            decimal.Parse(parts[3], CultureInfo.InvariantCulture),
            DateTime.Parse(parts[4], CultureInfo.InvariantCulture),
            Guid.Parse(parts[6]),
            string.IsNullOrEmpty(parts[5]) ? null : parts[5]
        )
        {
            Id = Guid.Parse(parts[0])
        };
    }
}
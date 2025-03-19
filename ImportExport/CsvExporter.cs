// Core/Services/ImportExport/Exporters/CsvExporter.cs
using Interfaces;
using Models;
using System.Globalization;
using System.Text;

namespace ImportExport
{
    public class CsvExporter : IDataExporter
    {
        public void Export(
            IEnumerable<BankAccount> accounts,
            IEnumerable<Category> categories,
            IEnumerable<Operation> operations,
            string filePath)
        {
            var csv = new StringBuilder();

            // Accounts section
            csv.AppendLine("ACCOUNTS");
            csv.AppendLine("Id,Name,Balance");
            foreach (var acc in accounts)
            {
                csv.AppendLine(
                    $"{acc.Id}," +
                    $"{EscapeCsv(acc.Name)}," +
                    $"{acc.Balance.ToString(CultureInfo.InvariantCulture)}"
                );
            }

            // Categories section
            csv.AppendLine("\nCATEGORIES");
            csv.AppendLine("Id,Type,Name");
            foreach (var cat in categories)
            {
                csv.AppendLine(
                    $"{cat.Id}," +
                    $"{(int)cat.Type}," +
                    $"{EscapeCsv(cat.Name)}"
                );
            }

            // Operations section
            csv.AppendLine("\nOPERATIONS");
            csv.AppendLine("Id,Type,BankAccountId,Amount,Date,Description,CategoryId");
            foreach (var op in operations)
            {
                csv.AppendLine(
                    $"{op.Id}," +
                    $"{(int)op.Type}," +
                    $"{op.BankAccountId}," +
                    $"{op.Amount.ToString(CultureInfo.InvariantCulture)}," +
                    $"{op.Date:O}," +
                    $"{EscapeCsv(op.Description ?? string.Empty)}," +
                    $"{op.CategoryId}"
                );
            }

            File.WriteAllText(filePath, csv.ToString());
        }

        private string EscapeCsv(string value) =>
            value.Contains(',') ? $"\"{value.Replace("\"", "\"\"")}\"" : value;
    }
}
using Interfaces;
using Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ImportExport
{
    public class JsonImporter : IDataImporter
    {
        public (List<BankAccount>, List<Category>, List<Operation>) Import(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var data = JsonConvert.DeserializeObject<ImportData>(json);

            return (data.Accounts, data.Categories, data.Operations);
        }

        private class ImportData
        {
            public List<BankAccount> Accounts { get; set; }
            public List<Category> Categories { get; set; }
            public List<Operation> Operations { get; set; }
        }
    }
}
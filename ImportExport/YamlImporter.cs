using YamlDotNet.Serialization;
using System.Collections.Generic;
using Interfaces;
using Models;

namespace ImportExport
{
    public class YamlImporter : IDataImporter
    {
        public (List<BankAccount>, List<Category>, List<Operation>) Import(string filePath)
        {
            var yaml = File.ReadAllText(filePath);
            var deserializer = new DeserializerBuilder().Build();
            var data = deserializer.Deserialize<ImportData>(yaml);

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
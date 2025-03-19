using YamlDotNet.Serialization;
using System.Collections.Generic;
using Interfaces;
using Models;

namespace ImportExport
{
    public class YamlExporter : IDataExporter
    {
        public void Export(
            IEnumerable<BankAccount> accounts,
            IEnumerable<Category> categories,
            IEnumerable<Operation> operations,
            string filePath)
        {
            var data = new
            {
                Accounts = accounts,
                Categories = categories,
                Operations = operations
            };

            var serializer = new SerializerBuilder()
                .WithNamingConvention(YamlDotNet.Serialization.NamingConventions.CamelCaseNamingConvention.Instance)
                .Build();

            File.WriteAllText(filePath, serializer.Serialize(data));
        }
    }
}
using Interfaces;
using Models;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace ImportExport
{
    public class JsonExporter : IDataExporter
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

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ"
            };

            File.WriteAllText(filePath, JsonConvert.SerializeObject(data, settings));
        }
    }
}
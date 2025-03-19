using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;


namespace ImportExport
{
    public static class ImportExportFactory
    {
        public static IDataExporter CreateExporter(ExportFormat format)
            => format switch
            {
                ExportFormat.Csv => new CsvExporter(),
                ExportFormat.Json => new JsonExporter(),
                ExportFormat.Yaml => new YamlExporter(),
                _ => throw new NotSupportedException()
            };

        public static IDataImporter CreateImporter(ExportFormat format)
            => format switch
            {
                ExportFormat.Csv => new CsvImporter(),
                ExportFormat.Json => new JsonImporter(),
                ExportFormat.Yaml => new YamlImporter(),
                _ => throw new NotSupportedException()
            };
    }
}

using Pokedex_Datlo.Infrastructure.Repositories;

namespace Pokedex_Datlo.Domain.Services
{
    public class CsvFileImporter : IFileImporterRepository
    {
        public bool FileExists(string fileName)
        {
            return File.Exists(fileName) && Path.GetExtension(fileName).Equals(".csv", StringComparison.OrdinalIgnoreCase);
        }

        public List<Dictionary<string, string>> Import(Stream fileStream)
        {
            using (var reader = new StreamReader(fileStream))
            {
                var importedData = new List<Dictionary<string, string>>();
                var headers = reader.ReadLine()?.Split(',');

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line?.Split(',');

                    if (values != null && values.Length == headers?.Length)
                    {
                        var rowData = new Dictionary<string, string>();

                        for (int i = 0; i < headers.Length; i++)
                        {
                            rowData[headers[i]] = values[i];
                        }

                        importedData.Add(rowData);
                    }
                }

                return importedData;
            }
        }
    }
}

using OfficeOpenXml;
using Pokedex_Datlo.Infrastructure.Repositories;

public class ExcelFileImporter : IFileImporterRepository
{
    public bool FileExists(string fileName)
    {
        return File.Exists(fileName) && Path.GetExtension(fileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase);
    }

    public List<Dictionary<string, string>> Import(Stream fileStream)
    {
        var importedData = new List<Dictionary<string, string>>();

        using (var package = new ExcelPackage(fileStream))
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var worksheet = package.Workbook.Worksheets.First();

            // Obtém o cabeçalho da planilha
            var headers = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column]
                .Select(cell => cell.Text)
                .ToList();

            // Itera sobre as linhas da planilha
            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var rowData = new Dictionary<string, string>();

                // Itera sobre as colunas da planilha
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    rowData[headers[col - 1]] = worksheet.Cells[row, col].Text;
                }

                importedData.Add(rowData);
            }
        }

        return importedData;
    }
}
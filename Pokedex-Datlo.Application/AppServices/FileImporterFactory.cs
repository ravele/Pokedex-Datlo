using Pokedex_Datlo.Domain.Enums;
using Pokedex_Datlo.Domain.Services;
using Pokedex_Datlo.Infrastructure.Repositories;

namespace Pokedex_Datlo.Application.AppServices
{
    public class FileImporterFactory : IFileImporterFactory
    {
        public IFileImporterRepository GetFileImporter(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Csv:
                    return new CsvFileImporter();

                case FileType.Excel:
                    return new ExcelFileImporter();

                default:
                    throw new ArgumentException("Tipo de arquivo não suportado.");
            }
        }
    }
}

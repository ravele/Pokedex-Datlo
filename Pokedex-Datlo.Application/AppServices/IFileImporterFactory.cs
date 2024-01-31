using Pokedex_Datlo.Domain.Enums;
using Pokedex_Datlo.Infrastructure.Repositories;

namespace Pokedex_Datlo.Application.AppServices
{
    public interface IFileImporterFactory
    {
        IFileImporterRepository GetFileImporter(FileType fileType);
    }
}

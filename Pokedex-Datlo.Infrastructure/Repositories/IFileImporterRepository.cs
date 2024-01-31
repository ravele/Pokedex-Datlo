namespace Pokedex_Datlo.Infrastructure.Repositories
{
    public interface IFileImporterRepository
    {
        bool FileExists(string fileName);
        List<Dictionary<string, string>> Import(Stream fileStream);
    }
}

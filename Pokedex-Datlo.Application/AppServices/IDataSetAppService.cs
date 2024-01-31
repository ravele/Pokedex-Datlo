using Pokedex_Datlo.Domain.Models;

namespace Pokedex_Datlo.Application.AppServices
{
    public interface IDataSetAppService
    {
        DataSet ImportDataSet(string fileName, Stream fileStream);
        List<Dictionary<string, string>> FilterDataSet(List<Filter> filters);
        List<Dictionary<string, string>> ImportIdsFromExcel(Stream fileStream);
    }
}

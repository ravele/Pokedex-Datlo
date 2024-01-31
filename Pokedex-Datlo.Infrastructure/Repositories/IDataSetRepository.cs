using Pokedex_Datlo.Domain.Models;

namespace Pokedex_Datlo.Infrastructure.Repositories
{
    public interface IDataSetRepository
    {
        DataSet GetDataSet();
        void Save(DataSet dataSet);
        List<Dictionary<string, string>> GetValuesForIds(List<Dictionary<string, string>> ids);
    }
}
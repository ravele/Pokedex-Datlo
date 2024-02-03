using Pokedex_Datlo.Domain.Models;
using System.Linq;

namespace Pokedex_Datlo.Infrastructure.Repositories
{
    public class DataSetRepository : IDataSetRepository
    {
        private readonly List<DataSet> _dataSets = new List<DataSet>();

        public DataSet GetDataSet()
        {
            return _dataSets.FirstOrDefault();
        }

        public void Save(DataSet dataSet)
        {
            if (!_dataSets.Any(ds => ds.Id == dataSet.Id))
            {
                _dataSets.Add(dataSet);
            }
            else
            {
                var existingDataSet = _dataSets.First(ds => ds.Id == dataSet.Id);
                _dataSets.Remove(existingDataSet);
                _dataSets.Add(dataSet);
            }
        }

        public List<Dictionary<string, string>> GetValuesForIds(List<Dictionary<string, string>> ids)
        {
            var idSet = new HashSet<string>(ids.Select(id => id["Id"]));
            
            var result = _dataSets
                .FirstOrDefault().Data
                .Where(item => idSet.Contains(item["Id"]))
            .ToList();

            return result;
        }
    }
}

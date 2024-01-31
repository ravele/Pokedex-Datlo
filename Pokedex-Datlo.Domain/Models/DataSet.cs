namespace Pokedex_Datlo.Domain.Models
{
    public class DataSet
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Column> Columns { get; set; }
        public List<Dictionary<string,string>> Data { get; set; }
    }
}

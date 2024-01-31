using Pokedex_Datlo.Domain.Enums;

namespace Pokedex_Datlo.Domain.Models
{
    public class Column
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DataType DataType { get; set; }
    }
}

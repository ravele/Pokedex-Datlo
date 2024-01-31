using Pokedex_Datlo.Application.Enums;

namespace Pokedex_Datlo.Domain.Models
{
    public class Filter
    {
        public Filter() { }

        public Filter(string columnName, string value, FilterOperator filterOperator)
        {
            ColumnName = columnName;
            Value = value;
            Operator = filterOperator;
        }

        public string ColumnName { get; set; }
        public string Value { get; set; }
        public FilterOperator Operator { get; set; }
    }
}

using Pokedex_Datlo.Application.Enums;
using Pokedex_Datlo.Domain.Enums;
using Pokedex_Datlo.Domain.Models;
using Pokedex_Datlo.Infrastructure.Repositories;

namespace Pokedex_Datlo.Application.AppServices
{
    public class DataSetAppService : IDataSetAppService
    {
        private readonly IDataSetRepository _dataSetRepository;
        private readonly IFileImporterRepository _fileImporterRepository;
        private readonly IFileImporterFactory _fileImporterFactory;

        public DataSetAppService(IDataSetRepository dataSetRepository, IFileImporterFactory fileImporterFactory)
        {
            _dataSetRepository = dataSetRepository;
            _fileImporterFactory = fileImporterFactory;
        }

        public DataSet ImportDataSet(string fileName, Stream fileStream)
        {
            // Verificar o tipo do arquivo.
            var fileType = DetermineFileType(fileName);

            // Obter o importador apropriado do factory.
            var fileImporter = _fileImporterFactory.GetFileImporter(fileType);

            // Importar os dados usando o importador apropriado.
            var importedData = fileImporter.Import(fileStream);

            // Criar um novo conjunto de dados com os dados importados.
            var dataSet = new DataSet
            {
                Id = GenerateDataSetId(),
                Name = Path.GetFileNameWithoutExtension(fileName),
                Columns = CreateColumns(importedData),
                Data = importedData
            };

            // Salvar
            _dataSetRepository.Save(dataSet);

            return dataSet;
        }

        public List<Dictionary<string, string>> FilterDataSet(List<Filter> filters)
        {
            // Recuperar o conjunto de dados pelo ID.
            var dataSet = _dataSetRepository.GetDataSet();

            if (dataSet == null)
            {
                throw new InvalidOperationException($"Conjunto de dados não encontrado.");
            }

            // Aplicar os filtros aos dados do conjunto de dados.
            var filteredData = ApplyFilters(dataSet.Columns, dataSet.Data, filters);

            return filteredData;
        }

        private FileType DetermineFileType(string fileName)
        {
            // Verifico a extensão do arquivo
            var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

            if (fileExtension == ".csv")
            {
                return FileType.Csv;
            }
            else if (fileExtension == ".xlsx")
            {
                return FileType.Excel;
            }
            else
            {
                throw new ArgumentException("Formato de arquivo não suportado.");
            }
        }

        private Guid GenerateDataSetId()
        {
            return Guid.NewGuid();
        }

        private List<Dictionary<string, string>> ApplyFilters(List<Column> columns, List<Dictionary<string, string>> data, List<Filter> filters)
        {
            var filteredData = new List<Dictionary<string, string>>();

            foreach (var row in data)
            {
                // Verificar se a linha atende a todos os filtros.
                if (RowMatchesFilters(row, columns, filters))
                {
                    filteredData.Add(row);
                }
            }

            return filteredData;
        }

        private bool RowMatchesFilters(Dictionary<string, string> row, List<Column> columns, List<Filter> filters)
        {
            foreach (var filter in filters)
            {
                var columnName = filter.ColumnName;
                var column = columns.FirstOrDefault(c => c.Name == columnName);

                if (column == null)
                {
                    // Lidar com a situação em que o nome da coluna do filtro não é encontrado.
                    throw new InvalidOperationException($"Coluna '{columnName}' não encontrada no conjunto de dados.");
                }

                var cellValue = row[columnName];

                // Comparar o valor da célula com o valor do filtro usando o operador apropriado.
                if (!CompareValues(cellValue, filter.Value, filter.Operator, column.DataType))
                {
                    return false; // A linha não atende a pelo menos um dos filtros.
                }
            }

            return true; // A linha atende a todos os filtros.
        }

        private bool CompareValues(string cellValue, string filterValue, FilterOperator filterOperator, DataType dataType)
        {
            // Lógica para comparar os valores com base no operador e no tipo de dados.
            // Neste exemplo, estamos fazendo comparações de igualdade para texto e números.

            switch (filterOperator)
            {
                case FilterOperator.Equal:
                    return cellValue.Equals(filterValue, StringComparison.OrdinalIgnoreCase);

                case FilterOperator.NotEqual:
                    return !cellValue.Equals(filterValue, StringComparison.OrdinalIgnoreCase);

                case FilterOperator.Contains:
                    return cellValue.Contains(filterValue, StringComparison.OrdinalIgnoreCase);

                // Outros casos conforme necessário.

                default:
                    throw new ArgumentException($"Operador de filtro '{filterOperator}' não suportado.");
            }
        }

        private List<Column> CreateColumns(List<Dictionary<string, string>> importedData)
        {
            var columns = new List<Column>();

            if (importedData.Count > 0)
            {
                var firstRow = importedData[0];

                foreach (var columnName in firstRow.Keys)
                {
                    // Inferir o tipo de dado da coluna com base nos dados do primeiro row
                    var dataType = InferDataType(firstRow[columnName]);

                    columns.Add(new Column
                    {
                        Name = columnName,
                        DataType = dataType,
                        // Outras propriedades e configurações necessárias.
                    });
                }
            }

            return columns;
        }

        private DataType InferDataType(string value)
        {
            // Lógica para inferir o tipo de dados com base no valor.
            // Neste exemplo, verifica se o valor pode ser convertido para número ou permanece como texto.

            if (int.TryParse(value, out _))
            {
                return DataType.Number;
            }
            else
            {
                return DataType.Text;
            }
        }

        public List<Dictionary<string, string>> ImportIdsFromExcel(Stream fileStream)
        {
            // Obter o importador apropriado do factory.
            var fileImporter = _fileImporterFactory.GetFileImporter(FileType.Excel);

            // Importar os dados usando o importador apropriado.
            var importedData = fileImporter.Import(fileStream);

            // Consulta do DataSet para obter os valores correspondentes
            var dataSetValues = _dataSetRepository.GetValuesForIds(importedData);

            return dataSetValues;
        }
    }
}

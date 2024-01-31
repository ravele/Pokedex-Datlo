using Microsoft.AspNetCore.Mvc;
using Pokedex_Datlo.Application.AppServices;
using Pokedex_Datlo.Application.DTOs;

namespace Pokedex_Datlo.Controllers
{
    [ApiController]
    [Route("api/dataset")]
    public class DataSetController : ControllerBase
    {
        private readonly IDataSetAppService _dataSetAppService;

        public DataSetController(IDataSetAppService dataSetAppService)
        {
            _dataSetAppService = dataSetAppService;
        }

        [HttpPost("import-dataset/csv")]
        public IActionResult ImportDataSetCsv(IFormFile file)
        {
            try
            {
                // Validar se o arquivo é CSV
                if (file.ContentType.ToLower() != "text/csv" || !file.FileName.EndsWith(".csv"))
                {
                    return BadRequest("Formato de arquivo inválido. Somente arquivos CSV são suportados.");
                }

                // Importar o conjunto de dados do arquivo CSV
                var dataSet = _dataSetAppService.ImportDataSet(file.FileName, file.OpenReadStream());

                return Ok(dataSet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro durante a importação do arquivo CSV: {ex.Message}");
            }
        }

        [HttpPost("import-dataset/excel")]
        public IActionResult ImportDataSetExcel(IFormFile file)
        {
            try
            {
                // Validar se o arquivo é Excel
                if (file.ContentType.ToLower() != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ||
                    !file.FileName.EndsWith(".xlsx"))
                {
                    return BadRequest("Formato de arquivo inválido. Somente arquivos Excel (.xlsx) são suportados.");
                }

                // Importar o conjunto de dados do arquivo Excel
                var dataSet = _dataSetAppService.ImportDataSet(file.FileName, file.OpenReadStream());

                return Ok(dataSet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro durante a importação do arquivo Excel: {ex.Message}");
            }
        }

        [HttpPost("filter")]
        public IActionResult FilterDataSet([FromBody] FilterRequestDTO request)
        {
            // Lógica para filtrar o conjunto de dados.
            var filteredData = _dataSetAppService.FilterDataSet(request.Filters);
            return Ok(filteredData);
        }

        [HttpPost("import-filter/excel")]
        public IActionResult ImportFilterExcel(IFormFile file)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var result = _dataSetAppService.ImportIdsFromExcel(stream);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro durante a importação do arquivo Excel: {ex.Message}");
            }
        }
    }
}

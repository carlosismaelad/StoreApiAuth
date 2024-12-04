using Microsoft.AspNetCore.Mvc;
using StoreApiAuth.Services;
using StoreApiAuth.src.Services;

namespace StoreApiAuth.src.Controllers
{
    [ApiController]
    [Route("api/v1/status/sqlserver")]
    public class SqlServerStatusController: ControllerBase
    {
        private readonly SqlServerStatusService _sqlServiceStatusService;
        private readonly IConfiguration _configuration;

        public SqlServerStatusController(SqlServerStatusService sqlServiceStatusService)
        {
            _sqlServiceStatusService = sqlServiceStatusService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStatus()
        {
            try
            {
                var databaseStatus = await _sqlServiceStatusService.GetDatabaseStatusAsync();

                var response = new
                {
                    dependencies = new
                    {
                        database = databaseStatus
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "Ocorreu um erro ao verificar o status do banco de dados.",
                    details = ex.Message
                });
            }
        }
    }
}
}

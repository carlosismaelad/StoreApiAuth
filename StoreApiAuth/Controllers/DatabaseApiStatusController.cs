using Microsoft.AspNetCore.Mvc;
using StoreApiAuth.Services;

namespace StoreApiAuth.Controllers
{
    [ApiController]
    [Route("api/v1/status")]
    public class DatabaseApiStatusController: ControllerBase
    {
        private readonly DatabaseStatusService _databaseStatusService;
        private readonly IConfiguration _configuration;

        public DatabaseApiStatusController(DatabaseStatusService databaseStatusService)
        {
            _databaseStatusService = databaseStatusService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStatus()
        {
            try
            {
                var databaseStatus = await _databaseStatusService.GetDatabaseStatusAsync();

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

﻿using Microsoft.AspNetCore.Mvc;
using StoreApiAuth.Services;

namespace StoreApiAuth.Controllers
{
    [ApiController]
    [Route("api/v1/status/postgres")]
    public class PostgresStatusController: ControllerBase
    {
        private readonly PostgresStatusService _postgresStatusService;
        private readonly IConfiguration _configuration;

        public PostgresStatusController(PostgresStatusService postgresStatusService)
        {
            _postgresStatusService = postgresStatusService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStatus()
        {
            try
            {
                var databaseStatus = await _postgresStatusService.GetDatabaseStatusAsync();

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

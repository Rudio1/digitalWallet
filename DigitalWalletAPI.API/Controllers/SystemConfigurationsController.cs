using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DigitalWalletAPI.Application.Interfaces;

namespace DigitalWalletAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SystemConfigurationsController : ControllerBase
    {
        private readonly ISystemConfigurationService _service;

        public SystemConfigurationsController(ISystemConfigurationService service)
        {
            _service = service;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> GetByKey(string key)
        {
            try
            {
                var value = await _service.GetValueAsync(key);
                return Ok(new { key, value });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 
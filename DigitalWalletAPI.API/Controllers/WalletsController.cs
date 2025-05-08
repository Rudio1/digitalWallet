using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DigitalWalletAPI.Application.DTOs;
using DigitalWalletAPI.Application.Services;
using DigitalWalletAPI.Domain.Exceptions;

namespace DigitalWalletAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletsController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<WalletDto>> GetByUserId(Guid userId)
        {
            try
            {
                var wallet = await _walletService.GetByUserIdAsync(userId);
                return Ok(wallet);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Carteira não encontrada" });
            }
        }

        [HttpGet("{walletId}/balance")]
        public async Task<ActionResult<decimal>> GetBalance(Guid walletId)
        {
            var balance = await _walletService.GetBalanceAsync(walletId);
            return Ok(balance);
        }

        [HttpPost("{walletId}/balance")]
        public async Task<ActionResult<WalletDto>> AddBalance(Guid walletId, [FromBody] AddBalanceDto addBalanceDto)
        {
            try
            {
                var wallet = await _walletService.AddBalanceAsync(walletId, addBalanceDto);
                return Ok(wallet);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Carteira não encontrada" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{walletId}/balance")]
        public async Task<IActionResult> UpdateBalance(Guid walletId, [FromBody] decimal newBalance)
        {
            try
            {
                await _walletService.UpdateBalanceAsync(walletId, newBalance);
                return NoContent();
            }
            catch (WalletNotFoundException)
            {
                return NotFound(new { message = "Carteira não encontrada" });
            }
            catch (InvalidWalletOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 
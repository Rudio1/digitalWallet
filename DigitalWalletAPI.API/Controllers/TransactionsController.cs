using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DigitalWalletAPI.Application.DTOs;
using DigitalWalletAPI.Application.Services;

namespace DigitalWalletAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("wallet/{senderWalletId}")]
        public async Task<ActionResult<TransactionDto>> Create(Guid senderWalletId, [FromBody] CreateTransactionDto createDto)
        {
            try
            {
                var transaction = await _transactionService.CreateAsync(senderWalletId, createDto);
                return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("wallet/{walletId}")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetByWalletId(Guid walletId, [FromQuery] TransactionFilterDto filterDto)
        {
            var transactions = await _transactionService.GetByWalletIdAsync(walletId, filterDto);
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetById(Guid id)
        {
            try
            {
                var transaction = await _transactionService.GetByIdAsync(id);
                return Ok(transaction);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Transação não encontrada" });
            }
        }
    }
} 
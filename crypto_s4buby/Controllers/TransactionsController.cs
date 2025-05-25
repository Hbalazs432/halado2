using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using crypto_s4buby.Context.Context;
using crypto_s4buby.Context.Entities;
using crypto_s4buby.Services;

namespace crypto_s4buby.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetTransactionsOfUser(int userId)
        {
            try
            {
                var result = await _transactionService.GetTransactionsOfUserAsync(userId);
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("details/{transactionId}")]
        public async Task<IActionResult> GetTransactionDetails(int transactionId)
        {
            try
            {
                var result = await _transactionService.GetTransactionDetailsAsync(transactionId);
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

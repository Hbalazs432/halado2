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
using crypto_s4buby.Context.Dtos;

namespace crypto_s4buby.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptosController : ControllerBase
    {
        private readonly ICryptoService _cryptoService;

        public CryptosController(ICryptoService cryptoService)
        {
            _cryptoService = cryptoService;
        }

        // GET: api/Cryptos
        [HttpGet]
        public async Task<IActionResult> GetAllCryptos()
        {
            try
            {
                var res = await _cryptoService.GetAllCryptosAsync();
                return Ok(res);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Cryptos/5
        [HttpGet("{cryptoId}")]
        public async Task<IActionResult> GetCrypto(int cryptoId)
        {
            try
            {
                var res = await _cryptoService.GetCryptoByIdAsync(cryptoId);
                return Ok(res);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Cryptos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> AddCrypto([FromBody]CryptoPostDto cryptoPostDto)
        {
            try
            {
                var res = await _cryptoService.AddCryptoAsync(cryptoPostDto);
                return Ok(res);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Cryptos/5
        [HttpDelete("{cryptoId}")]
        public async Task<IActionResult> DeleteCrypto(int cryptoId)
        {
            try
            {
                var res = await _cryptoService.DeleteCryptoAsync(cryptoId);
                return Ok();

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("price")]
        public async Task<IActionResult> UpdateExchangeRate([FromBody]CryptoUpdateDto cryptoUpdateDto)
        {
            try
            {
                var res = await _cryptoService.UpdateExchangeRate(cryptoUpdateDto);
                return Ok(res);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("price/history/{cryptoId}")]
        public async Task<IActionResult> GetCryptoHistory(int cryptoId)
        {
            try
            {
                var res = await _cryptoService.GetExchangeRateHistoryOfCrypto(cryptoId);
                return Ok(res);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

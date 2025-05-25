using AutoMapper;
using crypto_s4buby.Context.Context;
using crypto_s4buby.Context.Dtos;
using crypto_s4buby.Context.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crypto_s4buby.Services
{
    public interface ICryptoService
    {
        Task<IEnumerable<CryptoDto>> GetAllCryptosAsync();
        Task<CryptoDto> GetCryptoByIdAsync(int id);
        Task<CryptoDto> AddCryptoAsync(CryptoPostDto cryptoPostDto);
        Task<bool> DeleteCryptoAsync(int id);
        Task<CryptoDto> UpdateExchangeRate(CryptoUpdateDto cryptoUpdateDto);
        Task<IEnumerable<CryptoHistoryDto>> GetExchangeRateHistoryOfCrypto(int id);
    }
    public class CryptoService : ICryptoService
    {
        private readonly IMapper _mapper;
        private readonly CryptoDbContext _context;

        public CryptoService(IMapper mapper, CryptoDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<CryptoDto> AddCryptoAsync(CryptoPostDto cryptoPostDto)
        {
            var crypto = _mapper.Map<Crypto>(cryptoPostDto);
            await _context.Cryptos.AddAsync(crypto);
            await _context.SaveChangesAsync();

            return _mapper.Map<CryptoDto>(crypto);
        }

        public async Task<bool> DeleteCryptoAsync(int id)
        {
            var crypto = await _context.Cryptos.FindAsync(id);
            if (crypto == null) throw new KeyNotFoundException("No Crypto found with such ID.");

            _context.Cryptos.Remove(crypto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CryptoDto>> GetAllCryptosAsync()
        {
            var cryptos = await _context.Cryptos.ToListAsync();
            return _mapper.Map<List<CryptoDto>>(cryptos);
        }

        public async Task<CryptoDto> GetCryptoByIdAsync(int id)
        {
            var crypto = await _context.Cryptos.FindAsync(id);
            if (crypto == null) throw new KeyNotFoundException("No Crypto found with such ID.");

            return _mapper.Map<CryptoDto>(crypto);
        }

        public async Task<IEnumerable<CryptoHistoryDto>> GetExchangeRateHistoryOfCrypto(int id)
        {
            var crypto = await _context.Cryptos.FindAsync(id);
            if (crypto == null) throw new KeyNotFoundException("No crypto found with such ID.");

            var res =  await _context.CryptoHistories.Where(ch => ch.CryptoId == id).ToListAsync();

            return _mapper.Map<List<CryptoHistoryDto>>(res);
        }

        public async Task<CryptoDto> UpdateExchangeRate(CryptoUpdateDto cryptoUpdateDto)
        {
            var crypto = await _context.Cryptos.FindAsync(cryptoUpdateDto.Id);
            if (crypto == null) throw new KeyNotFoundException("No crypto found with such ID.");

            crypto.ExchangeRate = cryptoUpdateDto.ExchangeRate;
            _context.Update(crypto);

            var temp = new CryptoHistoryUpdateDto
            {
                CryptoId = cryptoUpdateDto.Id,
                ExchangeRate = cryptoUpdateDto.ExchangeRate
            };

            await _context.CryptoHistories.AddAsync(_mapper.Map<CryptoHistory>(temp));
            await _context.SaveChangesAsync();

            return _mapper.Map<CryptoDto>(crypto);
        }
    }
}

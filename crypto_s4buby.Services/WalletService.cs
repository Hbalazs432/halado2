using AutoMapper;
using crypto_s4buby.Context.Context;
using crypto_s4buby.Context.Dtos;
using crypto_s4buby.Context.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace crypto_s4buby.Services
{
    public interface IWalletService
    {
        public Task<WalletDto> DeleteWalletAsync(int id);
        public Task<WalletDto> SetBalanceAsync(int id, WalletUpdateDto walletUpdateDto);
        public Task<WalletDto> GetWalletAsync(int id);
        public Task<PortfolioDto> GetPortfolio(int id);
        public Task<double> GetProfit(int id);
        public Task<IEnumerable<ProfitDto>> GetProfitDetails(int id);

    }
    public class WalletService : IWalletService
    {
        private readonly CryptoDbContext _context;
        private readonly IMapper _mapper;

        public WalletService(CryptoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<WalletDto> DeleteWalletAsync(int id)
        {
            var user = await _context.Users.Include(u => u.Wallet).Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null) throw new KeyNotFoundException("No user found with such ID.");

            foreach(var item in user.Wallet.WalletItems)
            {
                _context.WalletItems.Remove(item);
            }
            user.Wallet.WalletItems = new List<WalletItem>();
            user.Wallet.Balance = 0.0;

            await _context.SaveChangesAsync();
            return _mapper.Map<WalletDto>(user.Wallet);
        }

        public async Task<PortfolioDto> GetPortfolio(int id)
        {
            var user = await _context.Users.Include(u => u.Wallet)
                .ThenInclude(w => w.WalletItems)
                .Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null) throw new KeyNotFoundException("No user found with such ID.");

            var items = user.Wallet.WalletItems.GroupBy(wi => wi.CryptoId).Select(group => new
            {
                Id = group.Key,
                Total = group.Sum(c => c.Quantity)
            });

            Dictionary<string, int> res = new Dictionary<string, int>();

            foreach(var item in items)
            {
                var crypto = _context.Cryptos.Find(item.Id);
                if(crypto != null)
                    res.Add(crypto.Name, item.Total);
            }

            return new PortfolioDto
            {
                Balance = user.Wallet.Balance,
                Cryptos = res
            };
        }

        public async Task<double> GetProfit(int id)
        {
            double profit = 0.0;

            var user = await _context.Users.Include(u => u.Wallet)
                .ThenInclude(w => w.WalletItems)
                .Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null) throw new KeyNotFoundException("No user found with such ID.");
            
            foreach(var item in user.Wallet.WalletItems)
            {
                profit += getProfitFromCrypto(item);
            }

            return profit;
        }

        private  double getProfitFromCrypto(WalletItem walletItem)
        {
            var crypto =  _context.Cryptos.Find(walletItem.CryptoId);
            if (crypto == null) return 0.0;
            return (crypto.ExchangeRate - walletItem.BoughtAtPrice) * walletItem.Quantity;
        }

        public async Task<IEnumerable<ProfitDto>> GetProfitDetails(int id)
        {
            var user = await _context.Users.Include(u => u.Wallet)
                .ThenInclude(w => w.WalletItems)
                .Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null) throw new KeyNotFoundException("No user found with such ID.");


            var res = user.Wallet.WalletItems.GroupBy(wi => wi.CryptoId)
                .ToDictionary(group => group.Key, group =>
                {
                    double profit = 0.0;

                    foreach (var item in group)
                        profit += getProfitFromCrypto(item);

                    return profit;
                });

            var result = new List<ProfitDto>();

            foreach (var item in res)
            {
                var temp = _context.Cryptos.Find(item.Key);
                if (temp != null)
                    result.Add(new ProfitDto
                    {
                        Crypto = temp.Name,
                        Profit = item.Value
                    });
            }
            return result;
        }

        public async Task<WalletDto> GetWalletAsync(int id)
        {
            var user = await _context.Users.Include(u => u.Wallet)
                .ThenInclude(w => w.WalletItems)
                .Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null) throw new KeyNotFoundException("No user found with such ID.");

            return _mapper.Map<WalletDto>(user.Wallet);
        }

        public async Task<WalletDto> SetBalanceAsync(int id, WalletUpdateDto walletUpdateDto)
        {
            var user = await _context.Users.Include(u => u.Wallet).Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null) throw new KeyNotFoundException("No user found with such ID.");

            user.Wallet.Balance = walletUpdateDto.Balance;
            await _context.SaveChangesAsync();
            return _mapper.Map<WalletDto>(user.Wallet);
        }
    }
}

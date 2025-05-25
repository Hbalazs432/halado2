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
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetTransactionsOfUserAsync(int id);
        Task<TransactionDetailDto> GetTransactionDetailsAsync(int id);
        Task<TransactionSellDto> Sell(TransactionSellDto transactionSellDto);
        Task<WalletItemDto> Buy(TransactionBuyDto transactionBuytDto);
    }
    public class TransactionService : ITransactionService
    {
        private readonly CryptoDbContext _context;
        private readonly IMapper _mapper;

        public TransactionService(CryptoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TransactionDetailDto> GetTransactionDetailsAsync(int id)
        {
            var transaction = await _context.Transactions.Include(t => t.Crypto)
                .Where(t => t.Id == id).FirstOrDefaultAsync();
            if (transaction == null) throw new KeyNotFoundException("No transaction with such ID.");

            return _mapper.Map<TransactionDetailDto>(transaction);
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsOfUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new KeyNotFoundException("No user with such ID.");

            var transactions = await _context.Transactions
                .Include(t => t.Crypto)
                .Where(t => t.UserId == id).OrderBy(t => t.Id).ToListAsync();
            return _mapper.Map<List<TransactionDto>>(transactions);
        }

        public async Task<TransactionSellDto> Sell(TransactionSellDto transactionSellDto)
        {
            var user = await _context.Users.Include(u => u.Wallet)
                .ThenInclude(w => w.WalletItems)
                .Where(u => u.Id == transactionSellDto.UserId).FirstOrDefaultAsync();
            if (user == null) throw new KeyNotFoundException("No user with such ID.");

            var crypto = await _context.Cryptos.FindAsync(transactionSellDto.CryptoId);
            if (crypto == null) throw new KeyNotFoundException("No crypto with such ID.");

            var totalCrypto = user.Wallet.WalletItems.GroupBy(wi => wi.CryptoId).Select(group => new
            {
                Id = group.Key,
                Total = group.Sum(c => c.Quantity)
            }).Where(wi => wi.Id == transactionSellDto.CryptoId).First();

            if (transactionSellDto.Quantity > totalCrypto.Total) throw new InvalidDataException("The user doesn't have enough cryptos in their wallet for this transaction.");

            var cryptos = user.Wallet.WalletItems.Where(wi => wi.CryptoId == transactionSellDto.CryptoId)
                .OrderBy(wi => wi.BoughtAtPrice);

            var remainingQuantity = transactionSellDto.Quantity;
            foreach(var c in cryptos)
            {
                if (c.Quantity < remainingQuantity)
                {
                    remainingQuantity -= c.Quantity;
                    user.Wallet.Balance += c.Quantity * crypto.ExchangeRate;
                    c.Quantity = 0;
                }
                else
                {
                    user.Wallet.Balance += remainingQuantity * crypto.ExchangeRate;
                    c.Quantity -= remainingQuantity;
                }
            }

            foreach(var c in cryptos)
            {
                if (c.Quantity == 0)
                    _context.WalletItems.Remove(c);
            }

            await _context.Transactions.AddAsync(_mapper.Map<Transaction>(transactionSellDto));

            await _context.SaveChangesAsync();

            return transactionSellDto;
        }

        public async Task<WalletItemDto> Buy(TransactionBuyDto transactionBuyDto)
        {
            var user = await _context.Users.Include(u => u.Wallet).Where(u => u.Id == transactionBuyDto.UserId).FirstOrDefaultAsync();
            if (user == null) throw new KeyNotFoundException("No user with such ID.");

            var crypto = await _context.Cryptos.FindAsync(transactionBuyDto.CryptoId);
            if (crypto == null) throw new KeyNotFoundException("No crypto with such ID.");

            if (crypto.AvailableQuantity < transactionBuyDto.Quantity) throw new InvalidDataException("Not enough crypto left for this exchange.");
            var sum = transactionBuyDto.Quantity * crypto.ExchangeRate;

            if (user.Wallet.Balance < sum) throw new InvalidDataException("The user doesn't have enough money on their account for this transaction.");

            var wallettitem = new WalletItemPostDto
            {
                CryptoId = transactionBuyDto.CryptoId,
                BoughtAtPrice = crypto.ExchangeRate,
                Quantity = transactionBuyDto.Quantity,
                Timestamp = DateTime.Now,
                WalletId = user.WalletId
            };

            user.Wallet.WalletItems.Add(_mapper.Map<WalletItem>(wallettitem));

            crypto.AvailableQuantity -= transactionBuyDto.Quantity;
            _context.Cryptos.Update(crypto);

            user.Wallet.Balance -= sum;
            _context.Wallets.Update(user.Wallet);

            await _context.WalletItems.AddAsync(_mapper.Map<WalletItem>(wallettitem));
            await _context.Transactions.AddAsync(_mapper.Map<Transaction>(transactionBuyDto));

            await _context.SaveChangesAsync();

            return _mapper.Map<WalletItemDto>(wallettitem);
        }
    }
}

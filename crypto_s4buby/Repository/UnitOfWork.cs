using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using System;
using crypto_s4buby.Entities;

namespace crypto_s4buby.Repository
{
    public class UnitOfWork
    {
        private CryptoDbContext _dbContext;

        public UnitOfWork(CryptoDbContext dbContext)
        {
            _dbContext = dbContext;
            UserRepository = new GenericRepository<User>(dbContext);
            WalletRepository = new GenericRepository<Wallet>(dbContext);
            CryptoRepository = new GenericRepository<Crypto>(dbContext);
            WalletItemRepository = new GenericRepository<WalletItem>(dbContext);
        }

        public GenericRepository<User> UserRepository { get; set; }
        public GenericRepository<Wallet> WalletRepository { get; set; }
        public GenericRepository<Crypto> CryptoRepository { get; set; }
        public GenericRepository<WalletItem> WalletItemRepository { get; set; }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

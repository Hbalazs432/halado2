using crypto_s4buby.Context.Entities;
using Microsoft.EntityFrameworkCore;

namespace crypto_s4buby.Context.Context
{
    public class CryptoDbContext : DbContext
    {
        public CryptoDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Crypto> Cryptos { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletItem> WalletItems { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<CryptoHistory> CryptoHistories { get; set; }
    }
}

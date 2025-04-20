using Microsoft.EntityFrameworkCore;

namespace crypto_s4buby.Entities
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>().HasOne(w => w.Owner)
                .WithOne(w => w.Wallet).HasForeignKey<User>(u => u.WalletId);
        }
    }
}

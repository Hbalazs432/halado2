using System.ComponentModel.DataAnnotations.Schema;

namespace crypto_s4buby.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public List<WalletItem> WalletItems { get; set; } = new List<WalletItem>();
        public double Balance { get; set; } = 1000.0;
    }
}

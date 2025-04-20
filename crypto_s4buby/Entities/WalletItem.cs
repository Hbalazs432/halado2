namespace crypto_s4buby.Entities
{
    public class WalletItem
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }
        public int CryptoId { get; set; }
        public Crypto Crypto { get; set; }
        public int Quantity { get; set; }
    }
}

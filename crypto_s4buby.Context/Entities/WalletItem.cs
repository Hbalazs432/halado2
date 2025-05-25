namespace crypto_s4buby.Context.Entities
{
    public class WalletItem
    {
        public int Id { get; set; }
        public int CryptoId { get; set; }
        public Crypto Crypto { get; set; }
        public int Quantity { get; set; }
        public double BoughtAtPrice { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

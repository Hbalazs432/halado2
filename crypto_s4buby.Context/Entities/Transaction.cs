namespace crypto_s4buby.Context.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CryptoId { get; set; }
        public Crypto Crypto { get; set; }
        public int Quantity { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Sold { get; set; }
    }
}

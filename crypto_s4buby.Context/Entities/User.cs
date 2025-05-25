namespace crypto_s4buby.Context.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }
    }
}

using crypto_s4buby.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crypto_s4buby.Context.Dtos
{
    public class WalletUpdateDto
    {
        public double Balance { get; set; }
    }

    public class WalletDto
    {
        public int Id { get; set; }
        public List<WalletItem> WalletItems { get; set; }
        public double Balance { get; set; }
    }

    public class PortfolioDto
    {
        public double Balance { get; set; }
        public Dictionary<string, int> Cryptos { get; set; } = new Dictionary<string,int>();
    }

    public class WalletItemPostDto
    {
        public int WalletId { get; set; }
        public int CryptoId { get; set; }
        public int Quantity { get; set; }
        public double BoughtAtPrice { get; set; }
        public DateTime Timestamp { get; set; }
    }
    public class WalletItemDto
    {
        public int Id { get; set; }
        public int CryptoId { get; set; }
        public string Crypto { get; set; }
        public int Quantity { get; set; }
        public double BoughtAtPrice { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class ProfitDto
    {
        public string Crypto { get; set; }
        public double Profit { get; set;}
    }
}

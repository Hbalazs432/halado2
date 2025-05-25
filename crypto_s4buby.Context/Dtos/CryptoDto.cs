using crypto_s4buby.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crypto_s4buby.Context.Dtos
{
    public class CryptoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double ExchangeRate { get; set; }
        public int AvailableQuantity { get; set; }
    }

    public class CryptoPostDto
    {   
        public string Name { get; set; }
        public double ExchangeRate { get; set; }
        public int AvailableQuantity { get; set; }
    }

    public class CryptoUpdateDto
    {
        public int Id { get; set; }
        public double ExchangeRate { get; set; }
    }

    public class CryptoHistoryDto
    {
        public int Id { get; set; }
        public int CryptoId { get; set; }
        public Crypto Crypto { get; set; }
        public double ExchangeRate { get; set; }
    }

    public class CryptoHistoryUpdateDto
    {
        public int CryptoId { get; set; }
        public double ExchangeRate { get; set; }
    }
}

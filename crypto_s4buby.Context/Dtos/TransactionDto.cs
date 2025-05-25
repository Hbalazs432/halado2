using crypto_s4buby.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crypto_s4buby.Context.Dtos
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public int CryptoId { get; set; }
        public string Crypto { get; set; }
        public string Action { get; set; }
    }

    public class TransactionDetailDto
    {
        public int Id { get; set; }
        public int CryptoId { get; set; }
        public string Crypto { get; set; }
        public int Quantity { get; set; }
        public DateTime Timestamp { get; set; }
        public double BoughtAtPrice { get; set; }
        public string Action { get; set; }
    }

    public class TransactionPostDto
    {
        public int UserId { get; set; }
        public int CryptoId { get; set; }
        public int Quantity { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Sold { get; set; }
    }

    public class TransactionBuyDto
    {
        public int UserId { get; set; }
        public int CryptoId { get; set; }
        public int Quantity { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class TransactionSellDto
    {
        public int UserId { get; set; }
        public int CryptoId { get; set; }
        public int Quantity { get; set; }
        public DateTime Timestamp { get; set; }
    }

}

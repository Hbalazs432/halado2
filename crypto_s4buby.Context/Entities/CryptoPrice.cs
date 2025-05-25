using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crypto_s4buby.Context.Entities
{
    public class CryptoPrice
    {
        public int Id { get; set; }
        public int CryptoId { get; set; }
        public Crypto Crypto { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
    }
}

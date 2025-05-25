using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crypto_s4buby.Context.Entities
{
    public class CryptoHistory
    {
        public int Id { get; set; }
        public int CryptoId { get; set; }
        public Crypto Crypto { get; set; }
        public double ExchangeRate { get; set; }
    }
}

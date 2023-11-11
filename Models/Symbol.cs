using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleTraderAssignment.Models
{
    public class Symbol
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public string Isin { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime DateAdded { get; set; }
        public float Price { get; set; }
        public DateTime PriceDate { get; set; }

        public Type Type { get; set; }
        public int TypeId { get; set; }

        public Exchange Exchange { get; set; }
        public int ExchangeId { get; set; }
    }
}

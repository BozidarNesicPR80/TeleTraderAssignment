using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleTraderAssignment.Models
{
    public class Symbol
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public string Isin { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime DateAdded { get; set; }
        public float Price { get; set; }
        public DateTime PriceDate { get; set; }

        [ForeignKey("TypeId")]
        public Type Type { get; set; }
        public int TypeId { get; set; }

        [ForeignKey("ExchangeId")]
        public Exchange Exchange { get; set; }
        public int ExchangeId { get; set; }
    }
}

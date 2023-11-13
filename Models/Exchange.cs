using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleTraderAssignment.Models
{
    public class Exchange
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

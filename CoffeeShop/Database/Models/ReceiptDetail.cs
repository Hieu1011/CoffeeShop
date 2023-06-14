using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CoffeeShop.Database.Models
{
    [Table("ReceiptDetail")]
    public class ReceiptDetail
    {
        [Key, Column(Order = 0), MaxLength(10)]
        public string ReceiptID { get; set; }

        [Key, Column(Order = 1), MaxLength(10)]
        public string BeverageID { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public int Price { get; set; }
    }
}

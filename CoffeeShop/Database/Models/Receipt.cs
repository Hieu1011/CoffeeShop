using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CoffeeShop.Database.Models
{
    [Table("Receipt")]
    public class Receipt
    {
        [Key, MaxLength(10)]
        public string ReceiptID { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required, MaxLength(10)]
        public string EmployeeID { get; set; }

        [MaxLength(10)]
        public string DiscountID { get; set; }
    }
}

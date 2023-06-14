using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Database.Models
{
    [Table("PaymentVoucher")]
    public class PaymentVoucher
    {
        [Key]
        [MaxLength(10)]
        public string PaymentID { get; set; }

        public float TotalAmount { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(10)]
        public string EmployeeID { get; set; }

        public string Time { get; set; }
    }
}

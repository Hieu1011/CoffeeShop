using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Database.Models
{
    [Table("Discount")]
    public class Discount
    {
        [Key, MaxLength(10)]
        public string DiscountID { get; set; }

        public string DiscountName { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public double DiscountValue { get; set; }

        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Database.Models
{
    [Table("InventoryImportDetail")]
    public class InventoryImportDetail
    {
        [Key, Column(Order = 0)]
        [MaxLength(10)]
        public string ImportID { get; set; }

        [Key, Column(Order = 1)]
        [MaxLength(10)]
        public string MaterialID { get; set; }

        [Required]
        public string Amount { get; set; }

        [Required]
        public string Price { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Database.Models
{
    [Table("InventoryExport")]
    public class InventoryExport
    {
        [Key]
        [MaxLength(10)]
        public string ExportID { get; set; }

        [MaxLength(10), Required]
        public string EmployeeID { get; set; }

        [MaxLength(10), Required]
        public string ExportDate { get; set; }

        [MaxLength(10)]
        public string Description { get; set; }
    }
}

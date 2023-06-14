using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Database.Models
{
    [Table("InventoryImport")]
    public class InventoryImport
    {
        [Key, MaxLength(10)]
        public string ImportID { get; set; }

        [Required, MaxLength(10)]
        public string EmployeeID { get; set; }

        [Required, MaxLength(20)]
        public string ImportDate { get; set; }
    }
}

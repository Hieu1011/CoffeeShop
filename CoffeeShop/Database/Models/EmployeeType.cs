using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Database.Models
{
    [Table("EmployeeType")]
    public class EmployeeType
    {
        [Key]
        [MaxLength(10)]
        public string EmployeeTypeID { get; set; }

        [MaxLength(50)]
        public string EmployeeTypeName { get; set; }
    }
}

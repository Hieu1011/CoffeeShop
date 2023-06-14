using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Database.Models
{
    [Table("Employees")]
    public class Employees
    {
        [Key]
        [MaxLength(10)]
        public string EmployeeID { get; set; }

        [MaxLength(100)]
        public string EmployeeName { get; set; }

        [MaxLength(10)]
        public string EmployeeTypeID { get; set; }

        [MaxLength(20)]
        public string Password { get; set; }

        public int State { get; set; }
    }
}

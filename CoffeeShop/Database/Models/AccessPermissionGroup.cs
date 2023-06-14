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
    [Table("AccessPermissionGroup")]
    public class AccessPermissionGroup
    {
        [Key, Column(Order = 0)]
        [MaxLength(10)]
        public string EmployeeTypeID { get; set; }

        [Key, Column(Order = 1)]
        [MaxLength(10)]
        public string AccessPermissionID { get; set; }
    }
}

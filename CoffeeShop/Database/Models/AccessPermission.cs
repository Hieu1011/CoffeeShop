using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Database.Models
{
    [Table("AccessPermission")]
    public class AccessPermission
    {
        [Key]
        [MaxLength(10)]
        public string AccessPermissionID { get; set; }

        [MaxLength(50)]
        public string AccessPermissionName { get; set; }
    }
}

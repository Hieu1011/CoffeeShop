using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Database.Models
{
    [Table("BeverageType")]
    public class BeverageType
    {
        [Key, MaxLength(5)]
        public string BeverageTypeID { get; set; }

        [MaxLength(50)]
        public string BeverageTypeName { get; set; }
    }
}

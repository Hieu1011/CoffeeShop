using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Database.Models
{
    [Table("BeverageName")]
    public class BeverageName
    {
        [Key, MaxLength(10)]
        public string BeverageID { get; set; }

        [MaxLength(10)]
        public string BeverageTypeID { get; set; }

        [Required, Column("BeverageName")]
        public string beverageName { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int IsOutOfStock { get; set; }

        [MaxLength(20)]
        public string Unit { get; set; }

        [DefaultValue("")]
        public byte[] Link { get; set; }
    }
}

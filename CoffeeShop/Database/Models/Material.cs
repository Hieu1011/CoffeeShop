using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Database.Models
{
    [Table("Material")]
    public class Material
    {
        [Key, MaxLength(10)]
        public string materialid { get; set; }

        [MaxLength(50)]
        public string materialname { get; set; }

        [MaxLength(20)]
        public string unit { get; set; }

        [MaxLength(1)]
        public string isUse { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DTO
{
    public class DTO_InventoryImportDetail
    {
        public string ImportID { get; set; }
        public string MaterialID { get; set; }
        public string Amount { get; set; }
        public string Price { get; set; }
    }
}

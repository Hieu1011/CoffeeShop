using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DTO
{
    public class DTO_InventoryExportDetail
    {
        public string ExportID { get; set; }
        public string MaterialID { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
    }
}

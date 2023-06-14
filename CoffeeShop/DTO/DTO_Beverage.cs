using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DTO
{
    public class DTO_Beverage
    {
        public string BeverageID { get; set; }
        public string BeverageTypeID { get; set; }
        public string BeverageName { get; set; }
        public int IsOutOfStock { get; set; }
        public int Price { get; set; }
        public string Unit { get; set; }
        public byte[] Link { get; set; }

    }
}

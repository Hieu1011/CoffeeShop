using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DTO
{
    public class DTO_Discount
    {
        public string DiscountID { get; set; }
        public string DiscountName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public float DiscountValue { get; set; }
        public string Description { get; set; }

        public DTO_Discount() { }

        public DTO_Discount(string id, string name, string start, string end, float value, string descrip)
        {
            DiscountID = id;
            DiscountName = name;
            StartDate = start;
            EndDate = end;
            DiscountValue = value;
            Description = descrip;
        }    

    }
}

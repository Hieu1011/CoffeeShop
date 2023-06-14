using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DTO
{
    public class DTO_Payment
    {
        public string PaymentID { get; set; }
        public string Time { get; set; }
        public string EmployeeID { get; set; }
        public float TotalAmount { get; set; }
        public string Description { get; set; }
    }
}

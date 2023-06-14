using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DTO
{
    public class DTO_Receipt
    {
        #region Atrributes
        public string ReceiptID { get; set; }
        public DateTime Time { get; set; }
        public string EmployeeID { get; set; }
        public string DiscountID { get; set; }
        #endregion

        #region Methods
        public DTO_Receipt() { }

        public DTO_Receipt(string id, DateTime createTime, string createEmp, string disID)
        {
            ReceiptID = id;
            Time = createTime;
            EmployeeID = createEmp;
            DiscountID = disID;
        }

        public DTO_Receipt(string id, string createEmp, string disID)
        {
            ReceiptID = id;
            EmployeeID = createEmp;
            DiscountID = disID;
        }

        public DTO_Receipt(string id, string createEmp)
        {
            ReceiptID = id;
            EmployeeID = createEmp;
            DiscountID = "";
        }
        #endregion
    }
}

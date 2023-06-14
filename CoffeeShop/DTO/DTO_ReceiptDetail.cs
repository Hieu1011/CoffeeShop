using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DTO
{
    class DTO_ReceiptDetail
    {
        #region Atrributes
        public string ReceiptID { get; set; }
        public string BeverageID { get; set; }
        public int Amount { get; set; }
        public int Price { get; set; }
        #endregion

        #region Methods
        public DTO_ReceiptDetail() { }

        public DTO_ReceiptDetail(string receiptID, string beverID, int amount, int price)
        {
            ReceiptID = receiptID;
            BeverageID = beverID;
            Amount = amount;
            Price = price;
        }    
        #endregion
    }
}

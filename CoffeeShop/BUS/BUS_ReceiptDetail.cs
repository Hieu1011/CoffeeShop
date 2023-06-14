using CoffeeShop.DAL;
using CoffeeShop.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.BUS
{
    class BUS_ReceiptDetail
    {
        DAL_ReceiptDetail dalReceiptDetail = new DAL_ReceiptDetail();
        public bool CreateReceiptDetail(DTO_ReceiptDetail newReceiptDetail)
        {
            return dalReceiptDetail.CreateReceiptDetail(newReceiptDetail);
        }

        public DataTable GetDetailByID(string id)
        {
            return dalReceiptDetail.GetDetailByID(id);
        }

        public bool DeleteDetailByID(string id)
        {
            return dalReceiptDetail.DeleteDetailByID(id);
        }    
    }
}

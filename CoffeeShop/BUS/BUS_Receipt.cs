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
    class BUS_Receipt
    {
        DAL_Receipt dalReceipt = new DAL_Receipt();
        public string CreateReceipt(DTO_Receipt newReceipt)
        {
            return dalReceipt.CreateReceipt(newReceipt);
        }

        public DataTable GetReceipts(int limit, int offset)
        {
            return dalReceipt.GetReceipt(limit, offset);
        }

        public DataTable GetReceipts(DateTime startDate, DateTime endDate, string keyword, int limit, int offset)
        {
            return dalReceipt.GetReceipt(startDate, endDate, keyword, limit, offset);
        }

        public int CountReceipt()
        {
            return dalReceipt.CountReceipt();
        }

        public int CountReceipt(DateTime startDate, DateTime endDate, string keyword)
        {
            return dalReceipt.CountReceipt(startDate, endDate, keyword);
        }

        public bool DeleteReceiptByID(string id)
        {
            return dalReceipt.DeleteReceiptByID(id);
        }

        public DataTable GetTotalIncomeByMonth(int month, int year)
        {
            return dalReceipt.GetTotalIncomeByMonth(month, year);
        }

        public DataTable GetTotalIncomeByYear(int year)
        {
            return dalReceipt.GetTotalIncomeByYear(year);
        }

        public DateTime GetCreateDayByID(string id)
        {
            return dalReceipt.GetCreateDayByID(id);
        }    
    }
}

using CoffeeShop.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.DTO;
namespace CoffeeShop.BUS
{
    public class BUS_Payment
    {
        DAL_Payment dalPayment = new DAL_Payment();
        public DataTable getAllPayment()
        {
            return dalPayment.getAllPayment();
        }

        public DataTable findPayment(string type, string name)
        {
            return dalPayment.findPayment(type, name);
        }
        public DataTable findPaymentbyID(string ID)
        {
            return dalPayment.findPaymentbyID(ID);
        }
        public int createNewPayment(DTO_Payment Payment)
        {
            return dalPayment.createNewPayment(Payment);
        }
        public int editPayment(DTO_Payment Payment)
        {
            return dalPayment.editPayment(Payment);
        }
        public int deletePayment(string ID)
        {
            return dalPayment.deletePayment(ID);
        }
        public string createID()
        {
            return dalPayment.createID();
        }
        public List<String> getEmployee()
        {
            return dalPayment.GetEmployee();
        }
        public string getEmployeeID(string Employeename)
        {
            return dalPayment.getEmployeeID(Employeename);
        }
        public DataTable GetTotalAmountByMonth(int month, int year)
        {
            return dalPayment.GetTotalAmountByMonth(month, year);
        }
        public DataTable GetTotalAmountByYear(int year)
        {
            return dalPayment.GetTotalAmountByYear(year);
        }
    }
}

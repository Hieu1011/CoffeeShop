using CoffeeShop.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.BUS
{
    public class BUS_InventoryImport
    {
        DAL_InventoryImport import = new DAL_InventoryImport();
        public int TotalCost(string id)
        {
            return import.TotalCost(id);
        }
        public DataTable SelectAllMaterialNameFromDetail(String id)
        {
            return import.SelectAllMaterialNameFromDetail(id);
        }
        public DataTable selectAll()
        {
            return import.SelectAllImport();
        }
        public string Create(String name, String date)
        {
            return import.Create(name,date);
        }
        public bool Delete(String id)
        {
            return import.Delete(id);
        }
        public bool Update(String id, String employId, String date)
        {
            return import.Update(id, employId, date);
        }
        public DataTable selectDetail(String id)
        {
            return import.SelectDetail(id);
        }
        public DataTable GetTotalAmountByMonth(int month, int year)
        {
            return import.GetTotalAmountByMonth(month, year);
        }
        public DataTable GetTotalAmountByYear(int year)
        {
            return import.GetTotalAmountByYear(year);
        }
    }
}

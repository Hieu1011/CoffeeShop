using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.DAL;
namespace CoffeeShop.BUS
{
    public class BUS_InventoryImportDetail
    {
        DAL_InventoryImportDetail temp = new DAL_InventoryImportDetail();
        public void Delete(String id)
        {
            temp.delete(id);
        }
        public void ImportList(List<String> sqlList)
        {
            temp.ImportList(sqlList);
        }
        public DataTable SelectAllImportDetail()
        {
            return temp.SelectAllImportDetail();
        }
        public DataTable SelectAllImportDetailGroupByName()
        {
            return temp.SelectAllImportDetailGroupByName();
        }
        public DataTable Find(String key)
        {
            return temp.FindWithKeyWord(key);
        }
    }
}

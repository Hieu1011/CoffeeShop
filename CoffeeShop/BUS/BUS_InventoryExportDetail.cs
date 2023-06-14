using CoffeeShop.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.BUS
{
    public class BUS_InventoryExportDetail
    {
        DAL_InventoryExportDetail temp = new DAL_InventoryExportDetail();
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
            return temp.SelectAllExportDetail();
        }
        public DataTable SelectAllExportDetailGroupByName()
        {
            return temp.SelectAllExportDetailGroupByName();
        }
        public DataTable Find(String key)
        {
            return temp.FindWithKeyWord(key);
        }
    }
}

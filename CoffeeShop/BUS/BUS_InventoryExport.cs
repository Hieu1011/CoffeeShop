using CoffeeShop.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.BUS
{
    public class BUS_InventoryExport
    {
        DAL_InventoryExport export = new DAL_InventoryExport();
        public DataTable  SelectAll()
        {
            return export.SelectAllExport();
        }
        public string Create(String name, String date,String reason)
        {
            return export.Create(name, date,reason);
        }
        public bool Delete(String id)
        {
            return export.Delete(id);
        }
        public DataTable SelectDetail(String id)
        {
            return export.SelectDetail(id);
        }
        public string SelectDescription(String id)
        {
            return export.SelectDescription(id);
        }
        public void updateDescription(String exportID, String value)
        {
            export.updateDescription(exportID, value);
        }
    }
}

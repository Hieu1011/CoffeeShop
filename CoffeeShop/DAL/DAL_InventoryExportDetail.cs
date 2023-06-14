using CoffeeShop.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DAL
{
    class DAL_InventoryExportDetail : DBConnect
    {
        public void delete(String exportID)
        {
            String sql = $"delete from InventoryExportDetail where exportID='{exportID}'";
            try
            {
                DataProvider.Instance.ExecuteNoneQuery(sql);
            }
            catch (Exception)
            {
            }
        }
        public void ImportList(List<String> sqlList)
        {
            foreach (String s in sqlList)
            {
                try
                {
                    DataProvider.Instance.ExecuteNoneQuery(s);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }


        }
        public DataTable SelectAllExportDetail()
        {
            try
            {
                string sql = $"select * from InventoryExportDetail";
                DataTable listDetail = DataProvider.Instance.ExecuteQuery(sql);
                return listDetail;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                return new DataTable();
            };
        }
        public DataTable SelectAllExportDetailGroupByName()
        {
            try
            {
                string sql = $" select materialname as 'Tên', sum(amount) as 'Số lượng' " +
                            $"from InventoryExportDetail detail Join Material mater " +
                            $"on detail.MaterialID= mater.MaterialID " +
                            $"group by materialname";
                DataTable listImportDetail = DataProvider.Instance.ExecuteQuery(sql);
                return listImportDetail;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                return new DataTable();
            };

        }
        public DataTable FindWithKeyWord(String key)
        {
            try
            {
                string sql = $" select materialname as 'Tên', sum(amount) as 'Số lượng' " +
                            $"from InventoryExportDetail detail Join Material mater " +
                            $"on detail.MaterialID= mater.MaterialID where materialName like '%{key}%' or unit like '%{key}%' " +
                            $"group by materialname";
                DataTable listImportDetail = DataProvider.Instance.ExecuteQuery(sql);
                return listImportDetail;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                return new DataTable();
            };

        }
        
    }
}

using CoffeeShop.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DAL
{
    class DAL_InventoryImportDetail : DBConnect
    {
        public void delete(String importID)
        {    
            String sql = $"delete from InventoryImportDetail where importID='{importID}'";
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
            foreach(String s in sqlList)
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
        public DataTable SelectAllImportDetail()
        {
            try
            {
                string sql = $"select * from InventoryImportDetail";
                DataTable listImportDetail = DataProvider.Instance.ExecuteQuery(sql);
                return listImportDetail;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                return new DataTable();
            };

        }
        public DataTable SelectAllImportDetailGroupByName()
        {
            try
            {
                string sql = $" select isUse ,materialname as 'Tên',unit as 'Đơn vị tính', sum(CAST(amount AS int)) as 'Số lượng' " +
                            $"from InventoryImportDetail detail Join Material mater " +
                            $"on detail.MaterialID= mater.MaterialID " +
                            $"group by isUse ,materialname, unit";
                return DataProvider.Instance.ExecuteQuery(sql);
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
                string sql = $" select materialname as 'Tên',unit as 'Đơn vị tính', sum(amount) as 'Số lượng' " +
                            $"from InventoryImportDetail detail Join Material mater " +
                            $"on detail.MaterialID= mater.MaterialID where materialName like '%{key}%' or unit like '%{key}%' " +
                            $"group by materialname, unit ";
                return DataProvider.Instance.ExecuteQuery(sql);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                return new DataTable();
            };

        }
        /*public void Create(String employID)
        {
            //create auto increase ID
            //Get max MaterialID
            String id = "";
            string tempSQL = "SELECT importID FROM InventoryImport order by importID desc LIMIT 1 ";
            SQLiteDataAdapter da = new SQLiteDataAdapter(tempSQL, getConnection());
            DataTable maxId = new DataTable();
            da.Fill(maxId);
            foreach (DataRow row in maxId.Rows)
            {
                id = row["MaterialID"].ToString();
            }
            //auto increase ID
            string newID = "Imp" +
                (Convert.ToInt32(id.Replace("Mater", "")) + 1)
                    .ToString()
                    .PadLeft(7, '0');
            //insert SQLite 
            string sql = $"insert into InventoryImport('ImportID','EmployeeID','ImportDate') VALUES ('{newID}','{employID}','{DateTime.Now.ToString()}');";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                insert.ExecuteNoneQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool Delete(String id)
        {
            string sql = $"delete from inventoryImport where ImportID='{id}'";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                return insert.ExecuteNoneQuery() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Update(String id, String employID, String date)
        {
            string sql = $"update inventoryImport set employeeID='{employID}', ImportDate = '{date}'  where importID='{id}'";
            SQLiteCommand update = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                return update.ExecuteNoneQuery() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public DataTable SelectDetail(String id)
        {
            try
            {
                string sql = $"select materialname as 'Tên',amount as 'Số lượng',price as 'Đơn giá',unit as 'Đơn vị tính' from InventoryImportDetail detail Join Material mater on detail.MaterialID= mater.MaterialID  where importID='{id}'";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable listImport = new DataTable();
                da.Fill(listImport);
                return listImport;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                return new DataTable();
            };
        }*/
    }
}

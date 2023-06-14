using CoffeeShop.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DAL
{
    class DAL_InventoryExport : DBConnect
    {
        public bool Delete(String id)
        {
            string sql = $"delete from inventoryExport where ExportID='{id}'";
            try
            {
                return DataProvider.Instance.ExecuteNoneQuery(sql) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public DataTable SelectAllExport()
        {
            try
            {
                string sql = $"select employeename, exportid, exportdate from InventoryExport Imp Join Employees employ on employ.EmployeeID=Imp.EmployeeID";
                DataTable listExport = DataProvider.Instance.ExecuteQuery(sql);
                return listExport;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                return new DataTable();
            };

        }
        public DataTable SelectDetail(String id)
        {
            try
            {
                string sql = $"select detail.MaterialID,unit,materialname as 'Tên',amount as 'Số lượng' from InventoryExportDetail detail Join Material mater on detail.MaterialID= mater.MaterialID where exportID='{id}'";
                DataTable listImport = DataProvider.Instance.ExecuteQuery(sql);
                return listImport;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                return new DataTable();
            };
        }
        public string Create(String name, String date, String description)
        {
            //create auto increase ID
            //Get max MaterialID
            String id = "Exp0000000";
            string tempSQL = "SELECT top 1 exportID FROM InventoryExport order by exportID desc";
            DataTable maxId = DataProvider.Instance.ExecuteQuery(tempSQL);
            foreach (DataRow row in maxId.Rows)
            {
                id = row["exportID"].ToString();
            }
            //auto increase ID
            string newID = "Exp" +
                (Convert.ToInt32(id.Replace("Exp", "")) + 1)
                    .ToString()
                    .PadLeft(7, '0');
            //get employid from name 
            String employId = "";
            string tempSQL1 = $"select employeeid from Employees where employeename= N'{name}' ";
            DataTable employid = DataProvider.Instance.ExecuteQuery(tempSQL1);
            foreach (DataRow row in employid.Rows)
            {
                employId = row["EmployeeID"].ToString();
            }
            //insert SQLServer
            string sql = $"insert into InventoryExport(ExportID, EmployeeID, ExportDate, Description) VALUES ('{newID}','{employId}','{date}', N'{description}');";
            try
            {
                DataProvider.Instance.ExecuteNoneQuery(sql); return newID;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }
        public string SelectDescription(String id)
        {
            try
            {
                string sql = $"select Description from InventoryExport where exportID='{id}'";
                DataTable listImport = DataProvider.Instance.ExecuteQuery(sql);
                foreach (DataRow row in listImport.Rows)
                {
                    return row["Description"].ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                
            };
            return null;
        }
        public void updateDescription(String exportID, String value)
        {
            String sql = $"update InventoryExport set description='{value}' where exportid='{exportID}'";
            try
            {
                DataProvider.Instance.ExecuteNoneQuery(sql);
            }
            catch (Exception)
            {
            }
        }
    }
}

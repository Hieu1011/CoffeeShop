using CoffeeShop.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DAL
{
    public class DAL_InventoryImport : DBConnect
    {
        public DataTable SelectAllImport()
        {
            try
            {
                string sql = $"select employeename, importid, importdate from InventoryImport Imp Join Employees employ on employ.EmployeeID=Imp.EmployeeID ";
                
                DataTable listImport = DataProvider.Instance.ExecuteQuery(sql);
                return listImport;
            }
            catch (Exception e)
            {
                Console.WriteLine( "INVENTORYIMPORT SELECT: Exception AT" + e.ToString());
                return new DataTable();
            };            
        }

        public int TotalCost(String id)
        {
            int total = 0;
            try
            {
                string sql = $"select amount,price  from InventoryImportDetail detail " +
                                 $"Join Material mater on detail.MaterialID= mater.MaterialID  where importID='{id}'";
                DataTable list = DataProvider.Instance.ExecuteQuery(sql);
                foreach (DataRow row in list.Rows)
                {
                    string amount = row["amount"].ToString();
                    string unitPrice = row["price"].ToString();
                    total += int.Parse(amount)*int.Parse(unitPrice);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
            };
            return total;
        }
        public string Create(String name, String date)
        {
            //create auto increase ID
            //Get max MaterialID
            String id = "Imp0000000";
            string tempSQL = "SELECT top 1 importID FROM InventoryImport order by importID desc";
            DataTable maxId = DataProvider.Instance.ExecuteQuery(tempSQL);
            foreach (DataRow row in maxId.Rows)
            {
                id = row["importID"].ToString();
            }
            //auto increase ID
            string newID = "Imp" +
                (Convert.ToInt32(id.Replace("Imp", "")) + 1)
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
            string sql = $"insert into InventoryImport(ImportID, EmployeeID, ImportDate) VALUES ('{newID}','{employId}','{date}');";
            
            try
            {
                DataProvider.Instance.ExecuteNoneQuery(sql);
                return newID;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool Delete(String id)
        {
            string sql = $"delete from inventoryImport where ImportID='{id}'";
            
            try
            {
                return DataProvider.Instance.ExecuteNoneQuery(sql) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Update(String id, String employID, String date)
        {
            string sql = $"update inventoryImport set employeeID='{employID}', ImportDate = '{date}'  where importID='{id}'";
            
            try
            {
                return DataProvider.Instance.ExecuteNoneQuery(sql) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public DataTable SelectDetail(String id)
        {
            try
            {
                string sql = $"select detail.materialid as 'ID' ,materialname as 'Tên',amount as 'Số lượng',price as 'Đơn giá',unit as 'Đơn vị tính' from InventoryImportDetail detail Join Material mater on detail.MaterialID= mater.MaterialID  where importID='{id}'";
                
                DataTable listImport = DataProvider.Instance.ExecuteQuery(sql);
                return listImport;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                return new DataTable();
            };

        }
        public DataTable SelectAllMaterialNameFromDetail(String id)
        {
            try
            {
                string sql = $"select materialname, impDetail.Amount from Material mater JOIN InventoryImportDetail impDetail on mater.materialid=impDetail.MaterialID " +
                    $"JOIN InventoryImport imp on imp.ImportID= impDetail.ImportID where imp.ImportID='{id}'";
                DataTable listImport = DataProvider.Instance.ExecuteQuery(sql);
                return listImport;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                return new DataTable();
            };
        }

        public DataTable GetTotalAmountByYear(int year)
        {
            DataTable data = new DataTable();
            //string sql = $"select substr(ImportDate, 4, 2) as Month, sum(Amount * Price) as TotalAmount from InventoryImport join InventoryImportDetail on InventoryImport.ImportID = InventoryImportDetail.ImportID where ImportDate like '%/%/{year}' group by Month";
            string sql = $"SELECT SUBSTRING(ImportDate, 4, 2) AS Month, sum(cast(Amount as int) * cast(Price as int)) as TotalAmount from InventoryImport join InventoryImportDetail on InventoryImport.ImportID = InventoryImportDetail.ImportID where ImportDate like '%/%/{year}' group by SUBSTRING(ImportDate, 4, 2)";
            try
            {
                data = DataProvider.Instance.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return data;
        }

        public DataTable GetTotalAmountByMonth(int month, int year)
        {
            DataTable data = new DataTable();
            string sql = $"select SUBSTRING(ImportDate, 1, 2) as Day, sum(Amount * Price) as TotalAmount from InventoryImport join InventoryImportDetail on InventoryImport.ImportID = InventoryImportDetail.ImportID where ImportDate like '%/{month.ToString().PadLeft(2, '0')}/{year}' group by SUBSTRING(ImportDate, 1, 2)";
            try
            {
                data = DataProvider.Instance.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return data;
        }
    }
}

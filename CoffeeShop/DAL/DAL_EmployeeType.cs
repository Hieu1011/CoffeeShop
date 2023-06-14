using CoffeeShop.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DAL
{
    public class DAL_EmployeeType : DBConnect
    {
        public DataTable GetEmployeeTypes()
        {
            DataTable empTypes = new DataTable();
            try
            {
                string sql = $"select * from EmployeeType";
                empTypes = DataProvider.Instance.ExecuteQuery(sql);
            }
            catch
            {

            }
            return empTypes;
        }

        public int CountEmployeeTypes()
        {
            DataTable empTypes = new DataTable();
            try
            {
                string sql = $"select count(EmployeeTypeID) from EmployeeType";
                empTypes = DataProvider.Instance.ExecuteQuery(sql);
            }
            catch
            {

            }
            return Int32.Parse(empTypes.Rows[0].ItemArray[0].ToString());
        }

        public string GetNameByID(string id)
        {
            string name = "";
            DataTable empTypeName = new DataTable();
            try
            {
                string sql = $"select EmployeeTypeName from EmployeeType where EmployeeTypeID = '{id}'";
                empTypeName = DataProvider.Instance.ExecuteQuery(sql);
                name = empTypeName.Rows[0].ItemArray[0].ToString();
            }
            catch
            {

            }
            return name;
        }

        public string GetIDByName(string name)
        {
            string id = "";
            DataTable empTypeName = new DataTable();
            try
            {
                string sql = $"select EmployeeTypeID from EmployeeType where EmployeeTypeName = N'{name}'";
                empTypeName = DataProvider.Instance.ExecuteQuery(sql);
                id = empTypeName.Rows[0].ItemArray[0].ToString();
            }
            catch
            {

            }
            return id;
        }

        public string CreateEmployeeType(DTO_EmployeeType newEmpType)
        {
            DataTable employeeType = GetEmployeeTypes();
            string lastID = employeeType.Rows[employeeType.Rows.Count - 1]["EmployeeTypeID"].ToString();
            newEmpType.EmployeeTypeID = "ET" +
                (Convert.ToInt32(lastID.Replace("ET", "")) + 1)
                    .ToString()
                    .PadLeft(3, '0');

            //insert SQLite
            string sql = $"insert into EmployeeType(EmployeeTypeID,EmployeeTypeName) VALUES ('{newEmpType.EmployeeTypeID}',N'{newEmpType.EmployeeTypeName}')";
            
            try
            {
                DataProvider.Instance.ExecuteNoneQuery(sql);
                return newEmpType.EmployeeTypeID;
            }
            catch (Exception )
            {
                return "";
            }
        }

        public int Delete(string typeID)
        {
            bool isDelete = IsHaveEmployee(typeID);
            if (isDelete)
            {
                string sql = $"delete from EmployeeType where EmployeeTypeID = '{typeID}'";
                
                try
                {
                    DataProvider.Instance.ExecuteNoneQuery(sql);
                    return 1;
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }    

        public bool IsHaveEmployee(string typeID)
        {
            try
            {
                DataTable countData = new DataTable();
                string sql = $"select count(EmployeeTypeID) from Employees where EmployeeTypeID = '{typeID}'";
                countData = DataProvider.Instance.ExecuteQuery(sql);
                if (countData.Rows[0].ItemArray[0].ToString() != "0")
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }    

        public bool EditEmployeeType(DTO_EmployeeType editEmpType)
        {
            string sql = $"update EmployeeType set EmployeeTypeName = N'{editEmpType.EmployeeTypeName}' where EmployeeTypeID = '{editEmpType.EmployeeTypeID}'";
            
            try
            {
                DataProvider.Instance.ExecuteNoneQuery(sql);
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }    
    }
}

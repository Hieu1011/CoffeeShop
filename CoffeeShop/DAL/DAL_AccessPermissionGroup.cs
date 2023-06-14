using CoffeeShop.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DAL
{
    class DAL_AccessPermissionGroup : DBConnect
    {
        public bool CreateAccessPermissionGroup(DTO_AccessPermissionGroup newAccPerGr)
        {
            //insert SQLite 
            string sql = $"insert into AccessPermissionGroup(EmployeeTypeID,AccessPermissionID) VALUES ('{newAccPerGr.EmployeeTypeID}','{newAccPerGr.AccessPermissionID}')";
            
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

        public bool DeleteAccessPermissionGroup(DTO_AccessPermissionGroup deleteAccPerGr)
        {
            string sql = $"delete from AccessPermissionGroup where AccessPermissionID = '{deleteAccPerGr.AccessPermissionID}' and EmployeeTypeID = '{deleteAccPerGr.EmployeeTypeID}'";
            
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

        public bool DeleteByEmpTypeID(string id)
        {
            string sql = $"delete from AccessPermissionGroup where EmployeeTypeID = '{id}'";
            try
            {
                DataProvider.Instance.ExecuteNoneQuery(sql);
                return true;
            }
            catch
            {
                return false;
            }
        }    

        public bool isHavePermission(string typeId, string permissionID)
        {
            DataTable data = new DataTable();
            try
            {
                string sql = $"select count(EmployeeTypeID) from AccessPermissionGroup where AccessPermissionID = '{permissionID}' and EmployeeTypeID = '{typeId}'";
                data = DataProvider.Instance.ExecuteQuery(sql);
                if (data.Rows[0].ItemArray[0].ToString() == "1")
                    return true;
            }
            catch
            {

            }
            return false;
        }    
    }
}

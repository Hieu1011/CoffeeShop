using CoffeeShop.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DAL
{
    class DAL_AccessPermission : DBConnect
    {
        public DataTable GetAccessInfo(int limit, int offset)
        {
            DataTable accessData = new DataTable(); //datatable from database
            DataTable perListData = new DataTable();
            DataTable accessInfo = new DataTable(); //datatable for showwing
            DataColumn dtColumn;
            try
            {
                // create columns of datatable for show
                string sql = $"select AccessPermissionName from AccessPermission";
                perListData = DataProvider.Instance.ExecuteQuery(sql);

                // create rows of datatable for show
                //sql = $"select * from EmployeeType limit {limit} offset {offset}";
                sql = $"select top {limit} * from EmployeeType";
                accessInfo = DataProvider.Instance.ExecuteQuery(sql);

                for (int i = 0; i < perListData.Rows.Count; i++)
                {
                    dtColumn = new DataColumn();
                    dtColumn.DataType = Type.GetType("System.String");
                    dtColumn.Caption = dtColumn.ColumnName = perListData.Rows[i].ItemArray[0].ToString();
                    accessInfo.Columns.Add(dtColumn);
                }

                for (int i = 0; i < accessInfo.Rows.Count; i++)
                    for (int j = 0; j < perListData.Rows.Count; j++)
                        accessInfo.Rows[i][perListData.Rows[j].ItemArray[0].ToString()] = "0";

                sql = $"select EmployeeType.EmployeeTypeName, AccessPermission.AccessPermissionName from EmployeeType join AccessPermissionGroup on EmployeeType.EmployeeTypeID = AccessPermissionGroup.EmployeeTypeID join AccessPermission on AccessPermissionGroup.AccessPermissionID = AccessPermission.AccessPermissionID";
                accessData = DataProvider.Instance.ExecuteQuery(sql);

                foreach (DataRow row in accessData.Rows)
                {
                    for (int i = 0; i < accessInfo.Rows.Count; i++)
                    {
                        if (row.ItemArray[0].ToString() == accessInfo.Rows[i].ItemArray[1].ToString())
                        {
                            accessInfo.Rows[i][columnName: row.ItemArray[1].ToString()] = "1";
                            break;
                        }
                    }
                }
            }
            catch
            {

            }
            return accessInfo;
        }

        public DataTable GetAccessInfoByEmployeeTypeName(string EmployeeTypeName)
        {
            DataTable accessData = new DataTable(); //datatable from database
            DataTable perListData = new DataTable();
            DataTable accessInfo = new DataTable(); //datatable for showwing
            DataColumn dtColumn;
            try
            {
                // create columns of datatable for show
                string sql = $"select AccessPermissionName from AccessPermission";
                perListData = DataProvider.Instance.ExecuteQuery(sql);

                // create rows of datatable for show
                //sql = $"select * from EmployeeType limit {limit} offset {offset}";
                sql = $"select top 1 * from EmployeeType";
                accessInfo = DataProvider.Instance.ExecuteQuery(sql);

                for (int i = 0; i < perListData.Rows.Count; i++)
                {
                    dtColumn = new DataColumn();
                    dtColumn.DataType = Type.GetType("System.String");
                    dtColumn.Caption = dtColumn.ColumnName = perListData.Rows[i].ItemArray[0].ToString();
                    accessInfo.Columns.Add(dtColumn);
                }

                for (int i = 0; i < accessInfo.Rows.Count; i++)
                    for (int j = 0; j < perListData.Rows.Count; j++)
                        accessInfo.Rows[i][perListData.Rows[j].ItemArray[0].ToString()] = "0";

                sql = $"select EmployeeType.EmployeeTypeName, AccessPermission.AccessPermissionName from EmployeeType join AccessPermissionGroup on EmployeeType.EmployeeTypeID = AccessPermissionGroup.EmployeeTypeID join AccessPermission on AccessPermissionGroup.AccessPermissionID = AccessPermission.AccessPermissionID WHERE EmployeeType.EmployeeTypeName = '{EmployeeTypeName}' ";
                accessData = DataProvider.Instance.ExecuteQuery(sql);

                foreach (DataRow row in accessData.Rows)
                {
                    for (int i = 0; i < accessInfo.Rows.Count; i++)
                    {
                        if (row.ItemArray[0].ToString() == accessInfo.Rows[i].ItemArray[1].ToString())
                        {
                            accessInfo.Rows[i][columnName: row.ItemArray[1].ToString()] = "1";
                            break;
                        }
                    }
                }
            }
            catch
            {

            }
            return accessInfo;
        }

        public DataTable GetAccessInfoByEmployeeTypeID(string EmployeeTypeID)
        {
            DataTable accessData = new DataTable(); //datatable from database
            DataTable perListData = new DataTable();
            DataTable accessInfo = new DataTable(); //datatable for showwing
            DataColumn dtColumn;
            try
            {
                // create columns of datatable for show
                string sql = $"select AccessPermissionName from AccessPermission";
                perListData = DataProvider.Instance.ExecuteQuery(sql);

                // create rows of datatable for show
                //sql = $"select * from EmployeeType limit {limit} offset {offset}";
                sql = $"select top 1 * from EmployeeType";
                accessInfo = DataProvider.Instance.ExecuteQuery(sql);

                for (int i = 0; i < perListData.Rows.Count; i++)
                {
                    dtColumn = new DataColumn();
                    dtColumn.DataType = Type.GetType("System.String");
                    dtColumn.Caption = dtColumn.ColumnName = perListData.Rows[i].ItemArray[0].ToString();
                    accessInfo.Columns.Add(dtColumn);
                }

                for (int i = 0; i < accessInfo.Rows.Count; i++)
                    for (int j = 0; j < perListData.Rows.Count; j++)
                        accessInfo.Rows[i][perListData.Rows[j].ItemArray[0].ToString()] = "0";

                sql = $"select EmployeeType.EmployeeTypeName, AccessPermission.AccessPermissionName from EmployeeType join AccessPermissionGroup on EmployeeType.EmployeeTypeID = AccessPermissionGroup.EmployeeTypeID join AccessPermission on AccessPermissionGroup.AccessPermissionID = AccessPermission.AccessPermissionID WHERE EmployeeType.EmployeeTypeID = '{EmployeeTypeID}' ";
                accessData = DataProvider.Instance.ExecuteQuery(sql);

                foreach (DataRow row in accessData.Rows)
                {
                    for (int i = 0; i < accessInfo.Rows.Count; i++)
                    {
                        if (row.ItemArray[0].ToString() == accessInfo.Rows[i].ItemArray[1].ToString())
                        {
                            accessInfo.Rows[i][columnName: row.ItemArray[1].ToString()] = "1";
                            break;
                        }
                    }
                }
            }
            catch
            {

            }
            return accessInfo;
        }

        public DataTable GetAccessPermissions()
        {
            DataTable accPers = new DataTable();

            try
            {
                string sql = $"select * from AccessPermission";
                accPers = DataProvider.Instance.ExecuteQuery(sql);
            }
            catch
            {

            }

            return accPers;
        }    

        public string GetIDByName(string name)
        {
            string id = "";
            DataTable empTypeName = new DataTable();
            try
            {
                string sql = $"select AccessPermissionID from AccessPermission where AccessPermissionName = N'{name}'";
                empTypeName = DataProvider.Instance.ExecuteQuery(sql);
                id = empTypeName.Rows[0].ItemArray[0].ToString();
            }
            catch
            {

            }
            return id;
        }    
    }
}

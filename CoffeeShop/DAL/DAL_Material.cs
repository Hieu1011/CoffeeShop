using CoffeeShop.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DAL
{
    public class DAL_Material : DBConnect
    {
       
        public DataTable SelectByName(List <String > name)
        {
            try
            {
                if (name == null) return null;
                String selectedName = $"(";
                for (int i = 0; i < name.Count - 1; i++)
                {
                    selectedName += $"N'{name[i]}',";
                }
                selectedName += $"N'{name[name.Count - 1]}')";
                string sql = $"SELECT * FROM Material where materialname IN {selectedName}";
                DataTable listMaterial = DataProvider.Instance.ExecuteQuery(sql);
                Console.WriteLine($"SELECT BY NAME ROW {listMaterial.Rows.Count}");
                return listMaterial;
            }
            catch (Exception)
            {

            };
            return null;
        }
        public DataTable SelectAllMaterial()
        {
            try
            {
                string sql = $"select * from Material";
                DataTable listMaterial = DataProvider.Instance.ExecuteQuery(sql);
                return listMaterial;
            }
            catch (Exception)
            {
            };
            return null;
        }
        public void UpdateUsing(string name, string unit)
        {
            string sql = $"UPDATE Material SET isuse = '1', unit='{unit}' WHERE materialName= N'{name}'";
            try
            {
                DataProvider.Instance.ExecuteNoneQuery(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool Create(String name, String unit)
        {
            //checking if material is already exist
            string checkMaterial = $"SELECT * FROM Material where MaterialName= N'{name}' ";
            DataTable checkMater = DataProvider.Instance.ExecuteQuery(checkMaterial);
            if (checkMater.Rows.Count!=0)
            {
                foreach (DataRow row in checkMater.Rows)
                {
                    string isUse = row["isUse"].ToString();
                    if(isUse== "1")
                        return false;
                }
                UpdateUsing(name,unit);
                return true;

            }               
            //create auto increase ID
            //Get max MaterialID
            String id = "Mater00000";
            string tempSQL = "SELECT top 1 materialId FROM Material order by materialId desc ";
            DataTable maxId = DataProvider.Instance.ExecuteQuery(tempSQL);
            foreach (DataRow row in maxId.Rows)
            {
                id = row["MaterialID"].ToString();
            }
            //auto increase ID
            string newID = "Mater" +
                (Convert.ToInt32(id.Replace("Mater", "")) + 1)
                    .ToString()
                    .PadLeft(5, '0');
            //insert SQLite 
            string sql = $"insert into Material VALUES ('{newID}', N'{name}',N'{unit}','1');";
            try
            {
                DataProvider.Instance.ExecuteNoneQuery(sql); return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
            
        }
        public bool Delete (String name)
        {
            string sql = $"UPDATE Material SET isuse = '0' WHERE materialName= '{name}'";
            try
            {
                return DataProvider.Instance.ExecuteNoneQuery(sql) > 0;                
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Update (String oldName,String newName, String unit)
        {
            string sql = $"update Material set MaterialName='{newName}', Unit = '{unit}'  where MaterialName= N'{oldName}'";
            try
            {
                return DataProvider.Instance.ExecuteNoneQuery(sql) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

}

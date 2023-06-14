using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.DTO;

namespace CoffeeShop.DAL
{
    class DAL_BeverageType: DBConnect
    {
        public List<DTO_BeverageType> GetBeverageType()
        {
            List<DTO_BeverageType> BeverageType = new List<DTO_BeverageType>();
            string sql = $"Select BeverageTypeName, BeverageTypeID From BeverageType";
            try
            {
                DataTable dt = DataProvider.Instance.ExecuteQuery(sql);

                foreach (DataRow row in dt.Rows)
                {
                    DTO_BeverageType dto = new DTO_BeverageType();
                    dto.BeverageTypeID = row["BeverageTypeID"].ToString();
                    dto.BeverageTypeName = row["BeverageTypeName"].ToString();
                    BeverageType.Add(dto);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
            return BeverageType;
        }
        public int createNewBeverageType(DTO_BeverageType beveragetype)
        {
            int rs = 0;
            string sql = $"Insert into BeverageType values ('" + beveragetype.BeverageTypeID + "','" + beveragetype.BeverageTypeName + "')";
            try
            {
                rs = DataProvider.Instance.ExecuteNoneQuery(sql);
                rs = 1;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return rs;
        }
        public int deleteBeverageType(string id)
        {
            int rs = 0;
            string sql = $"Delete From BeverageType Where BeverageTypeID='" + id + "'";
            if (checkConditionToDelete(id))
                try
                {
                    rs = DataProvider.Instance.ExecuteNoneQuery(sql);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }
            return rs;
        }
        public int editBeverageType(DTO_BeverageType beveragetype)
        {
            int rs = 0;
            Console.WriteLine(beveragetype.BeverageTypeID);
            string sql = $"Update BeverageType set BeverageTypeName='" + beveragetype.BeverageTypeName + "' Where BeverageTypeID='" + beveragetype.BeverageTypeID + "'";
            try
            {
                
                rs = DataProvider.Instance.ExecuteNoneQuery(sql);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return rs;
        }
        //public List<String> 
        public string createID()
        {
            string ID = "BT";
            int count = 0;
            int max = 0;
            string sql = "Select BeverageTypeID from BeverageType";
            try
            {
                DataTable dt = DataProvider.Instance.ExecuteQuery(sql);
                max = Int32.Parse(dt.Rows[dt.Rows.Count - 1]["BeverageTypeID"].ToString().Remove(0, 2));
                max++;
                ID = ID + max.ToString();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("BT: "+ e.Message);
            }
            return ID;
        }
        public List<DTO_BeverageType> findBeverageType(string type)
        {
            List<DTO_BeverageType> BeverageType = new List<DTO_BeverageType>();
            string sql = $"Select BeverageTypeName, BeverageTypeID From BeverageType Where BeverageTypeName like'%" + type + "%'";
            try
            {

                DataTable dt = DataProvider.Instance.ExecuteQuery(sql);
                foreach (DataRow row in dt.Rows)
                {
                    DTO_BeverageType dto = new DTO_BeverageType();
                    dto.BeverageTypeID = row["BeverageTypeID"].ToString();
                    dto.BeverageTypeName = row["BeverageTypeName"].ToString();
                    BeverageType.Add(dto);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Find: "+e.Message);
            }
            return BeverageType;
           
        }
        public bool checkConditionToDelete(string ID)
        {
            bool result = true;
            try
            {
                 string sql = $"Select BeverageTypeID From Beverage Where BeverageTypeID='" + ID + "'";
            //    SQLiteCommand cmd = new SQLiteCommand(sql, getConnection());
            //    SQLiteDataReader reader = cmd.ExecuteReader();
            //    if (reader.Read())
            //        result = false;

                DataTable dt = DataProvider.Instance.ExecuteQuery(sql);
                if (dt.Rows.Count > 0) return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            };
            return result;
        }
    }
}

using CoffeeShop.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.DAL
{
    class DAL_Parameter : DBConnect
    {
        public int GetValue(string valueName)
        {
            int rows = 0;
            try
            {
                DataTable value = new DataTable();
                string sql = $"Select Value From Parameter Where Name = '{valueName}'";
                value = DataProvider.Instance.ExecuteQuery(sql);
                rows = Int32.Parse(value.Rows[0].ItemArray[0].ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return rows;
        }

        public bool SetValue(string valueName, int value)
        {
            string sql = $"update Parameter set Value = {value} Where Name = '{valueName}'";
            try
            {
                DataProvider.Instance.ExecuteNoneQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}

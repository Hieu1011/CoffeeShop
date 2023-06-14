using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data.SQLite;

namespace CoffeeShop.DTO
{
    public class DBConnect
    {
        string connectionStr;
        public DBConnect()
        {
            connectionStr = "Data Source=CoffeeShop.db";
        }
        //public SQLiteConnection getConnection()
        //{
        //    return new SQLiteConnection(connectionStr);
        //}
    }
}

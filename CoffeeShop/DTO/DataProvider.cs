using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Database;

namespace CoffeeShop.DTO
{
    public class DataProvider
    {
        private static DataProvider instance;
        public static DataProvider Instance
        {
            get
            {
                if (instance == null) instance = new DataProvider(); return instance;
            }
            private set { instance = value; }
        }

        private DataProvider() { }

        private string connectionSTR = DBConfig.GetConnectionString();

        /// <summary>
        /// Hàm thực hiện lệnh truyền vào và trả về bảng dữ liệu
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();

            using (var db = DBContext.CreateInstance())
            {
                var connection = db.Database.Connection;
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = query;

                if (parameter != null)
                {
                    string[] listParameter = query.Split(' ');

                    int i = 0;

                    foreach (var item in listParameter)
                    {
                        if (item.Contains('@'))
                        {
                            var commandParam = command.CreateParameter();
                            commandParam.ParameterName = item;
                            commandParam.Value = parameter[i];
                            command.Parameters.Add(commandParam);
                            i++;
                        }
                    }
                }

                var reader = command.ExecuteReader();
                data.Load(reader);
                connection.Close();
            }

            return data;
        }

        public DataTable ExecuteQueryWithParam(string query, IDictionary<string, dynamic> paramList = null)
        {
            DataTable data = new DataTable();

            using (var db = DBContext.CreateInstance())
            {
                var connection = db.Database.Connection;
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = query;

                if (paramList != null)
                {
                    foreach (var param in paramList)
                    {
                        var commandParam = command.CreateParameter();
                        commandParam.ParameterName = param.Key;
                        commandParam.Value = param.Value;
                        command.Parameters.Add(commandParam);
                    }
                }

                var reader = command.ExecuteReader();
                data.Load(reader);
                connection.Close();
            }

            return data;
        }

        /// <summary>
        /// Hàm trả ra số dòng thành công, vd các lệnh: Update , Insert, Delete, ...
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public int ExecuteNoneQuery(string query, object[] parameter = null)
        {
            int data = 0;

            using (var db = DBContext.CreateInstance())
            {
                // Hàm trả ra số dòng thành công, vd các lệnh: Update , Insert, Delete, ...
                var connection = db.Database.Connection;
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = query;

                if (parameter != null)
                {
                    string[] listParameter = query.Split(' ');

                    int i = 0;

                    foreach (var item in listParameter)
                    {
                        if (item.Contains('@'))
                        {
                            var commandParam = command.CreateParameter();
                            commandParam.ParameterName = item;
                            commandParam.Value = parameter[i];
                            command.Parameters.Add(commandParam);
                            i++;
                        }
                    }
                }


                data = command.ExecuteNonQuery();

                connection.Close();
            }

            return data;
        }


        public int ExecuteNonQueryWithParam(string query, IDictionary<string, dynamic> paramList = null)
        {
            int data = 0;

            using (var db = DBContext.CreateInstance())
            {
                var connection = db.Database.Connection;
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = query;

                if (paramList != null)
                {
                    foreach (var param in paramList)
                    {
                        var commandParam = command.CreateParameter();
                        commandParam.ParameterName = param.Key;
                        commandParam.Value = param.Value;
                        command.Parameters.Add(commandParam);
                    }
                }

                data = command.ExecuteNonQuery();
                connection.Close();
            }

            return data;
        }

        /// <summary>
        /// Hàm thực hiện đếm số lượng (trả về ô đầu tiên của kết quả) , vd: SELECT Count(*) FROM ABC
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public object? ExecuteScalar(string query, object[] parameter = null)
        {
            // Hàm thực hiện đếm số lượng (trả về ô đầu tiên của kết quả) , vd: SELECT Count(*) FROM ABC

            object? data = 0;

            using (var db = DBContext.CreateInstance())
            {
                // Hàm trả ra số dòng thành công, vd các lệnh: Update , Insert, Delete, ...
                var connection = db.Database.Connection;
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = query;

                if (parameter != null)
                {
                    string[] listParameter = query.Split(' ');

                    int i = 0;

                    foreach (var item in listParameter)
                    {
                        if (item.Contains('@'))
                        {
                            var commandParam = command.CreateParameter();
                            commandParam.ParameterName = item;
                            commandParam.Value = parameter[i];
                            command.Parameters.Add(commandParam);
                            i++;
                        }
                    }
                }


                data = command.ExecuteScalar();

                connection.Close();
            }

            return data;
        }
    }
}

using MartinCostello.SqlLocalDb;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Database
{
    internal class DBConfig
    {
        private static string dbInstanceName = "test1";
        private static string dataFolderpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string GetConnectionString()
        {
            return String.Format("Data Source=(LocalDb)\\{0};AttachDbFilename={1};", dbInstanceName, GetDBFilePath());
        }

        public static string GetDBFilePath() {
            string basePath = Directory.GetCurrentDirectory();
            string databasePath = Path.Combine(basePath, "..", "..", "..", "..", "Database");
            string relativePath = Path.Combine(databasePath, "CoffeeShopDB.mdf");

            // Kiểm tra xem thư mục "database" đã tồn tại hay chưa
            if (!Directory.Exists(databasePath)) {
                Directory.CreateDirectory(databasePath);
            }

            return Path.GetFullPath(relativePath);
        }


        public static string GetDbLogFilePath()
        {
            return Path.GetFullPath("./CoffeeShopDB_log.ldf");
        }

        public static string GetInitSQLFilePath()
        {
            return Path.GetFullPath("./CoffeeShopDB.mdf.sql");
        }
        public static string GetInitBeverageFilePath()
        {
            return Path.GetFullPath("./BeverageName.sql");
        }

        public static bool EnsureSqlLocalDb(bool needRestart = false)
        {
            ISqlLocalDbApi localDB = new SqlLocalDbApi();

            if (!localDB.IsLocalDBInstalled())
            {
                return false;
            }
            else
            {
                if (!localDB.InstanceExists(dbInstanceName))
                {
                    localDB.CreateInstance(dbInstanceName, localDB.LatestVersion);
                }

                ISqlLocalDbInstanceInfo instance = localDB.GetInstanceInfo(dbInstanceName);
                ISqlLocalDbInstanceManager manager = instance.Manage();
                if (!instance.IsRunning)
                {
                    manager.Start();
                }
                else if(needRestart)
                {
                    manager.Restart();
                }
            }

            var fileFolderPath = Path.GetFullPath(Path.Combine(GetDBFilePath(), ".."));
            if (!Directory.Exists(fileFolderPath))
            {
                Directory.CreateDirectory(fileFolderPath);
            }

            return true;
        }

        public static void Release()
        {
            ISqlLocalDbApi localDB = new SqlLocalDbApi();

            if (localDB.IsLocalDBInstalled() && localDB.InstanceExists(dbInstanceName))
            {
                ISqlLocalDbInstanceInfo instance = localDB.GetInstanceInfo(dbInstanceName);
                ISqlLocalDbInstanceManager manager = instance.Manage();
                manager.Stop();
            }
        }

        public static void InitDB(DbContext context)
        {
            var db = context.Database;
            File.Delete(GetDbLogFilePath());
            db.Create();
            using (var transaction = db.BeginTransaction())
            {
                foreach (var sql in File.ReadLines(GetInitSQLFilePath(), Encoding.UTF8))
                {
                    db.ExecuteSqlCommand(sql);
                }
                foreach (var sql in File.ReadLines(GetInitBeverageFilePath(), Encoding.UTF8))
                {
                    db.ExecuteSqlCommand(sql);
                }
                transaction.Commit();
            }
        }

        public static void InitDBWithSampleData(DbContext context)
        {
            var db = context.Database;
            File.Delete(GetDbLogFilePath());
            db.Create();
            using (var transaction = db.BeginTransaction())
            {
                foreach (var sql in File.ReadLines(GetInitSQLFilePath() + ".sample", Encoding.UTF8))
                {
                    db.ExecuteSqlCommand(sql);
                }
                foreach (var sql in File.ReadLines(GetInitBeverageFilePath() + ".sample", Encoding.UTF8))
                {
                    db.ExecuteSqlCommand(sql);
                }
                transaction.Commit();
            }
        }
    }
}

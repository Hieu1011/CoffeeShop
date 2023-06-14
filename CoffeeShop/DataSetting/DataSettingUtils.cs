using CoffeeShop.Database.Models;
using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using CoffeeShop.Database;
using System.Linq;
using System.Data.Entity;

namespace CoffeeShop.DataSetting
{
    public class DataSettingUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns>Size in bytes</returns>
        public static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += FileSize(fi);
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns>Size in bytes</returns>
        public static long FileSize(FileInfo d)
        {
            if (File.Exists(d.FullName))
            {
                return d.Length;
            }
            return 0;
        }

        public static double GetAppSizeInKB()
        {
            var appDir = new DirectoryInfo(Directory.GetCurrentDirectory());
            var sizeInbytes = DirSize(appDir);
            return ConvertBytesToKBs(sizeInbytes);
        }

        public static double GetFileSizeInKB(string path)
        {
            var sizeInbytes = FileSize(new FileInfo(path));
            return ConvertBytesToKBs(sizeInbytes);
        }

        public static double ConvertBytesToKBs(double bytes)
        {
            return Math.Floor(bytes / 1024);
        }

        public static string PickFolder()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ValidateNames = false;
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.FileName = "Folder Selection.";
            if (dialog.ShowDialog() == true)
            {
                string? folderPath = Path.GetDirectoryName(dialog.FileName);
                if (folderPath != null)
                {
                    return folderPath;
                }
            }
            return "";
        }

        public static dynamic toDynamic<T>(T obj)
        {
            var rs = new ExpandoObject();
            foreach (var prop in obj.GetType().GetProperties())
            {
                var x = new ExpandoObject();
                (rs as IDictionary<string, Object?>).Add(prop.Name, prop.GetValue(obj, null));
            }
            return rs;
        }

        public static CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            ShouldQuote = args => true,
            Quote = '"',
        };

        public static int ImportFromCSV<T>(string path, DbSet dbSet)
        {
            int count = 0;
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, csvConfiguration))
            {
                foreach (var row in csv.GetRecords<T>())
                {
                    count++;
                    dbSet.Add(row);
                }
            }
            return count;
        }

        public static void WriteToCSV<T>(string path, IEnumerable<T> dataList)
        {
            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, csvConfiguration))
            {
                csv.WriteHeader<T>();
                foreach (var item in dataList)
                {
                    csv.NextRecord();
                    csv.WriteRecord(item);
                }
            }
        }

        public static void ResetData()
        {
            if (DBConfig.EnsureSqlLocalDb(true))
            {
                File.Delete(DBConfig.GetDBFilePath());
                using (var context = DBContext.CreateInstance())
                {
                    DBConfig.InitDB(context);
                }
            }
        }

        public static void ImportData(string folderPath)
        {
            if (DBConfig.EnsureSqlLocalDb(true))
            {
                File.Copy(DBConfig.GetDBFilePath(), DBConfig.GetDBFilePath() + ".backup");
                File.Copy(DBConfig.GetDbLogFilePath(), DBConfig.GetDbLogFilePath() + ".backup");
                File.Delete(DBConfig.GetDBFilePath());
                File.Delete(DBConfig.GetDbLogFilePath());

                using (var db = DBContext.CreateInstance())
                {
                    try
                    {
                        db.Database.Create();
                        ImportFromCSV<AccessPermission>(Path.Combine(folderPath, "AccessPermission.csv"), db.AccessPermission);
                        ImportFromCSV<AccessPermissionGroup>(Path.Combine(folderPath, "AccessPermissionGroup.csv"), db.AccessPermissionGroup);
                        ImportFromCSV<BeverageName>(Path.Combine(folderPath, "BeverageName.csv"), db.BeverageName);
                        ImportFromCSV<BeverageType>(Path.Combine(folderPath, "BeverageType.csv"), db.BeverageType);
                        ImportFromCSV<Database.Models.Discount>(Path.Combine(folderPath, "Discount.csv"), db.Discount);
                        ImportFromCSV<Employees>(Path.Combine(folderPath, "Employees.csv"), db.Employees);
                        ImportFromCSV<EmployeeType>(Path.Combine(folderPath, "EmployeeType.csv"), db.EmployeeType);
                        ImportFromCSV<InventoryExport>(Path.Combine(folderPath, "InventoryExport.csv"), db.InventoryExport);
                        ImportFromCSV<InventoryExportDetail>(Path.Combine(folderPath, "InventoryExportDetail.csv"), db.InventoryExportDetail);
                        ImportFromCSV<InventoryImport>(Path.Combine(folderPath, "InventoryImport.csv"), db.InventoryImport);
                        ImportFromCSV<InventoryImportDetail>(Path.Combine(folderPath, "InventoryImportDetail.csv"), db.InventoryImportDetail);
                        ImportFromCSV<Material>(Path.Combine(folderPath, "Material.csv"), db.Material);
                        ImportFromCSV<Parameter>(Path.Combine(folderPath, "Parameter.csv"), db.Parameter);
                        ImportFromCSV<PaymentVoucher>(Path.Combine(folderPath, "PaymentVoucher.csv"), db.PaymentVoucher);
                        ImportFromCSV<Receipt>(Path.Combine(folderPath, "Receipt.csv"), db.Receipt);
                        ImportFromCSV<ReceiptDetail>(Path.Combine(folderPath, "ReceiptDetail.csv"), db.ReceiptDetail);
                        var count = db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        if (DBConfig.EnsureSqlLocalDb(true))
                        {
                            File.Delete(DBConfig.GetDBFilePath());
                            File.Delete(DBConfig.GetDbLogFilePath());
                            File.Copy(DBConfig.GetDBFilePath() + ".backup", DBConfig.GetDBFilePath());
                            File.Copy(DBConfig.GetDbLogFilePath() + ".backup", DBConfig.GetDbLogFilePath());
                        }

                        throw e;
                    }
                    finally
                    {
                        File.Delete(DBConfig.GetDBFilePath() + ".backup");
                        File.Delete(DBConfig.GetDbLogFilePath() + ".backup");
                    }
                }

            }
        }

        public static void ExportData(string folderPath)
        {
            using (var db = DBContext.CreateInstance())
            {
                DataSettingUtils.WriteToCSV(Path.Combine(folderPath, "AccessPermission.csv"), db.AccessPermission.ToList());
                DataSettingUtils.WriteToCSV(Path.Combine(folderPath, "AccessPermissionGroup.csv"), db.AccessPermissionGroup.ToList());
                DataSettingUtils.WriteToCSV(Path.Combine(folderPath, "BeverageName.csv"), db.BeverageName.ToList());
                DataSettingUtils.WriteToCSV(Path.Combine(folderPath, "BeverageType.csv"), db.BeverageType.ToList());
                DataSettingUtils.WriteToCSV(Path.Combine(folderPath, "Discount.csv"), db.Discount.ToList());
                DataSettingUtils.WriteToCSV(Path.Combine(folderPath, "Employees.csv"), db.Employees.ToList());
                DataSettingUtils.WriteToCSV(Path.Combine(folderPath, "EmployeeType.csv"), db.EmployeeType.ToList());
                DataSettingUtils.WriteToCSV(Path.Combine(folderPath, "InventoryExport.csv"), db.InventoryExport.ToList());
                DataSettingUtils.WriteToCSV(Path.Combine(folderPath, "InventoryExportDetail.csv"), db.InventoryExportDetail.ToList());
                DataSettingUtils.WriteToCSV(Path.Combine(folderPath, "InventoryImport.csv"), db.InventoryImport.ToList());
                DataSettingUtils.WriteToCSV(Path.Combine(folderPath, "InventoryImportDetail.csv"), db.InventoryImportDetail.ToList());
                DataSettingUtils.WriteToCSV(Path.Combine(folderPath, "Material.csv"), db.Material.ToList());
                DataSettingUtils.WriteToCSV(Path.Combine(folderPath, "Parameter.csv"), db.Parameter.ToList());
                DataSettingUtils.WriteToCSV(Path.Combine(folderPath, "PaymentVoucher.csv"), db.PaymentVoucher.ToList());
                DataSettingUtils.WriteToCSV(Path.Combine(folderPath, "Receipt.csv"), db.Receipt.ToList());
                DataSettingUtils.WriteToCSV(Path.Combine(folderPath, "ReceiptDetail.csv"), db.ReceiptDetail.ToList());
            }
        }
    }
}

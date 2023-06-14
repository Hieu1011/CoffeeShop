using CoffeeShop.Database.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
//using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Database
{
    internal class DBContext : DbContext
    {
        public static DBContext CreateInstance()
        {
            // File.Delete(DBConfig.GetDBFilePath());
            var context = new DBContext(DBConfig.GetConnectionString());
            return context;
        }

        internal DBContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<Models.AccessPermission> AccessPermission { get; set; }
        public DbSet<Models.AccessPermissionGroup> AccessPermissionGroup { get; set; }
        public DbSet<Models.BeverageName> BeverageName { get; set; }
        public DbSet<Models.BeverageType> BeverageType { get; set; }
        public DbSet<Models.Discount> Discount { get; set; }
        public DbSet<Models.Employees> Employees { get; set; }
        public DbSet<Models.EmployeeType> EmployeeType { get; set; }
        public DbSet<Models.InventoryExport> InventoryExport { get; set; }
        public DbSet<Models.InventoryExportDetail> InventoryExportDetail { get; set; }
        public DbSet<Models.InventoryImport> InventoryImport { get; set; }
        public DbSet<Models.InventoryImportDetail> InventoryImportDetail { get; set; }
        public DbSet<Models.Material> Material { get; set; }
        public DbSet<Models.Parameter> Parameter { get; set; }
        public DbSet<Models.PaymentVoucher> PaymentVoucher { get; set; }
        public DbSet<Models.Receipt> Receipt { get; set; }
        public DbSet<Models.ReceiptDetail> ReceiptDetail { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


    }
}

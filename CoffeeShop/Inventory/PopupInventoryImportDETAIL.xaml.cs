using CoffeeShop.BUS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoffeeShop.Inventory
{
    public partial class PopupInventoryImportDETAIL : UserControl
    {
        public String selectionID = "";
        public String ImportName = "";
        
        public class InventoryImportDetailObject
        {
            public String name { get; set; }
            public String unitPrice { get; set; }
            public String amount { get; set; }
            public String unit { get; set; }
            public String totalCost { get; set; }
        }
        public PopupInventoryImportDETAIL()
        {
            InitializeComponent();
            dataGridMaterialImport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            LoadData();
        }
        public PopupInventoryImportDETAIL(String id, String importname, String importdate)
        {
            this.selectionID = id;
            this.ImportName = importname;
            InitializeComponent();
            dataGridMaterialImport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            tbDate.Text = importdate;
            tbEmployeeName.Text = importname;
            tbImportID.Text = id;
            if (selectionID != "")
                LoadData();
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
        public void LoadData()
        {
            var list = new ObservableCollection<InventoryImportDetailObject>();
            BUS_InventoryImport import = new BUS_InventoryImport();
            DataTable temp = import.selectDetail(selectionID);
            Console.WriteLine(temp.Rows.Count);
            foreach (DataRow row in temp.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                string unitprice = row["Đơn giá"].ToString();
                string unit = row["Đơn vị tính"].ToString();
                float tongtien = int.Parse(amount) * float.Parse(unitprice);
                list.Add(new InventoryImportDetailObject() { amount = amount, name = name, unit = unit, totalCost = tongtien.ToString(), unitPrice = unitprice });
            }
            this.dataGridMaterialImport.ItemsSource = list;
            this.tbTotalImportCost.Text = import.TotalCost(selectionID).ToString(); 
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}

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
    /// <summary>
    /// Interaction logic for PopupInventoryExportDETAIL.xaml
    /// </summary>
    public partial class PopupInventoryExportDETAIL : UserControl
    {
        public String selectionID = "";
        public String ExportName = "";

        public class InventoryExportDetailObject
        {
            public int number { get; set; }
            public String name { get; set; }
            public String unitPrice { get; set; }
            public String amount { get; set; }
            public String unit { get; set; }
            public String description { get; set; }
        }
        public PopupInventoryExportDETAIL()
        {
            InitializeComponent();
            dataGridMaterialExport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
        }
        public PopupInventoryExportDETAIL(String id, String name, String date)
        {
            this.selectionID = id;
            this.ExportName = name;
            InitializeComponent();
            dataGridMaterialExport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            tbDate.Text = date;
            tbEmployeeName.Text = name;
            tbExportID.Text = id;
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
            var list = new ObservableCollection<InventoryExportDetailObject>();
            BUS_InventoryExport export = new BUS_InventoryExport();
            DataTable temp = export.SelectDetail(selectionID);
            tbDescription.Text = export.SelectDescription(selectionID);
            Console.WriteLine(temp.Rows.Count);
            foreach (DataRow row in temp.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                string unit = row["Unit"].ToString();
                list.Add(new InventoryExportDetailObject() { number = list.Count+1, amount = amount, name = name,unit=unit });
            }
            this.dataGridMaterialExport.ItemsSource = list;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}

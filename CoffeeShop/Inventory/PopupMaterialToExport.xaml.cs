using CoffeeShop.BUS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for PopupMaterialToExport.xaml
    /// </summary>
    public partial class PopupMaterialToExport : UserControl
    {
        public UserControl parent { get; set; }
        List<String> temp = new List<string>();
        List<MAterialObject> mainList = new List<MAterialObject>();
        List<MAterialObject> findList = new List<MAterialObject>();
        public class MAterialObject : INotifyPropertyChanged
        {
            public string id { get; set; }
            public String name { get; set; }
            public String unit { get; set; }
            public String amount { get; set; }
            public bool _isSelected;
            public bool IsSelected
            {
                get { return _isSelected; }
                set { _isSelected = value; OnPropertyChanged(); }
            }
            #region INotifyPropertyChanged
            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string name = "")
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(name));
            }
            #endregion

        }
        public PopupMaterialToExport()
        {
            InitializeComponent();
        }
        public PopupMaterialToExport(UserControl parent)
        {
            InitializeComponent();
            dataGridMaterialExport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            LoadData();
            this.parent = parent;
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
        public void LoadData()
        {
            Dictionary<String, int> mapNameAmount = new Dictionary<string, int>();
            Dictionary<String, String> mapNameUnit = new Dictionary<string, string>();
            //mapping name with unit & import amount
            //With import amount
            BUS_InventoryImportDetail import = new BUS_InventoryImportDetail();
            DataTable temp = import.SelectAllImportDetailGroupByName();
            foreach (DataRow row in temp.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                mapNameAmount[name] = int.Parse(amount);
            }
            //With unit
            BUS_Material mater = new BUS_Material();
            DataTable tempMater = mater.selectAll();
            foreach (DataRow row in tempMater.Rows)
            {
                string name = row["MaterialName"].ToString();
                string unit = row["Unit"].ToString();
                mapNameUnit[name] = unit;
            }
            //calculate amount in stock = import - export (if have)
            BUS_InventoryExportDetail export = new BUS_InventoryExportDetail();
            DataTable temp1 = export.SelectAllExportDetailGroupByName();
            foreach (DataRow row in temp1.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                if (mapNameAmount.ContainsKey(name))
                    mapNameAmount[name] -= int.Parse(amount);
            }
            //finally get the amount of mater in stock (if not import yet then:  amount =0 )
            foreach (KeyValuePair<string, string> name in mapNameUnit)
            {
                int amount = 0;
                if (mapNameAmount.ContainsKey(name.Key))
                    amount = mapNameAmount[name.Key];
                if (amount == 0) continue;
                mainList.Add(new MAterialObject() { name = name.Key, amount = amount.ToString(), unit = name.Value, IsSelected = false }); ;
            }
            this.dataGridMaterialExport.ItemsSource = mainList;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void cbCheck_Checked(object sender, RoutedEventArgs e)
        {
            MAterialObject row = (MAterialObject)dataGridMaterialExport.SelectedItem;
            temp.Add(row.name);
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            foreach (MAterialObject obj in mainList.ToList())
            {
                if (obj.IsSelected == true)
                    temp.Add(obj.name);
            }
            if (parent.GetType() == new InventoryImportADD().GetType())
                ((InventoryImportADD)parent).MaterName = temp;
            else if (parent.GetType() == new InventoryExportADD().GetType())
                ((InventoryExportADD)parent).MaterName = temp;
            else if (parent.GetType() == new InventoryExportEDIT().GetType())
                ((InventoryExportEDIT)parent).MaterName = temp;
            else ((InventoryImportEDIT)parent).MaterName = temp;
            Window.GetWindow(this).Close();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            findList.Clear();
            foreach (MAterialObject obj in mainList.ToList())
            {
                if (obj.name.Contains(tbFind.Text))
                    findList.Add(obj);
            }
            this.dataGridMaterialExport.Items.Refresh();
            this.dataGridMaterialExport.ItemsSource = findList;
        }
    }
}

using CoffeeShop.BUS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for InventoryExportEDIT.xaml
    /// </summary>
    public partial class InventoryExportEDIT : UserControl
    {  
        public String selectionID = "";
        public String ImportName = "";
        public List<String> MaterName { get; set; }
        public List<InventoryObject> list = new List<InventoryObject>();
        public List<String> sqlCommand = new List<string>();
        Dictionary<String, int> mapNameAmountInStock = new Dictionary<string, int>();
        MainWindow _context;
       
        public class InventoryObject : INotifyPropertyChanged
        {
            public String name { get; set; }
            public String id { get; set; }
            public String unitPrice { get; set; }
            public String _amount;
            public string amount
            {
                get
                {
                    return this._amount;
                }

                set
                {
                    if (value != _amount)
                    {
                        _amount = value;
                        OnPropertyChanged();
                    }
                }
            }
            public String unit { get; set; }
            public String totalCost { get; set; }
            public String description { get; set; }
            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public InventoryExportEDIT()
        {
            InitializeComponent();
            dataGridMaterialExport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            if (selectionID != "")
                LoadData();
        }
        public InventoryExportEDIT(String id, String name, String date, String description, MainWindow mainWindow)
        {
            this.ImportName = name;
            this.selectionID = id;
            InitializeComponent();
            dataGridMaterialExport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            LoadData();
            this._context = mainWindow;
            tbEmployeeName.Text = name;
            tbDate.Text = date;
            tbExportID.Text = id;
            tbDescription.Text = description;
            
        }
        public Dictionary<String, int> loadAmountofMaterial()
        {
            Dictionary<String, int> mapNameAmount = new Dictionary<string, int>();
            Dictionary<String, String> mapNameUnit = new Dictionary<string, string>();
            //With import amount
            BUS_InventoryImportDetail import = new BUS_InventoryImportDetail();
            DataTable temp = import.SelectAllImportDetailGroupByName();
            foreach (DataRow row in temp.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                string use = row["isUse"].ToString();
                if (use == "1")
                    mapNameAmount[name] = int.Parse(amount);
            }
            //With unit
            BUS_Material mater = new BUS_Material();
            DataTable tempMater = mater.selectAll();
            foreach (DataRow row in tempMater.Rows)
            {
                string name = row["MaterialName"].ToString();
                string unit = row["Unit"].ToString();
                string use = row["isUse"].ToString();
                if (use == "1")
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
            foreach (KeyValuePair<string, string> name in mapNameUnit)
            {
                if (!mapNameAmount.ContainsKey(name.Key))
                    mapNameAmount[name.Key] = 0;
            }
            return mapNameAmount;
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
        public void LoadData()
        {
            mapNameAmountInStock = loadAmountofMaterial();
            BUS_InventoryExport export = new BUS_InventoryExport();
            DataTable temp = export.SelectDetail(selectionID);
            tbDescription.Text = export.SelectDescription(selectionID);
            Console.WriteLine(temp.Rows.Count);
            foreach (DataRow row in temp.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                string unit = row["Unit"].ToString();
                string materID = row["MaterialID"].ToString();
                list.Add(new InventoryObject() {  amount = amount, name = name, unit = unit, id= materID });
                if (mapNameAmountInStock.ContainsKey(name))
                    mapNameAmountInStock[name] += int.Parse(amount);
            }
            this.dataGridMaterialExport.ItemsSource = list;
            if (list.Count == 0) return;
            tbDescription.Text = list[0].description;
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var screen = new InventoryExport(_context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (list.Count.Equals(0))
            {
                MessageBox.Show($"Danh sách nguyên vật liệu, thiết bị không được để trống!");
                return;
            }
            foreach (InventoryObject obj in list)
            {
                int amountInExport; int temp1 = -1, temp2 = -1;
                if (int.TryParse(obj.amount, out amountInExport))
                {
                    if (mapNameAmountInStock[obj.name] < amountInExport)
                    {
                        MessageBox.Show($"Số lượng của '{obj.name}' trong kho chỉ còn {mapNameAmountInStock[obj.name]}!");
                        return;
                    }
                }
                if (!int.TryParse(obj.amount, out temp2) || temp2 <= 0)
                {
                    MessageBox.Show($"Số lượng của {obj.name} không hợp lệ!");
                    return;
                }
                //if (tbDescription.Text.Length == 0)
                //{
                //    MessageBox.Show($"Vui lòng nhập lí do !");
                //    return;
                //}
                string temp = $"insert into InventoryExportDetail values ('{selectionID}','{obj.id}','{obj.amount}')";
                sqlCommand.Add(temp);
            }
            BUS_InventoryExport export = new BUS_InventoryExport();
            export.updateDescription(selectionID, tbDescription.Text);
            BUS_InventoryExportDetail detail = new BUS_InventoryExportDetail();
            detail.Delete(selectionID);
            detail.ImportList(sqlCommand);
            var screen = new InventoryExport(_context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }
        public int findInList(String id)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].id == id)
                    return i;
            }
            return -1;
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            InventoryObject row = (InventoryObject)dataGridMaterialExport.SelectedItem;
            if (row != null)
            {
                list.RemoveAt(findInList(row.id));
                dataGridMaterialExport.Items.Refresh();
            }
        }
        public bool containInList(String id)
        {
            foreach (InventoryObject obj in list)
            {
                if (obj.id == id)
                    return true;
            }
            return false;
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "",
                Content = new PopupMaterialToExport(this),
                Width = 540,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;

            BUS_Material mater = new BUS_Material();
            DataTable temp = mater.selectByName(this.MaterName);
            if (temp == null) return;
            foreach (DataRow row in temp.Rows)
            {
                string name = row["MaterialName"].ToString();
                string unit = row["Unit"].ToString();
                string id = row["MaterialID"].ToString();
                if (!containInList(id))
                    list.Add(new InventoryObject() { id = id,  name = name, unit = unit });
            }
            dataGridMaterialExport.ItemsSource = list;
            dataGridMaterialExport.Items.Refresh();
        }

        private void tbAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
        }
        private void tbDescription_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[a-zA-Zàáạảãâầấậẩẫăằắặẳẵèéẹẻẽêềếệểễìíịỉĩòóọỏõôồốộổỗơờớợởỡùúụủũưừứựửữỳýỵỷỹđ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
    }
}

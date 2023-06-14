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
    /// Interaction logic for InventoryImportEDIT.xaml
    /// </summary>
    public partial class InventoryImportEDIT : UserControl
    {
        public String selectionID = "";
        public String ImportName = "";
        public List<String> MaterName { get; set; }
        public List<InventoryImportDetailObject> list = new List<InventoryImportDetailObject>();
        public List<String> sqlCommand = new List<string>();

        MainWindow _context;

        //Map to decide can change or not of material
        Dictionary<String, String> mapNameUnit = new Dictionary<string, string>();
        Dictionary<String, String> mapNameIsUse = new Dictionary<string, string>();
        public class InventoryImportDetailObject
        {
            public String name { get; set; }
            public String id { get; set; }
            public String unitPrice { get; set; }
            public String amount { get; set; }
            public String unit { get; set; }
            public String totalCost { get; set; }
        }
        public InventoryImportEDIT()
        {
            InitializeComponent();
            dataGridMaterialImport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            if (selectionID != "")
                LoadData();
        }
        public InventoryImportEDIT(String id, String importname, String importdate, MainWindow mainWindow)
        {
            this.selectionID = id;
            this.ImportName = importname;
            InitializeComponent();
            dataGridMaterialImport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            if (selectionID != "")
                LoadData();
            this._context = mainWindow;
            tbEmployeeName.Text = importname;
            tbDate.Text = importdate;
            tbImportID.Text = id;
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
        public void LoadData()
        {
            BUS_InventoryImport import = new BUS_InventoryImport();
            DataTable temp = import.selectDetail(selectionID);
            Console.WriteLine(temp.Rows.Count);
            foreach (DataRow row in temp.Rows)
            {
                string id = row["ID"].ToString();
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                string unitprice = row["Đơn giá"].ToString();
                string unit = row["Đơn vị tính"].ToString();
                int tongtien = int.Parse(amount) * int.Parse(unitprice);
                list.Add(new InventoryImportDetailObject() { id= id, amount = amount, name = name, unit = unit, totalCost = tongtien.ToString(), unitPrice = unitprice });
            }
            this.dataGridMaterialImport.ItemsSource = list;
            loadAllMap();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var screen = new InventoryImport(_context);
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
                Content = new PopupMaterialToImport(this),
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
                if (findInList(id) == -1 && name != "")
                    list.Add(new InventoryImportDetailObject() { id = id,  name = name, unit = unit });
            }
            dataGridMaterialImport.ItemsSource = list;
            dataGridMaterialImport.Items.Refresh();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            InventoryImportDetailObject row = (InventoryImportDetailObject)dataGridMaterialImport.SelectedItem;
            if (row != null)
            {
                try
                {
                    list.RemoveAt(findInList(row.id));
                    dataGridMaterialImport.Items.Refresh();
                }
                catch (Exception) { }
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(sqlCommand[0]);
            if (list.Count.Equals(0))
            {
                MessageBox.Show($"Danh sách nguyên vật liệu, thiết bị không được để trống !");
                return;
            }
            foreach (InventoryImportDetailObject obj in list)
            {
                int temp1 = -1, temp2 = -1;
                if (!int.TryParse(obj.unitPrice, out temp1) || temp1 <= 0 || obj.unitPrice == "" || obj.unitPrice == null)
                {
                    MessageBox.Show($"Đơn giá của {obj.name} không hợp lệ!");
                    return;
                }
                else if (!int.TryParse(obj.amount, out temp2) || temp2 <= 0)
                {
                    MessageBox.Show($"Số lượng của {obj.name} không hợp lệ!");
                    return;
                }
                string temp = $"insert into InventoryImportDetail values ('{selectionID}','{obj.id}','{obj.amount}','{obj.unitPrice}')";
                sqlCommand.Add(temp);
            }
            BUS_InventoryImportDetail detail = new BUS_InventoryImportDetail();
            detail.Delete(selectionID);
            detail.ImportList(sqlCommand);            
            var screen = new InventoryImport(_context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }
        private void tbPrice_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            //tb.GotFocus -= tbPrice_GotFocus;
        }

        private void tbAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            //tb.GotFocus -= tbAmount_GotFocus;
        }

        public void loadAllMap()
        {
            BUS_Material mater = new BUS_Material();
            DataTable temp = mater.selectAll();
            foreach (DataRow row in temp.Rows)
            {
                string name = row["materialName"].ToString();
                string unit = row["unit"].ToString();
                string isUse = row["isUse"].ToString();
                mapNameUnit[name] = unit;
                mapNameIsUse[name] = isUse;
            }
        }

        private void tbAmount_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InventoryImportDetailObject row = (InventoryImportDetailObject)dataGridMaterialImport.SelectedItem;
            //MessageBox.Show(row.unit);
            if (mapNameIsUse[row.name] != "1" || mapNameUnit[row.name] != row.unit)
            {
                MessageBox.Show("Thông tin về vật liệu này đã được chỉnh sửa, bạn không thể thay đổi ");
                TextBox tb1 = (TextBox)sender;
                tb1.IsEnabled = false;
                tb1.IsReadOnly = true;
                tb1.MouseLeftButtonUp -= tbAmount_MouseDown;
                return;
            }
            TextBox tb = (TextBox)sender;
            tb.GotFocus += tbAmount_GotFocus;
        }

        private void tbPrice_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            InventoryImportDetailObject row = (InventoryImportDetailObject)dataGridMaterialImport.SelectedItem;
            //MessageBox.Show(row.unit);
            if (mapNameIsUse[row.name] != "1" || mapNameUnit[row.name] != row.unit)
            {
                MessageBox.Show("Thông tin về vật liệu này đã được chỉnh sửa, bạn không thể thay đổi ");
                TextBox tb1 = (TextBox)sender; tb1.IsEnabled = false;
                tb1.IsReadOnly = true;
                tb1.MouseLeftButtonUp -= tbPrice_MouseLeftButtonUp;
                return;
            }
            TextBox tb = (TextBox)sender;
            tb.GotFocus += tbPrice_GotFocus;
        }
    }
}

using CoffeeShop.BUS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
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
    /// Interaction logic for InventoryImportADD.xaml
    /// </summary>
    public partial class InventoryImportADD : UserControl
    {
        public String selectionID = "";
        public List<String> MaterName = new List<String>();
        public List<InventoryImportDetailObject> list = new List<InventoryImportDetailObject>();
        MainWindow _context;
        
        public class InventoryImportDetailObject : INotifyPropertyChanged
        {
            public String name { get; set; }
            public String id { get; set; }
            public string _unitPrice;
            public string unitPrice
            {
                get
                {
                    return this._unitPrice;
                }

                set
                {
                    if (value != _unitPrice)
                    {
                        _unitPrice = value;
                        OnPropertyChanged();
                        OnPropertyChanged("totalCost");
                    }
                }
            }
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
                        OnPropertyChanged("totalCost");
                    }
                }
            }
            public String unit { get; set; }
            public String totalCost
            {
                get {
                    int temp, temp1;
                    if(int.TryParse(unitPrice, out temp )&& int.TryParse(amount, out temp1))
                        return (temp*temp1).ToString();
                    return null;
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
            {
                this.PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
            }
            
        }
        public InventoryImportADD()
        {
            InitializeComponent();
            dataGridMaterialImport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
        }
        
        public InventoryImportADD(String id, MainWindow mainWindow)
        {
            this.selectionID = id;
            InitializeComponent();
            dataGridMaterialImport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            tbDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            tbEmployeeName.Text = id;
            this._context = mainWindow;
            this.dataGridMaterialImport.ItemsSource= new List<InventoryImportDetailObject>();
        }

        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
        public int findInList (String id)
        {
            for(int i=0; i < list.Count; i++)
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
            DataTable temp = mater.selectByName( this.MaterName);
            if (temp == null) return;
            foreach (DataRow row in temp.Rows)
            {
                string name = row["MaterialName"].ToString();
                string unit = row["Unit"].ToString();
                string id = row["MaterialID"].ToString();
                if(findInList(id)== -1  && name != "")
                    list.Add(new InventoryImportDetailObject() { id=id, name=name, unit=unit });
            }
            
            dataGridMaterialImport.ItemsSource = list;
            dataGridMaterialImport.Items.Refresh();
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {

            dataGridMaterialImport.Items.Refresh();
            
            BUS_InventoryImport import = new BUS_InventoryImport();
            String newImportID = import.Create(tbEmployeeName.Text, tbDate.Text);
            if (newImportID == null) return;
            List<String> sqlString = new List<string>();
            if (list.Count.Equals(0))
            {
                MessageBox.Show($"Danh sách nguyên vật liệu, thiết bị không được để trống!");
                return;
            }
            foreach (InventoryImportDetailObject obj in list)
            {
                int temp1=-1,temp2=-1;
                if (!int.TryParse(obj.unitPrice,out temp1) || temp1 <= 0 || obj.unitPrice == "" || obj.unitPrice == null)                    
                {
                    MessageBox.Show($"Đơn giá của {obj.name} không hợp lệ!");
                    import.Delete(newImportID);
                    return;
                }
                else if ( !int.TryParse(obj.amount, out temp2) || temp2 <= 0)
                {
                    MessageBox.Show($"Số lượng của {obj.name} không hợp lệ!");
                    import.Delete(newImportID);
                    return;
                }
                string temp = $"insert into InventoryImportDetail values ('{newImportID}','{obj.id}','{obj.amount}','{obj.unitPrice}')";
                sqlString.Add(temp);
            }
            
            BUS_InventoryImportDetail detail = new BUS_InventoryImportDetail();
            detail.ImportList(sqlString);
            var screen = new InventoryImport(_context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
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

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
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
    }
}

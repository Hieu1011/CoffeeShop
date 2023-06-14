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
    /// Interaction logic for MaterialList.xaml
    /// </summary>
    public partial class MaterialList : UserControl
    {
        public int page { get; set; }
        public MaterialList()
        {
            InitializeComponent();

            dataGridMaterial.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            Loaded += LoadData;
        }
        public void LoadData(Object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
        public List<InventoryObject> list = new List<InventoryObject>();
        public List<InventoryObject> findList = new List<InventoryObject>();
        public List<String> materialInUse = new List<string>();
        public String selectionName = "";
        public class InventoryObject
        {
            public string id { get; set; }
            public String Name { get; set; }
            public String Unit { get; set; }
            public String Amount { get; set; }
            //this variable for button add/del/edit
            public String IsUsing { get; set; }
        }
        public void LoadData()
        {
            btBack.IsEnabled = false;
            list.Clear();
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
                string use = row["isUse"].ToString();
                if (use == "1")
                    mapNameAmount[name] = int.Parse(amount);
                else materialInUse.Add(name);
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
                materialInUse.Add(name);
            }
            //finally get the amount of mater in stock (if not import yet then:  amount =0 )
            int number0 = 1;
            foreach (KeyValuePair<string, string> name in mapNameUnit)
            {
                int amount = 0;
                if (mapNameAmount.ContainsKey(name.Key))
                    amount = mapNameAmount[name.Key];
                list.Add(new InventoryObject() { Name = name.Key, Amount = amount.ToString(), Unit = name.Value });
                number0++;
            }
            BUS_Parameter busParameter = new BUS_Parameter();
            int rowPerSheet = busParameter.GetValue("RowInList");
            if (list.Count % rowPerSheet == 0)
                lblMaxPage.Content = list.Count / rowPerSheet;
            else lblMaxPage.Content = list.Count / rowPerSheet + 1;
            if (int.Parse(lblMaxPage.Content.ToString()) == 0)
                this.tbNumPage.Text = "0";
            if (int.Parse(lblMaxPage.Content.ToString()) == 1)
                btNext.IsEnabled = false;
            else btNext.IsEnabled = true;
            splitDataGrid(1);
        }
        public void splitDataGrid(int numpage)
        {
            try 
            {                
                List<InventoryObject> displayList = new List<InventoryObject>();
                if (numpage == 0)
                {
                    this.dataGridMaterial.ItemsSource = list;
                    this.dataGridMaterial.Items.Refresh();
                    return;
                }
                BUS_Parameter busParameter = new BUS_Parameter();
                int numberPersheet = busParameter.GetValue("RowInList");
                if (list.Count < numberPersheet * numpage)
                {
                    displayList = list.GetRange((numpage - 1) * numberPersheet, list.Count - (numpage - 1) * numberPersheet);
                }
                else displayList = list.GetRange((numpage - 1) * numberPersheet, numberPersheet);
                //displayList = list.GetRange(10, list.Count-10);
                this.dataGridMaterial.ItemsSource = displayList;
            }
            catch (Exception)
            {
                MessageBox.Show("Something wrong!");
            }
            
        }
        public void splitDataGridFind(int numpage)
        {
            try
            {
                List<InventoryObject> displayList = new List<InventoryObject>();
                if (numpage == 0)
                {
                    this.dataGridMaterial.ItemsSource = findList;
                    this.dataGridMaterial.Items.Refresh();
                    return;
                }
                BUS_Parameter busParameter = new BUS_Parameter();
                int numberPersheet = busParameter.GetValue("RowInList");
                if (findList.Count < numberPersheet * numpage)
                {
                    displayList = findList.GetRange((numpage - 1) * numberPersheet, findList.Count - (numpage - 1) * numberPersheet);
                }
                else displayList = findList.GetRange((numpage - 1) * numberPersheet, numberPersheet);
                //displayList = list.GetRange(10, list.Count-10);
                this.dataGridMaterial.ItemsSource = displayList;
            }
            catch (Exception)
            {
                MessageBox.Show("Something wrong!");
            }

        }
        public void findMaterial(String keyword)
        {
            findList.Clear();
            tbNumPage.Text = "1";
            btBack.IsEnabled = false;
            foreach (InventoryObject obj in list.ToList())
            {
                if (obj.Name.ToLower().Contains(keyword.ToLower()))
                    findList.Add(obj);
            }
            BUS_Parameter busParameter = new BUS_Parameter();
            int rowPerSheet = busParameter.GetValue("RowInList");
            btNext.IsEnabled = true;
            if (findList.Count % rowPerSheet == 0)
                lblMaxPage.Content = findList.Count / rowPerSheet;
            else lblMaxPage.Content = findList.Count / rowPerSheet + 1;
            if (int.Parse(lblMaxPage.Content.ToString()) == 0)
                this.tbNumPage.Text = "0";
            else this.tbNumPage.Text = "1";
            if (int.Parse(lblMaxPage.Content.ToString()) < 2)
                btNext.IsEnabled = false;
            splitDataGridFind(int.Parse(tbNumPage.Text));
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            findMaterial(tbFind.Text);

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
                Title = "Thêm vật liệu",
                Content = new PopupAddMaterial(),
                Width = 460,
                Height = 300,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
            LoadData();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            InventoryObject row = (InventoryObject)dataGridMaterial.SelectedItem;
            if (row != null)
            {
                Window window = new Window
                {
                    ResizeMode = ResizeMode.NoResize,
                    WindowStyle = WindowStyle.None,
                    Title = "Sửa vật liệu",
                    Content = new PopupEditMaterial(row.Name, row.Unit),
                    Width = 460,
                    Height = 300,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                window.ShowDialog();
                LoadData();
            }
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void dataGridInfo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                InventoryObject row = (InventoryObject)dataGridMaterial.SelectedItem;
                this.selectionName = row.Name;
            }
            catch (Exception) { }

        }

        public bool Delete(String name)
        {
            BUS_Material material = new BUS_Material();
            bool result = material.Delete(name);
            if (!result)
                //MessageBox.Show($"Đã xóa thông tin của {name}");
            //else 
                MessageBox.Show($"Đã có lỗi trong quá trình xóa {name}");
            LoadData();
            return result;
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            InventoryObject row = (InventoryObject)dataGridMaterial.SelectedItem;
            if(int.Parse(row.Amount) > 0)
            {
                MessageBox.Show($"Không thể xóa vật liệu vẫn còn trong kho!");                
                return;
            }
            for (int i = 0; i < materialInUse.Count; i++)
            {
                if (materialInUse[i].Contains(row.Name))
                {
                    MessageBox.Show($"Không thể xóa vật liệu đã được nhập/xuất kho!");
                    return;
                }

            }
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Xóa vật liệu ",
                Content = new PopupDeleteConfirm(this, row.Name), //delete message
                Width = 420,
                Height = 210,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
            this.tbNumPage.Text = "1";
            this.btBack.IsEnabled = false;
            if ((int)lblMaxPage.Content == 1)
                this.btNext.IsEnabled = false;
            else this.btNext.IsEnabled = true;
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            if (int.Parse(tbNumPage.Text)>1)
            {
                tbNumPage.Text = (int.Parse(tbNumPage.Text) - 1).ToString();
                if(tbFind.Text == "")
                    splitDataGrid(int.Parse(tbNumPage.Text));
                else splitDataGridFind(int.Parse(tbNumPage.Text));
                if (int.Parse(tbNumPage.Text) == 1)
                {
                    btBack.IsEnabled = false;
                    if((int)lblMaxPage.Content == 1)
                        btNext.IsEnabled = false;
                    else btNext.IsEnabled = true;
                }
                    
                else
                {
                    btNext.IsEnabled = true;
                    btBack.IsEnabled = true;
                }
            }
                
        }

        private void btNext_Click(object sender, RoutedEventArgs e)
        {
            if((int.Parse(lblMaxPage.Content.ToString()) - int.Parse(tbNumPage.Text)) >=1)
            {
                tbNumPage.Text = (int.Parse(tbNumPage.Text) + 1).ToString();
                if (tbFind.Text == "")
                    splitDataGrid(int.Parse(tbNumPage.Text));
                else splitDataGridFind(int.Parse(tbNumPage.Text));
                if (int.Parse(tbNumPage.Text) == (int)lblMaxPage.Content)
                {
                   // MessageBox.Show("");
                    btNext.IsEnabled = false;
                    btBack.IsEnabled = true;
                }                    
                else
                {
                    btNext.IsEnabled = true;
                    
                }
            }
        }

        private void tbNumPage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void tbNumPage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.Any(x => Char.IsDigit(x));
            if (e.Text.Contains(" "))
                e.Handled = false;
        }

        private void tbNumPage_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
           // tb.GotFocus -= tbNumPage_GotFocus;
        }

        private void tbNumPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && int.TryParse(tbNumPage.Text,out int i))
            {
                if (int.Parse(tbNumPage.Text) > 0 && int.Parse(tbNumPage.Text) <= int.Parse(lblMaxPage.Content.ToString()))
                {
                    if (tbFind.Text == "")
                        splitDataGrid(int.Parse(tbNumPage.Text));
                    else splitDataGridFind(int.Parse(tbNumPage.Text));
                    if(int.Parse(tbNumPage.Text) == (int)lblMaxPage.Content)
                    {
                        btNext.IsEnabled = false;
                        btBack.IsEnabled = true;                       
                    }
                    else if(int.Parse(tbNumPage.Text) == 1)
                    {
                        btNext.IsEnabled = true;
                        btBack.IsEnabled = false;
                    }
                    else
                    {
                        btNext.IsEnabled = true;
                        btBack.IsEnabled = true;
                    }
                    if (int.Parse(tbNumPage.Text) == (int)lblMaxPage.Content && (int)lblMaxPage.Content == 1)

                    {
                        btNext.IsEnabled = false;
                        btBack.IsEnabled = false;
                    }
                }
                else MessageBox.Show("Trang không hợp lệ!");
            }
        }
    }
}

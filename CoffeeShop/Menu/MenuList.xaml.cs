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
using CoffeeShop.BUS;
using CoffeeShop.DTO;

namespace CoffeeShop.Menu
{
    /// <summary>
    /// Interaction logic for MenuList.xaml
    /// </summary>
    public partial class MenuList : UserControl
    {
        BUS_Beverage bus;
        MainWindow _context;
        bool find = false;
        int maxNumpage;
        int numRow;
        int currentNumpage;
        int limitRow;
        public MenuList()
        {
            InitializeComponent();
            dgMenu.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            bus = new BUS_Beverage();
            Loaded += LoadData;
        }
        public MenuList(MainWindow mainWindow)
        {
            InitializeComponent();
            dgMenu.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            bus = new BUS_Beverage();
            currentNumpage = 1;
            this._context = mainWindow;
            Loaded += LoadData;
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 60;
        }
        public void LoadData(Object sender, RoutedEventArgs e)
        {
            loadData();
        }
        void loadData()
        {
            var list = new ObservableCollection<DTO_Beverage>();
            List<string> listt = new List<string>();
            BUS_Parameter busParameter = new BUS_Parameter();
            limitRow = busParameter.GetValue("RowInList");
            listt = bus.getBeverageType();
            listt.Add("All");
            cbBeverageType.ItemsSource = listt;
            DataTable temp = bus.getAllBeverage();
            int rowNumber = Int32.Parse(tbNumPage.Text);
            int count = 1;
            foreach (DataRow row in temp.Rows)
            {
                string name = row["BeverageName"].ToString();
                string id = row["BeverageID"].ToString();
                string type = row["BeverageTypeName"].ToString();
                int price = Int32.Parse(row["Price"].ToString());
                byte[] link =(byte [])row["Link"];
                if (count >= (rowNumber - 1) * limitRow + 1 && count <= rowNumber * limitRow)
                {
                    list.Add(new DTO_Beverage() { BeverageID = id, BeverageName = name, BeverageTypeID = type, Price = price, Link=link });
                    count++;
                }
                else count++;
            }
            numRow = temp.Rows.Count;
            dgMenu.ItemsSource = list;
            dgMenu.Items.Refresh();
            setNumPage();
        }
        void setNumPage()
        {
            if (numRow % limitRow == 0)
            {
                maxNumpage = numRow / limitRow;
            }
            else
                maxNumpage = numRow / limitRow + 1;
            
            lblMaxPage.Content = maxNumpage.ToString();
            if (maxNumpage == 0)
            {
                tbNumPage.Text = "0";
                btnPageNext.IsEnabled = false;
            }
            if (currentNumpage == maxNumpage)
                btnPageNext.IsEnabled = false;
            else
                btnPageNext.IsEnabled = true;
            if (currentNumpage == 1)
                btnPagePre.IsEnabled = false;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //CREATE POPUP FROM HERE
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Thêm món",
                Content = new PopupAddMenu(_context),
                Width = 460,
                Height = 620,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            loadData();
            
        }

        private void ButtonBeverageType_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Thêm loại món",
                Content = new PopupBeverageType(),
                Width = 400,
                Height = 400,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DTO_Beverage row = (DTO_Beverage)dgMenu.SelectedItem;
            var rowView = dgMenu.SelectedItem;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Sửa món",
                Content = new PopupEditMenu(row.BeverageName, row.BeverageTypeID, row.Price.ToString(), row.BeverageID, row.Link, this._context),
                Width = 460,
                Height = 620,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            loadData();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DTO_Beverage row = (DTO_Beverage)dgMenu.SelectedItem;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Xóa món",
                Content = new PopupDeleteConfirm(row, this._context),
                Width = 420,
                Height = 210,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null; 
            setNumPage();
            if (maxNumpage < int.Parse(tbNumPage.Text))
                tbNumPage.Text = (int.Parse(tbNumPage.Text) - 1).ToString();
            loadData();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            currentNumpage = 1;
            tbNumPage.Text = "1";
            findBeverage();
        }
        void findBeverage()
        {
            find = true;
            var list = new ObservableCollection<DTO_Beverage>();
            DataTable temp = null;
            if (cbBeverageType.SelectedItem != null && !cbBeverageType.SelectedItem.ToString().Equals("All"))
            {
                temp = bus.findBeverage(cbBeverageType.SelectedItem.ToString(), tbName.Text);
            }
            else
                if ((cbBeverageType.SelectedItem == null || cbBeverageType.SelectedItem.ToString().Equals("All")) && tbName.Text.Length != 0 )
                    temp = bus.findBeverage("", tbName.Text);
                else
                    loadData();
            if (temp!=null)
            {
                int count = 1;
                int rowNumber = Int32.Parse(tbNumPage.Text);
                foreach (DataRow row in temp.Rows)
                {
                    string name = row["BeverageName"].ToString();
                    string id = row["BeverageID"].ToString();
                    string type = row["BeverageTypeName"].ToString();
                    int price = Int32.Parse(row["Price"].ToString());
                    byte[] link = (byte[])row["Link"];
                    if (count >= (rowNumber - 1) * limitRow + 1 && count <= rowNumber * limitRow)
                    {
                        list.Add(new DTO_Beverage() { BeverageID = id, BeverageName = name, BeverageTypeID = type, Price = price, Link = link });
                        count++;
                    }
                    else count++;
                }
                numRow = temp.Rows.Count;
                setNumPage();
                dgMenu.ItemsSource = list;
                dgMenu.Items.Refresh();
            }    
            
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            currentNumpage = Int32.Parse(tbNumPage.Text);
            if (Int32.Parse(tbNumPage.Text) == 1)
            {
                btnPagePre.IsEnabled = false;
            }
            else
            {
                tbNumPage.Text = (Int32.Parse(tbNumPage.Text) - 1).ToString();
                currentNumpage--;
                btnPageNext.IsEnabled = true;
                if (!find)
                    loadData();
                else
                    findBeverage();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            currentNumpage = Int32.Parse(tbNumPage.Text);
            if (Int32.Parse(tbNumPage.Text) == maxNumpage)
            {
                btnPageNext.IsEnabled = false;
            }
            else
            {
                tbNumPage.Text = (Int32.Parse(tbNumPage.Text) + 1).ToString();
                currentNumpage++;
                btnPagePre.IsEnabled = true;
                if (!find)
                    loadData();
                else
                    findBeverage();
            }
            

        }

        private void tbNumPage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
            if (e.Key == Key.Enter)
            {
                if (tbNumPage.Text.Length != 0 && int.Parse(tbNumPage.Text) <= maxNumpage && int.Parse(tbNumPage.Text) > 0)
                {
                    int newPage = Int32.Parse(tbNumPage.Text);
                    currentNumpage = newPage;
                    if (currentNumpage == 1)
                        btnPagePre.IsEnabled = false;
                    else
                        btnPagePre.IsEnabled = true;
                    if (currentNumpage == maxNumpage)
                        btnPageNext.IsEnabled = false;
                    else
                        btnPageNext.IsEnabled = true;
                    loadData();
                }     
                else
                {
                    MessageBox.Show("Không có trang này!");
                    tbNumPage.Text = currentNumpage.ToString();
                    if (!find)
                        loadData();
                    else
                        return;
                }
            }
        }

        private void tbNumPage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.Any(x => Char.IsDigit(x));
            if (e.Text.Contains(" "))
                e.Handled = false;
        }

        private void tbName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && tbName.Text.Length==0 && !cbBeverageType.SelectedItem.ToString().Equals("All"))
            {
                findBeverage();
            }
            if (e.Key == Key.Delete && tbName.Text.Length == 0 && cbBeverageType.SelectedItem.ToString().Equals("All"))
                loadData();
        }
    }
}

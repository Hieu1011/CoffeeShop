using CoffeeShop.BUS;
using CoffeeShop.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoffeeShop.Account
{
    /// <summary>
    /// Interaction logic for AccountList.xaml
    /// </summary>
    public partial class AccountList : UserControl
    {
        MainWindow _context;
        int limitRow;
        int currentPage;
        class AccountInfo
        {
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string pass { get; set; }
            public bool state { get; set; }
            public AccountInfo() { }
            public AccountInfo(string newid, string newname, string newtype, string newpass, bool newState)
            {
                id = newid;
                name = newname;
                type = newtype;
                pass = newpass;
                state = newState;
            }
        }

        public AccountList()
        {
            InitializeComponent();
            dataGridAccount.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            LoadData();
        }

        public AccountList(MainWindow mainWindow)
        {
            InitializeComponent();
            Loaded += LoadData;
            dataGridAccount.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            this._context = mainWindow;
        }

        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1 + limitRow * (currentPage - 1);
            e.Row.Height = 40;
        }

        public void LoadData(Object sender, RoutedEventArgs e)
        {
            LoadData();
            currentPage = 1;
            tbNumPage.Text = "1";
            btnPagePre.IsEnabled = false;
        }    

        public void LoadData()
        {
            BUS_Parameter busParameter = new BUS_Parameter();
            limitRow = busParameter.GetValue("RowInList");
            BUS_Employees bus_employees = new BUS_Employees();
            int empCount = bus_employees.CountEmployees();
            if (empCount % limitRow == 0)
                lblMaxPage.Content = empCount / limitRow;
            else
                lblMaxPage.Content = empCount / limitRow + 1;
            if (currentPage == (int)lblMaxPage.Content)
                btnPageNext.IsEnabled = false;
            else
                btnPageNext.IsEnabled = true;
            ReloadDGAccount();
        }
        
        void ReloadDGAccount()
        {
            BUS_Employees busEmp = new BUS_Employees();
            List<AccountInfo> employees = new List<AccountInfo>();
            DataTable temp = busEmp.GetEmployees(limitRow, limitRow * (currentPage - 1));

            foreach (DataRow row in temp.Rows)
            {
                string id = row["EmployeeID"].ToString();
                string name = row["EmployeeName"].ToString();
                string type = row["EmployeeTypeName"].ToString();
                string pass = row["Password"].ToString();
                bool state = true;
                if (row["State"].ToString() == "0")
                    state = false;
                employees.Add(item: new AccountInfo(id, name, type, pass, state));
            }
            this.dataGridAccount.ItemsSource = employees;
            this.dataGridAccount.Items.Refresh();
            
        }    

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Thêm tài khoản",
                Content = new PopupAddAccount(),
                Width = 460,
                Height = 460,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();

            BUS_Employees busEmp = new BUS_Employees();
            int empCount = busEmp.CountEmployees();
            if (empCount % limitRow == 0)
                lblMaxPage.Content = empCount / limitRow;
            else
                lblMaxPage.Content = empCount / limitRow + 1;

            if (currentPage < (int)lblMaxPage.Content)
                btnPageNext.IsEnabled = true;
            else
                btnPageNext.IsEnabled = false;

            ReloadDGAccount();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string id = ((Button)sender).Tag.ToString();

            if (_context.GetCurrentEmpID() == id)
            {
                MessageBox.Show("Không được xóa tài khoản của bản thân!");
                return;
            }

            if (id == "E001")
            {
                MessageBox.Show("Đây là tài khoản gốc, không thể xóa!");
                return;
            }    

            BUS_Employees busEmp = new BUS_Employees();
            string name = busEmp.GetEmpNameByID(id);

            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Xóa tài khoản",
                Content = new PopupDeleteConfirm($"Bạn có chắc chắn muốn xóa tài khoản {id}\ncủa nhân viên {name} không?", id, 1),
                Width = 420,
                Height = 210,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();

            int empCount = busEmp.CountEmployees();
            if (empCount % limitRow == 0)
                lblMaxPage.Content = empCount / limitRow;
            else
                lblMaxPage.Content = empCount / limitRow + 1;
            if (currentPage > (int)lblMaxPage.Content)
            {
                tbNumPage.Text = (--currentPage).ToString();
            }

            if (currentPage == (int)lblMaxPage.Content)
                btnPageNext.IsEnabled = false;
            else
                btnPageNext.IsEnabled = true;

            if (currentPage == 1)
                btnPagePre.IsEnabled = false;
            else
                btnPagePre.IsEnabled = true;

            ReloadDGAccount();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            string id = ((Button)sender).Tag.ToString();
            BUS_Employees busEmp = new BUS_Employees();
            DTO_Employees editEmp = busEmp.GetEmpByID(id);

            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Sửa tài khoản",
                Content = new PopupEditAccount(editEmp),
                Width = 460,
                Height = 460,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
            LoadData();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void cbCheck_Click(object sender, RoutedEventArgs e)
        {
            string id = ((CheckBox)sender).Tag.ToString();
            bool? state = ((CheckBox)sender).IsChecked;
            if (state == false)
            {
                if (_context.GetCurrentEmpID() == id)
                {
                    MessageBox.Show("Không được vô hiệu hóa tài khoản của bản thân!");
                    ((CheckBox)sender).IsChecked = true;
                    return;
                }

                if ("E001" == id)
                {
                    MessageBox.Show("Đây là tài khoản gốc, không được vô hiệu hóa!");
                    ((CheckBox)sender).IsChecked = true;
                    return;
                }
            }    
            BUS_Employees busEmp = new BUS_Employees();
            string name = busEmp.GetEmpNameByID(id);

            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window;
            if (state == true)
                window = new Window
                {
                    ResizeMode = ResizeMode.NoResize,
                    WindowStyle = WindowStyle.None,
                    Title = "Kích hoạt tài khoản",
                    Content = new PopupDeleteConfirm($"Bạn có chắc chắn muốn kích hoạt tài khoản {id} \n của nhân viên {name} không?", id, 3),
                    Width = 380,
                    Height = 210,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
            else
                window = new Window
                {
                    ResizeMode = ResizeMode.NoResize,
                    WindowStyle = WindowStyle.None,
                    Title = "Vô hiệu hóa tài khoản",
                    Content = new PopupDeleteConfirm($"Bạn có chắc chắn muốn vô hiệu hóa tài khoản \n{id} của nhân viên {name} không?", id, 4),
                    Width = 380,
                    Height = 210,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
            window.ShowDialog();
            LoadData();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void tbNumPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                int newPage = Int32.Parse(tbNumPage.Text);
                if (newPage > (int)lblMaxPage.Content || newPage == 0)
                {
                    MessageBox.Show("Không có trang này!");
                    return;
                }
                currentPage = newPage;
                if (currentPage == 1)
                    btnPagePre.IsEnabled = false;
                else
                    btnPagePre.IsEnabled = true;
                if (currentPage == (int)lblMaxPage.Content)
                    btnPageNext.IsEnabled = false;
                else
                    btnPageNext.IsEnabled = true;

                ReloadDGAccount();
            }
        }

        private void btnPagePre_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                tbNumPage.Text = (--currentPage).ToString();
                btnPageNext.IsEnabled = true;
                ReloadDGAccount();
            }
            if (currentPage == 1)
                btnPagePre.IsEnabled = false;
        }

        private void btnPageNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < (int)lblMaxPage.Content)
            {
                tbNumPage.Text = (++currentPage).ToString();
                btnPagePre.IsEnabled = true;
                ReloadDGAccount();
            }
            if (currentPage == (int)lblMaxPage.Content)
                btnPageNext.IsEnabled = false;
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
    }
}

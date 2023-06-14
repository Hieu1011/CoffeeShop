using CoffeeShop.BUS;
using CoffeeShop.View;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using Dietapp;
using CoffeeShop.BUS;
using CoffeeShop.Database.Models;
using CoffeeShop.Database;
using CoffeeShop.View;
using CoffeeShop;
using System.Security.Principal;
using CoffeeShop.ViewModel;

namespace CoffeeShop {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string currentEmpType;
        string currentEmpID;
        static string oldPassword = "";
        static string oldUsername = "";
        static int check = 0;
        public MainWindow()
        {

            InitializeComponent();
            currentEmpID = "";
            var item1 = new ItemMenu("Thu ngân", "AP001", new Cashier(this, currentEmpID), PackIconKind.Schedule);

            var item2 = new ItemMenu("Menu", "AP006", new Menu.MenuList(this), PackIconKind.CalendarTextOutline);

            var item3 = new ItemMenu("Ưu đãi", "AP007", new Discount.DiscountList(this), PackIconKind.ShoppingBasket);

            var menuInventory = new List<SubItem>();
            menuInventory.Add(new SubItem("Thông tin kho", "AP003", new Inventory.MaterialList()));
            menuInventory.Add(new SubItem("Nhập kho", "AP003", new Inventory.InventoryImport(this)));
            menuInventory.Add(new SubItem("Xuất kho", "AP003", new Inventory.InventoryExport(this)));
            var item4 = new ItemMenu("Kho", menuInventory, PackIconKind.Warehouse);

            var menuRevenue = new List<SubItem>();
            menuRevenue.Add(new SubItem("Danh sách hóa đơn", "AP001", new IncomeAndPayment.ReceiptList()));
            menuRevenue.Add(new SubItem("Danh sách chi", "AP005", new IncomeAndPayment.PaymentList(this)));
            var item5 = new ItemMenu("Thu chi", menuRevenue, PackIconKind.ScaleBalance);

            var menuReport = new List<SubItem>();
            menuReport.Add(new SubItem("Mặt hàng bán chạy", "AP008", new Report.ReportSale()));
            menuReport.Add(new SubItem("Lợi nhuận", "AP008", new Report.ReportProfit()));
            var item6 = new ItemMenu("Báo cáo thống kê", menuReport, PackIconKind.ChartLineVariant);

            var menuAccount = new List<SubItem>();
            menuAccount.Add(new SubItem("Tài khoản", "AP002", new Account.AccountList(this)));
            menuAccount.Add(new SubItem("Nhóm tài khoản", "AP003", new Account.GroupAccountList()));
            var item7 = new ItemMenu("Tài khoản", menuAccount, PackIconKind.Register);

            var item8 = new ItemMenu("Thiết lập quy định", "AP009", new Rule.Rule(this), PackIconKind.Cog);

            var item9 = new ItemMenu("Quản lý dữ liệu", "AP010", new DataSetting.DataSetting(this), PackIconKind.Database);

            Menu.Children.Add(new MenuItem(item1, this));
            Menu.Children.Add(new MenuItem(item2, this));
            Menu.Children.Add(new MenuItem(item3, this));
            Menu.Children.Add(new MenuItem(item4, this));
            Menu.Children.Add(new MenuItem(item5, this));
            Menu.Children.Add(new MenuItem(item6, this));
            Menu.Children.Add(new MenuItem(item7, this));
            Menu.Children.Add(new MenuItem(item8, this));
            Menu.Children.Add(new MenuItem(item9, this));

            //loginScreen.btnManager.Click += LoginScreen_BtnManager_Click;
            loginScreen.btnLogin.Click += LoginScreen_BtnLogin_Click;

            //var screen = new Menu.MenuList();
            //StackPanelMain.Children.Add(screen);

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DBConfig.EnsureSqlLocalDb())
            {
                var window = new MissingFeatureWindow();
                window.Closed += (s, e2) =>
                {
                    this.Show();
                    MainWindow_Loaded(this, new RoutedEventArgs());
                };
                this.Hide();
                window.ShowDialog();
            }
            loginScreen.txtBoxAccount.Text = oldUsername;
            loginScreen.txtBoxPassword.Password = oldPassword;

        }

        private void LoginScreen_BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            bool checkResult = loginScreen.CheckPassword();
            if (checkResult)
            {
                BUS_Employees busEmp = new BUS_Employees();
                //tblockUsername.Text = loginScreen.txtBoxAccount.Text;
                tblockUsername.Text = busEmp.GetEmpNameByID(loginScreen.txtBoxAccount.Text);
                currentEmpID = loginScreen.txtBoxAccount.Text;
                currentEmpType = busEmp.GetEmpTypeByID(loginScreen.txtBoxAccount.Text);
                oldUsername = loginScreen.txtBoxAccount.Text;
                oldPassword = loginScreen.txtBoxPassword.Password;

                if (currentEmpType == "ET002")

                {

                    BUS_AccessPermissionGroup busAccPerGr = new BUS_AccessPermissionGroup();
                    bool isHavePermission = busAccPerGr.IsHavePermission(currentEmpType, "AP001");
                    if (isHavePermission)
                    {
                        gridLogin.Children.Clear();
                        var screen = new Cashier(this, currentEmpID);
                        StackPanelMain.Children.Add(screen);
                    }
                    else
                    {
                        MessageBox.Show("Bạn không có quyền truy cập chức năng này");
                    }
                }
                else
                {
                    ((ItemMenu)((MenuItem)Menu.Children[0]).DataContext)._Cashier.SetCurrrentUser(currentEmpID);
                    gridLogin.Children.Clear();
                    StackPanelMain.Children.Clear();
                    StackPanelMain.Children.Add(banner);
                }    
            }
            }
        /*private void LoginScreen_BtnSale_Click(object sender, RoutedEventArgs e)
        {
            bool checkResult = loginScreen.CheckPassword();
            if (checkResult)
            {
                tblockUsername.Text = loginScreen.txtBoxAccount.Text;
                BUS_Employees busEmp = new BUS_Employees();
                currentEmpID = tblockUsername.Text;
                currentEmpType = busEmp.GetEmpTypeByID(tblockUsername.Text);

                BUS_AccessPermissionGroup busAccPerGr = new BUS_AccessPermissionGroup();
                bool isHavePermission = busAccPerGr.IsHavePermission(currentEmpType, "AP001");
                if (isHavePermission)
                {
                    gridLogin.Children.Clear();
                    var screen = new Cashier(this, currentEmpID);
                    gridLogin.Children.Add(screen);
                }
                else
                    MessageBox.Show("Bạn không có quyền sử dụng chức năng này!");

            }
        }

        private void LoginScreen_BtnManager_Click(object sender, RoutedEventArgs e)
        {
            bool checkResult = loginScreen.CheckPassword();
            if (checkResult)
            {
                tblockUsername.Text = loginScreen.txtBoxAccount.Text;
                BUS_Employees busEmp = new BUS_Employees();
                currentEmpID = tblockUsername.Text;
                
                ((ItemMenu)((MenuItem)Menu.Children[0]).DataContext)._Cashier.SetCurrrentUser(currentEmpID);
                currentEmpType = busEmp.GetEmpTypeByID(tblockUsername.Text);
                gridLogin.Children.Clear();
                StackPanelMain.Children.Clear();
                StackPanelMain.Children.Add(banner);
            }
        }*/

        private void ChangePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            PopupChangePassword();
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            LogOut();
            currentEmpID = "";
            currentEmpType = "";
        }

        internal void SwitchScreen(object sender)
        {
            var screen = ((UserControl)sender);
            if (screen != null)
            {
                StackPanelMain.Children.Clear();
                if (check == 0)
                {
                    screen.MinHeight = StackPanelMain.ActualHeight - 300;
                    check = 1;
                }
                else
                    screen.MinHeight = StackPanelMain.ActualHeight;
                StackPanelMain.Children.Add(screen);
            }
        }
        internal void SwitchWindow(object sender)
        {
            var screen = ((UserControl)sender);
            if (screen != null)
            {
                StackPanelMain.Children.Clear();
                if (check == 0)
                {
                    screen.MinHeight = StackPanelMain.ActualHeight - 300;
                    check = 1;
                }
                else
                    screen.MinHeight = StackPanelMain.ActualHeight;
                StackPanelMain.Children.Add(screen);     
            }
        }
        internal void SwitchWindow(object sender, int type)
        {
            var screen = ((Cashier)sender);
            screen.LoadData();
            if (screen != null)
            {
                gridLogin.Children.Clear();
                //screen.MinHeight = StackPanelMain.ActualHeight - 20;
                gridLogin.Children.Add(screen);
            }
        }

        public void SwitchBackHome()
        {
            gridLogin.Children.Clear();
            StackPanelMain.Children.Clear();
            StackPanelMain.Children.Add(banner);
        }

        internal void SwitchToDiscount()
        {
            var screen = new Discount.DiscountList();
            gridLogin.Children.Clear();
            StackPanelMain.Children.Clear();
            if (check == 0)
            {
                screen.MinHeight = StackPanelMain.ActualHeight - 300;
                check = 1;
            }
            else
                screen.MinHeight = StackPanelMain.ActualHeight;
            StackPanelMain.Children.Add(screen);
        }

        internal void SwitchToReceipt()
        {
            var screen = new IncomeAndPayment.ReceiptList();
            gridLogin.Children.Clear();
            StackPanelMain.Children.Clear();
            if (check == 0)
            {
                screen.MinHeight = StackPanelMain.ActualHeight - 300;
                check = 1;
            }
            else
                screen.MinHeight = StackPanelMain.ActualHeight;
            StackPanelMain.Children.Add(screen);
        }

        internal void SwitchToMenu()
        {
            var screen = new Menu.MenuList();
            gridLogin.Children.Clear();
            StackPanelMain.Children.Clear();
            if (check == 0)
            {
                screen.MinHeight = StackPanelMain.ActualHeight - 300;
                check = 1;
            }
            else
                screen.MinHeight = StackPanelMain.ActualHeight;
            StackPanelMain.Children.Add(screen);
        }

        internal void LogOut()
        {
            loginScreen.txtBoxAccount.Clear();
            loginScreen.txtBoxPassword.Clear();
            StackPanelMain.Children.Clear();
            gridLogin.Children.Clear();
            gridLogin.Children.Add(loginScreen);
            ((MenuItem)Menu.Children[3]).ExpanderMenu.IsExpanded = false;
            ((MenuItem)Menu.Children[4]).ExpanderMenu.IsExpanded = false;
            ((MenuItem)Menu.Children[5]).ExpanderMenu.IsExpanded = false;
            ((MenuItem)Menu.Children[6]).ExpanderMenu.IsExpanded = false;
        }

        internal void PopupChangePassword()
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Đổi mật khẩu",
                Content = new Account.PopupChangePassword(currentEmpID),
                Width = 460,
                Height = 380,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public string GetCurrentEmpType()
        {
            return currentEmpType;
        }
        public string GetCurrentEmpID()
        {
            return currentEmpID;
        }
        public string GetCurrentEmpName()
        {
            BUS_Employees bus = new BUS_Employees();
            return bus.GetEmpNameByID(this.currentEmpID);
        }
    }
}

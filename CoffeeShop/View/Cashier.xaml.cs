using CoffeeShop.Account;
using CoffeeShop.BUS;
using CoffeeShop.DTO;
using System;
using System.Collections.Generic;
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

namespace CoffeeShop.View
{
    /// <summary>
    /// Interaction logic for Cashier.xaml
    /// </summary>
    public partial class Cashier : UserControl
    {
        MainWindow _context;
        string newReceiptID;
        public class MenuBeverage
        {
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public int cost { get; set; }
            public byte[] image { get; set; }
            public bool isOutOfStock { get; set; }

            public MenuBeverage(string newId, string newName, string newtype, int newCost, bool newState, byte[] newImage)
            {
                id = newId;
                name = newName;
                type = newtype;
                cost = newCost;
                isOutOfStock = newState;
                image = newImage;
            }

            public MenuBeverage(string newId, string newName, int newCost, bool newState, byte[] newImage)
            {
                id = newId;
                name = newName;
                cost = newCost;
                isOutOfStock = newState;
                image = newImage;
            }
        }

        class BillItem
        {
            public string id { get; set; }
            public string name { get; set; }
            public int unitCost { get; set; }
            public int cost { get; set; }
            public int amount { get; set; }
            public BillItem(string newId, string newName, int newCost)
            {
                id = newId;
                name = newName;
                unitCost = newCost;
                cost = newCost;
                amount = 1;
            }
        }

        class FilterButton
        {
            public string id { get; set; }
            public string text { get; set; }

            public FilterButton() { }
            public FilterButton(string newid, string newtext)
            {
                id = newid;
                text = newtext;
            }
        }

        List<MenuBeverage> menuItems;
        List<MenuBeverage> menuItemsDisplay;
        List<BillItem> billItems;
        List<FilterButton> filterButtons;
        string user;
        int total;
        int received;
        float discount;
        public Cashier(MainWindow mainWindow, string userID)
        {
            total = 0;
            received = 0;
            InitializeComponent();
            PrintScreen.Children.Clear();
            _context = mainWindow;
            user = userID;
            tblockUsername.Text = new BUS_Employees().GetEmpNameByID(userID);
            Loaded += Cashier_Loaded;
        }

        private void Cashier_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        public void SetCurrrentUser(string userID)
        {
            user = userID;
            tblockUsername.Text = new BUS_Employees().GetEmpNameByID(userID); ;
        }

        public void LoadData()
        {
            tblockUsername.Text = new BUS_Employees().GetEmpNameByID(user);;
            newReceiptID = "";
            menuItems = new List<MenuBeverage>();
            menuItemsDisplay = new List<MenuBeverage>();
            billItems = new List<BillItem>();

            total = 0;
            received = 0;
            discount = 0;

            tblockChange.Text = "0 VNĐ";
            tblockDiscount.Text = "0";
            tblockDiscountAmount.Text = "0 VNĐ";
            tblockPayAmount.Text = "0 VNĐ";
            tblockTotal.Text = "0 VNĐ";
            tboxAmountReceived.Text = "0";

            BUS_Beverage busBev = new BUS_Beverage();
            DataTable BevsData = busBev.getAllBeverage();
            foreach (DataRow row in BevsData.Rows)
            {
                string id = row["BeverageID"].ToString();
                string name = row["BeverageName"].ToString();
                string type = row["BeverageTypeName"].ToString();
                int price = Int32.Parse(row["Price"].ToString());
                byte[] image = (byte[])row["Link"];
                bool isOutOfStock;
                if (row["IsOutOfStock"].ToString() == "0")
                    isOutOfStock = false;
                else isOutOfStock = true;
                menuItems.Add(new MenuBeverage(id, name, type, price, isOutOfStock, image));
                menuItemsDisplay.Add(new MenuBeverage(id, name, type, price, isOutOfStock, image));
            }

            filterButtons = new List<FilterButton>();
            filterButtons.Add(new FilterButton("Tất cả", "Tất cả"));

            DataTable BevTypesData = busBev.GetBeverageTypeInfo();
            foreach (DataRow row in BevTypesData.Rows)
            {
                string id = row["BeverageTypeID"].ToString();
                string name = row["BeverageTypeName"].ToString();
                filterButtons.Add(new FilterButton(id, name));
            }

            ListViewMenu.ItemsSource = menuItemsDisplay;
            ListViewMenu.Items.Refresh();

            dgBill.ItemsSource = billItems;
            dgBill.Items.Refresh();

            ListFilterButton.ItemsSource = filterButtons;
            ListFilterButton.Items.Refresh();


            BUS_Discount busDiscount = new BUS_Discount();
            DTO_Discount curDiscount = busDiscount.GetCurrentDiscount();
            tblockDiscount.Text = curDiscount.DiscountValue.ToString() + " %";
            discount = curDiscount.DiscountValue;
        }

        private void MenuStatus_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Content = new PopupEditMenuStatus(),
                Width = 450,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
            LoadData();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            BUS_Employees busEmp = new BUS_Employees();
            string typeEmp = busEmp.GetEmpTypeByID(User.ID);
            BUS_AccessPermissionGroup busAccPerGr = new BUS_AccessPermissionGroup();
            bool isHavePermission = busAccPerGr.IsHavePermission(typeEmp, "AP006");
            if (isHavePermission)
            {
                _context.SwitchToMenu();
                PrintScreen.Children.Clear();
            }
            else
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này!");
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            _context.SwitchBackHome();
            PrintScreen.Children.Clear();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            string filterName = ((Button)sender).Tag.ToString();
            if (filterName == "Tất cả")
            {
                menuItemsDisplay = menuItems;
                ListViewMenu.ItemsSource = menuItemsDisplay;
                ListViewMenu.Items.Refresh();
                return;
            }
            menuItemsDisplay = new List<MenuBeverage>();
            foreach (MenuBeverage item in menuItems)
            {
                if (item.type == filterName)
                {
                    menuItemsDisplay.Add(item);
                }
            }
            ListViewMenu.ItemsSource = menuItemsDisplay;
            ListViewMenu.Items.Refresh();
        }

        private void Discount_Click(object sender, RoutedEventArgs e)
        {
            BUS_Employees busEmp = new BUS_Employees();
            string typeEmp = busEmp.GetEmpTypeByID(User.ID);
            BUS_AccessPermissionGroup busAccPerGr = new BUS_AccessPermissionGroup();
            bool isHavePermission = busAccPerGr.IsHavePermission(typeEmp, "AP007");
            if (isHavePermission)
            {
                _context.SwitchToDiscount();
                PrintScreen.Children.Clear();
            }
            else
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này!");
        }

        private void ReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            BUS_Employees busEmp = new BUS_Employees();
            string typeEmp = busEmp.GetEmpTypeByID(User.ID);
            BUS_AccessPermissionGroup busAccPerGr = new BUS_AccessPermissionGroup();
            bool isHavePermission = busAccPerGr.IsHavePermission(typeEmp, "AP001");
            if (isHavePermission)
            {
                _context.SwitchToReceipt();
                PrintScreen.Children.Clear();
            }
            else
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này!");
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            _context.LogOut();
            PrintScreen.Children.Clear();
        }
        private void ChangePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            _context.PopupChangePassword();
        }

        private void btnMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (newReceiptID != "")
            {
                MessageBox.Show($"Hóa đơn này đã thanh toán, mã hóa đơn là {newReceiptID}!\nVui lòng chọn hóa đơn mới để tiếp tục thanh toán!");
                return;
            }

            string id = ((Button)sender).Tag.ToString();
            string newName = "";
            int newCost = 0;
            for (int i = 0; i < menuItemsDisplay.Count; i++)
            {
                if (id == menuItemsDisplay[i].id)
                {
                    if (menuItemsDisplay[i].isOutOfStock)
                    {
                        //MessageBox.Show("Món này đã hết hàng!");
                        return;
                    }
                    newName = menuItemsDisplay[i].name;
                    newCost = menuItemsDisplay[i].cost;
                    break;
                }
            }

            for (int i = 0; i < billItems.Count; i++)
            {
                if (id == billItems[i].id)
                {
                    billItems[i].amount++;
                    billItems[i].cost += billItems[i].unitCost;
                    dgBill.Items.Refresh();
                    total += billItems[i].unitCost;
                    tblockTotal.Text = MoneyToString(total);
                    int discountAmount = (int)(total * discount / 100.0);
                    tblockDiscountAmount.Text = MoneyToString(discountAmount);
                    tblockPayAmount.Text = MoneyToString(total - discountAmount);
                    tblockChange.Text = MoneyToString(received - total + discountAmount);
                    return;
                }
            }

            total += newCost;
            tblockTotal.Text = MoneyToString(total);
            int disAmount = (int)(total * discount / 100.0);
            tblockDiscountAmount.Text = MoneyToString(disAmount);
            tblockPayAmount.Text = MoneyToString(total - disAmount);
            tblockChange.Text = MoneyToString(received - total + disAmount);
            billItems.Add(new BillItem(id, newName, newCost));
            dgBill.Items.Refresh();
        }

        private string MoneyToString(int amount)
        {
            string result = Math.Abs(amount).ToString();
            int start = result.Length % 3;
            if (start == 0)
                start = 3;

            for (int i = start; i < result.Length - 1; i = i + 4)
            {
                result = result.Insert(i, ",");
            }
            if (amount < 0)
                result = "-" + result;
            return result + " VNĐ";
        }

        private void tboxAmountReceived_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tboxAmountReceived.Text))
            {
                return;
            }

            received = Int32.Parse(tboxAmountReceived.Text);
            if (tblockChange != null)
                tblockChange.Text = MoneyToString(received - total - (int)(total * discount / 100.0));
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            string id = ((Button)sender).Tag.ToString();

            for (int i = 0; i < billItems.Count; i++)
            {
                if (id == billItems[i].id)
                {
                    billItems[i].amount++;
                    billItems[i].cost += billItems[i].unitCost;
                    dgBill.Items.Refresh();
                    total += billItems[i].unitCost;
                    tblockTotal.Text = MoneyToString(total);
                    int disAmount = (int)(total * discount / 100.0);
                    tblockDiscountAmount.Text = MoneyToString(disAmount);
                    tblockPayAmount.Text = MoneyToString(total - disAmount);
                    tblockChange.Text = MoneyToString(received - total + disAmount);
                    return;
                }
            }
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            string id = ((Button)sender).Tag.ToString();

            for (int i = 0; i < billItems.Count; i++)
            {
                if (id == billItems[i].id)
                {

                    billItems[i].amount--;
                    billItems[i].cost -= billItems[i].unitCost;
                    total -= billItems[i].unitCost;
                    if (billItems[i].amount == 0)
                    {
                        billItems.RemoveAt(i);
                    }
                    dgBill.Items.Refresh();
                    tblockTotal.Text = MoneyToString(total);
                    int disAmount = (int)(total * discount / 100.0);
                    tblockDiscountAmount.Text = MoneyToString(disAmount);
                    tblockPayAmount.Text = MoneyToString(total - disAmount);
                    tblockChange.Text = MoneyToString(received - total + disAmount);
                    return;
                }
            }
        }

        private void tboxAmountReceived_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.Any(x => Char.IsDigit(x));
            if (e.Text.Contains(" "))
                e.Handled = false;

            if (tboxAmountReceived.Text != "")
            {
                if (tboxAmountReceived.Text.Substring(0, 1) == "0")
                {
                    string[] txt = tboxAmountReceived.Text.Split('0');
                    tboxAmountReceived.Text = txt[1];
                }
            }

        }

        private void tboxAmountReceived_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        public void btnCash_Click(object sender, RoutedEventArgs e)
        {
            if (billItems.Count == 0)
            {
                ///
                return;
            }

            if (tboxAmountReceived.Text == "0" || tboxAmountReceived.Text == "")
            {
                MessageBox.Show("hãy nhập số tiền khách đưa");
                return;
            }

            int _amountReceived = int.Parse(tboxAmountReceived.Text);
            if (_amountReceived % 1000 != 0)
            {
                MessageBox.Show("Số tiền khách đưa phải là số chẵn chia hết cho 1000");
                return;
            }


            if (newReceiptID != "")
            {
                MessageBox.Show($"Hóa đơn này đã thanh toán, mã hóa đơn là {newReceiptID}!");
                return;
            }
            BUS_Discount busDiscount = new BUS_Discount();
            DTO_Discount curDiscount = busDiscount.GetCurrentDiscount();
            string disID = "";
            if (curDiscount.DiscountValue != 0)
                disID = curDiscount.DiscountID;
            DTO_Receipt newReceipt = new DTO_Receipt("", User.ID, disID);

            BUS_Receipt busReceipt = new BUS_Receipt();
            newReceiptID = busReceipt.CreateReceipt(newReceipt);
            if (newReceiptID != "")
            {
                BUS_ReceiptDetail busReceiptDetail = new BUS_ReceiptDetail();
                bool result = true;
                foreach (BillItem item in billItems)
                {
                    DTO_ReceiptDetail newReceiptDetail = new DTO_ReceiptDetail(newReceiptID, item.id, item.amount, item.unitCost);
                    result = result & busReceiptDetail.CreateReceiptDetail(newReceiptDetail);
                }
                if (result)
                {
                    MessageBox.Show("Tạo hóa đơn thành công!");
                }
                else
                {
                    MessageBox.Show("Đã xảy ra lỗi trong quá trình tạo chi tiết hóa đơn!");
                }
            }
            else
            {
                MessageBox.Show("Đã xảy ra lỗi trong quá trình tạo hóa đơn!");
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (newReceiptID != "")
            {
                printing.LoadData(newReceiptID);

                PrintScreen.Children.Add(printing);
            }
            else
                MessageBox.Show("Vui lòng ấn thanh toán trước khi in hóa đơn!");
        }

        private void btnNewReceipt_Click(object sender, RoutedEventArgs e)
        {
            billItems.Clear();
            dgBill.Items.Refresh();
            newReceiptID = "";
            LoadData();
        }
        private void btnCashier_Click(object sender, RoutedEventArgs e)
        {
            PrintScreen.Children.Clear();
        }
    }
}

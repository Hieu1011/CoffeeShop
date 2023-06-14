using CoffeeShop.BUS;
using System;
using System.Collections.Generic;
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
using static CoffeeShop.View.Cashier;

namespace CoffeeShop.View
{
    /// <summary>
    /// Interaction logic for PopupEditMenuStatus.xaml
    /// </summary>
    public partial class PopupEditMenuStatus : UserControl
    {
        List<MenuBeverage> menuItems;
        List<MenuBeverage> menuItemsDisplay;
        List<MenuBeverage> menuItemsResult;
            
        public PopupEditMenuStatus()
        {
            InitializeComponent();
            dgMenu.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            LoadData();
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }

        private void LoadData()
        {
            menuItems = new List<MenuBeverage>();
            menuItemsResult = new List<MenuBeverage>();
            BUS_Beverage busBev = new BUS_Beverage();
            DataTable BevsData = busBev.getAllBeverage();
            foreach (DataRow row in BevsData.Rows)
            {
                string id = row["BeverageID"].ToString();
                string name = row["BeverageName"].ToString();
                int price = Int32.Parse(row["Price"].ToString());
                byte[] image = (byte[])row["Link"];
                bool isOutOfStock;
                if (row["IsOutOfStock"].ToString() == "0")
                    isOutOfStock = false;
                else isOutOfStock = true;
                menuItems.Add(new MenuBeverage(id, name, price, isOutOfStock, image));
                menuItemsResult.Add(new MenuBeverage(id, name, price, isOutOfStock, image));
                menuItemsDisplay = menuItemsResult;
            }

            dgMenu.ItemsSource = menuItemsDisplay;
            dgMenu.Items.Refresh();
        }    

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void DisplayByKeyword()
        {
            string keyword = tbFind.Text.ToLower();
            if (keyword == "")
            {
                menuItemsDisplay = menuItemsResult;
                dgMenu.ItemsSource = menuItemsDisplay;
                dgMenu.Items.Refresh();
                return;
            }
            menuItemsDisplay = new List<MenuBeverage>();
            foreach (MenuBeverage item in menuItemsResult)
            {
                if (item.name.ToLower().Contains(keyword))
                {
                    menuItemsDisplay.Add(item);
                }
            }
            dgMenu.ItemsSource = menuItemsDisplay;
            dgMenu.Items.Refresh();
        }

        private void tbFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            DisplayByKeyword();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            BUS_Beverage busBever = new BUS_Beverage();
            bool flag = true;
            for (int i = 0; i < menuItemsResult.Count; i++)
            {
                if (menuItemsResult[i].isOutOfStock != menuItems[i].isOutOfStock)
                    if (!busBever.ChangeIsOutOfStockValue(menuItemsResult[i].id, menuItemsResult[i].isOutOfStock))
                        flag = false;
            }

            if (flag)
            {
                MessageBox.Show("Đã cập nhật tình trạng các món.");
                Window.GetWindow(this).Close();
            }    
            else
                MessageBox.Show("Đã xảy ra lỗi trong quá trình cập nhật.");
        }
    }
}

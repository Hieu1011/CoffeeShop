using CoffeeShop.BUS;
using CoffeeShop.ViewModel;
using System;
using System.Collections.Generic;
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

namespace CoffeeShop {
    /// <summary>
    /// Interaction logic for MenuItem.xaml
    /// </summary>
    public partial class MenuItem : UserControl
    {
        MainWindow _context;
        public MenuItem(ItemMenu itemMenu, MainWindow context)
        {
            InitializeComponent();
            _context = context;
            ExpanderMenu.Visibility = itemMenu.SubItems == null ? Visibility.Collapsed : Visibility.Visible;
            ListViewItemMenu.Visibility = itemMenu.SubItems == null ? Visibility.Visible : Visibility.Collapsed;
            this.DataContext = itemMenu;
        }
        
        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SubItem item = (SubItem)((ListView)sender).SelectedItem;
            if (ListViewMenu.IsMouseCaptured)
            {
                string perID = item.APID;
                BUS_AccessPermissionGroup busAccPerGr = new BUS_AccessPermissionGroup();
                bool isHavePermission = busAccPerGr.IsHavePermission(_context.GetCurrentEmpType(), perID);
                if (isHavePermission)
                    _context.SwitchScreen(item.Screen);
                else
                    MessageBox.Show("Bạn không có quyền sử dụng chức năng này!");
            }
        }

        private void ListViewItemMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // if(sender.Equals(Cashier))
            ItemMenu item = (ItemMenu)((ListBoxItem)sender).DataContext;
            string perID = item.APID;
            bool isHavePermission = true;
            if (perID.Length == 5)
            {
                BUS_AccessPermissionGroup busAccPerGr = new BUS_AccessPermissionGroup();
                isHavePermission = busAccPerGr.IsHavePermission(_context.GetCurrentEmpType(), perID);
            }
            if (isHavePermission) 
                if (item._Cashier!=null)
                    _context.SwitchWindow(item._Cashier, 1);
                else
                    _context.SwitchWindow(item.Screen);
            else
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này!");
        }

        private void ListViewMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            ListViewMenu.SelectedIndex = -1;
        }
    }
}

using CoffeeShop.DTO;
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

namespace CoffeeShop.Discount
{
    /// <summary>
    /// Interaction logic for PopupDeleteConfirm.xaml
    /// </summary>
    public partial class PopupDeleteConfirm : UserControl
    {
        MainWindow _context;
        String ID, name;
        DateTime time;
        BUS.BUS_Discount bus = new BUS.BUS_Discount();
        public PopupDeleteConfirm()
        {
            InitializeComponent();
        }

        public PopupDeleteConfirm(DTO_Discount discount, MainWindow context)
        {
            InitializeComponent();
            ID = discount.DiscountID;
            name = discount.DiscountName;
            time = DateTime.ParseExact(discount.StartDate, "dd/MM/yyyy", null);
            tblContent.Text = "Dữ liệu về " + discount.DiscountName + " sẽ bị xóa vĩnh viễn.\nBạn chắc chắn muốn xóa?";
            this._context = context;
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (DateTime.Compare(time, DateTime.Now.Date) <= 0)
            {
                MessageBox.Show("Không được xóa ưu đãi đang hoặc đã diễn ra");
            }
            else
            {
                if (bus.deleteDiscount(ID) > 0)
                {
                    MessageBox.Show($"Đã xóa ưu đãi {name}");
                }
                else
                    MessageBox.Show($"Đã có lỗi trong quá trình xóa ưu đãi {name}");
            }
            Window.GetWindow(this).Close();
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}

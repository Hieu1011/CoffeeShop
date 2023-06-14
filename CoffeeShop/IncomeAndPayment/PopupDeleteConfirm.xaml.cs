using CoffeeShop.BUS;
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
using CoffeeShop.DTO;

namespace CoffeeShop.IncomeAndPayment
{
    /// <summary>
    /// Interaction logic for PopupDeleteConfirm.xaml
    /// </summary>
    public partial class PopupDeleteConfirm : UserControl
    {
        BUS_Payment bus = new BUS_Payment();
        string deleteid;
        int type;
        public PopupDeleteConfirm()
        {
            InitializeComponent();
        }
        public PopupDeleteConfirm(string content, string name, int type)
        {
            InitializeComponent();
            this.tblContent.Text = content;
            this.type = type;
            deleteid = name;
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            switch (this.type)
            {
                case 1: /// Delete Account
                    BUS_ReceiptDetail busReceiptDetail = new BUS_ReceiptDetail();
                    bool result1 = busReceiptDetail.DeleteDetailByID(deleteid);

                    if (result1)
                    {
                        BUS_Receipt busReceipt = new BUS_Receipt();
                        if (busReceipt.DeleteReceiptByID(deleteid))
                        {
                            MessageBox.Show($"Đã xóa hóa đơn {deleteid}.");
                            Window.GetWindow(this).Close();
                        }
                        else
                            MessageBox.Show($"Đã xảy ra lỗi trong quá trình xóa hóa đơn!");
                    }
                    else
                    {
                        MessageBox.Show($"Đã xảy ra lỗi trong quá trình xóa chi tiết hóa đơn.");
                    }
                    break;
                case 2:
                    BUS_Payment bus = new BUS_Payment();
                    
                    if (bus.deletePayment(deleteid)>0)
                    {
                            MessageBox.Show($"Đã xóa phiếu chi {deleteid}.");
                    }
                        else
                            MessageBox.Show($"Đã xảy ra lỗi trong quá trình xóa phiếu chi!");
                    Window.GetWindow(this).Close();
                    break;
            }
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}

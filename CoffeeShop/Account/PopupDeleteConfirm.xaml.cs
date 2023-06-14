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
using static CoffeeShop.Account.GroupAccountList;

namespace CoffeeShop.Account
{
    /// <summary>
    /// Interaction logic for PopupDeleteConfirm.xaml
    /// </summary>
    public partial class PopupDeleteConfirm : UserControl
    {
        string deleteid;
        int type;
        public PopupDeleteConfirm()
        {
            InitializeComponent();
        }

        public PopupDeleteConfirm(string content, string name, int type)
        {
            InitializeComponent();
            this.tblockContent.Text = content;
            this.type = type;
            deleteid = name;
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            BUS_Employees busEmp = new BUS_Employees();
            switch (this.type)
            {
                case 1: /// Delete Account
                    if (busEmp.IsDoingAnything(deleteid))
                    {
                        MessageBox.Show($"Không thể xóa tài khoản {deleteid} do tài khoản này đã lập hóa đơn/lập phiếu chi/nhập hàng/xuất hàng.");
                        return;
                    }

                    bool result1 = busEmp.DeleteEmployee(deleteid);

                    if (result1)
                    {
                        MessageBox.Show($"Đã xóa tài khoản {deleteid}.");
                        Window.GetWindow(this).Close();
                    }
                    else
                    {
                        MessageBox.Show($"Đã xảy ra lỗi trong quá trình xóa tài khoản.");
                    }
                    break;

                case 2: /// Delete Account type
                    BUS_AccessPermissionGroup busAccPerGr = new BUS_AccessPermissionGroup();
                    BUS_EmployeeType busEmpType = new BUS_EmployeeType();

                    if (!busAccPerGr.DeleteByEmpTypeID(busEmpType.GetIDByName(deleteid)))
                    MessageBox.Show($"Đã xảy ra lỗi trong quá trình xóa nhóm tài khoản.");

                    int result2 = busEmpType.DeleteEmployeeType(busEmpType.GetIDByName(deleteid));
                    if (result2 == 0)
                    {
                        MessageBox.Show($"Không thể xóa do vẫn còn tài khoản có nhóm tài khoản này.");
                    }
                    else
                    {
                        MessageBox.Show($"Đã xóa nhóm tài khoản {deleteid}.");
                        Window.GetWindow(this).Close();
                    }
                    break;
                case 3: /// Active Account
                    bool result3 = busEmp.SetState(deleteid, true);

                    if (result3)
                    {
                        MessageBox.Show($"Đã kích hoạt tài khoản {deleteid}.");
                        Window.GetWindow(this).Close();
                    }
                    else
                    {
                        MessageBox.Show($"Đã xảy ra lỗi trong quá trình kích hoạt tài khoản.");
                    }
                    break;
                case 4: /// Disable Account
                    bool result4 = busEmp.SetState(deleteid, false);

                    if (result4)
                    {
                        MessageBox.Show($"Đã vô hiệu hóa tài khoản {deleteid}.");
                        Window.GetWindow(this).Close();
                    }
                    else
                    {
                        MessageBox.Show($"Đã xảy ra lỗi trong quá trình vô hiệu hóa tài khoản.");
                    }
                    break;
            }
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}

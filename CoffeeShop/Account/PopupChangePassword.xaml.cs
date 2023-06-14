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

namespace CoffeeShop.Account
{
    /// <summary>
    /// Interaction logic for PopupChangePassword.xaml
    /// </summary>
    public partial class PopupChangePassword : UserControl
    {
        string empId;
        public PopupChangePassword(string id)
        {
            InitializeComponent();
            empId = id;
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.Close();
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            tbOldPassValidation.Text = tbNewPassValidation.Text = tbConfirmPassValidation.Text = "";
            if (tboxOldPass.Password == "")
            {
                //Old password is empty
                tbOldPassValidation.Text = "Mật khẩu cũ không được để trống.";
                return;
            }

            if (tboxNewPass.Password == "")
            {
                //New password is empty
                tbNewPassValidation.Text = "Mật khẩu mới không được để trống.";
                return;
            }

            if (tboxNewPass.Password.Length < 4 || tboxNewPass.Password.Length > 20)
            {
                // New password < 4 characters or > 20 characters
                tbNewPassValidation.Text = "Mật khẩu mới phải từ 4-20 ký tự.";
                return;
            }

            if (tboxConfirmPass.Password == "")
            {
                //Confirm password is empty
                tbConfirmPassValidation.Text = "Xác nhận mật khẩu không được để trống.";
                return;
            }

            if (tboxNewPass.Password.Length != tboxConfirmPass.Password.Length)
            {
                // New password not equal to Confirm password
                tbConfirmPassValidation.Text = "Mật khẩu không khớp.";
                return;
            }
            
            BUS_Employees busAcc = new BUS_Employees();

            if (busAcc.GetPasswordByID(empId) != tboxOldPass.Password)
            {
                // Old password is wrong
                tbOldPassValidation.Text = "Mật khẩu không đúng.";
                return;
            }

            if (busAcc.EditPassword(empId, tboxConfirmPass.Password))
            {
                MessageBox.Show($"Đã đổi mật khẩu của nhân viên {empId}.");
                Window.GetWindow(this).Close();
            }
            else MessageBox.Show($"Đã xảy ra lỗi trong quá trình đổi mật khẩu.");
        }
    }
}

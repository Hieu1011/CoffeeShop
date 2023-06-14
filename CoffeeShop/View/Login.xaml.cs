using CoffeeShop.Account;
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

namespace CoffeeShop.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
            txtBoxPassword.Password = "123456";

        }

        public bool CheckPassword()
        {
            tbIDValidation.Text = tbPassValidation.Text = "";
            string id = txtBoxAccount.Text;
            string pass = txtBoxPassword.Password;
            if (id == "")
            {
                tbIDValidation.Text = "Tên tài khoản không được để trống.";
                return false;
            }    
            if (pass == "")
            {
                tbPassValidation.Text = "Mật khẩu không được để trống.";
                return false;
            }

            BUS_Employees busEmp = new BUS_Employees();
            string truePass = busEmp.GetPasswordByID(id);
            if (truePass == "")
            {
                //show validate can't find account
                tbIDValidation.Text = "Tài khoản không tồn tại hoặc đã bị vô hiệu hóa.";
                return false;
            }

            if (truePass != pass)
            {
                //show validate false account or password
                tbPassValidation.Text = "Mật khẩu không đúng.";
                return false;
            }

            User.ID = id;
            return true;
        }

        public string GetEmpType()
        {
            BUS_Employees busEmp = new BUS_Employees();
            return busEmp.GetEmpTypeByID(User.ID);
        }
    }
}
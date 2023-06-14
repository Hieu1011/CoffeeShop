using CoffeeShop.BUS;
using CoffeeShop.DTO;
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
using static CoffeeShop.Account.PopupAddAccount;

namespace CoffeeShop.Account
{
    /// <summary>
    /// Interaction logic for PopupEditAccount.xaml
    /// </summary>
    public partial class PopupEditAccount : UserControl
    {
        List<EmployeeType> empTypes;

        public PopupEditAccount()
        {
            InitializeComponent();
        }

        public PopupEditAccount(DTO_Employees editEmp)
        {
            InitializeComponent();
            LoadData(editEmp);
        }

        public void LoadData(DTO_Employees editEmp)
        {
            BUS_EmployeeType busEmpType = new BUS_EmployeeType();
            DataTable AccPer = busEmpType.GetEmployeeTypes();
            empTypes = new List<EmployeeType>();
            foreach (DataRow row in AccPer.Rows)
            {
                string id = row["EmployeeTypeID"].ToString();
                string name = row["EmployeeTypeName"].ToString();
                empTypes.Add(new EmployeeType(id, name));
            }
            this.cbEmpType.ItemsSource = empTypes;
            this.cbEmpType.Items.Refresh();

            tboxAccount.Text = editEmp.EmployeeID;
            tboxEmpName.Text = editEmp.EmployeeName;
            tboxPassword.Password = editEmp.Password;
            cbEmpType.Text = editEmp.EmployeeTypeID;

            if (editEmp.EmployeeID == "E001")
            {
                tboxAccount.IsEnabled = false;
                cbEmpType.IsEnabled = false;
            }
            else
            {
                tboxAccount.IsEnabled = true;
                cbEmpType.IsEnabled = true;
            }    
        }    

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            tbPassValidation.Text = tbNameValidation.Text = tbGroupAccountValidation.Text = "";

            if (tboxPassword.Password == "")
            {
                //Password is empty
                tbPassValidation.Text = "Mật khẩu không được để trống.";
                return;
            }

            if (tboxPassword.Password.Length < 4 || tboxPassword.Password.Length > 20)
            {
                //Password < 4 characters or > 20 characters
                tbPassValidation.Text = "Mật khẩu phải từ 4-20 ký tự.";
                return;
            }

            if (tboxEmpName.Text == "")
            {
                //Employee Name is empty
                tbNameValidation.Text = "Tên nhân viên không được để trống.";
                return;
            }

            string newEmpTypeID = "";
            foreach (EmployeeType empType in empTypes)
            {
                if (empType.name == cbEmpType.Text)
                {
                    newEmpTypeID = empType.id;
                    break;
                }
            }

            DTO_Employees newEmp = new DTO_Employees(tboxAccount.Text, tboxEmpName.Text, newEmpTypeID, tboxPassword.Password);

            BUS_Employees busAcc = new BUS_Employees();
            if (busAcc.EditEmployee(newEmp))
            {
                MessageBox.Show($"Đã sửa tài khoản {tboxAccount.Text} của nhân viên {tboxEmpName.Text}.");
                Window.GetWindow(this).Close();
            }
            else MessageBox.Show($"Đã xảy ra lỗi trong quá trình sửa tài khoản.");
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void tboxEmpName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"\p{L}"))
            {
                e.Handled = true;
            }
        }
    }
}

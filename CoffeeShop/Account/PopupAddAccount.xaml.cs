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

namespace CoffeeShop.Account
{
    /// <summary>
    /// Interaction logic for PopupAddAccount.xaml
    /// </summary>
    public partial class PopupAddAccount : UserControl
    {
        public class EmployeeType
        {
            public string id { get; set; }
            public string name { get; set; }

            public EmployeeType() { }
            public EmployeeType(string newid, string newname)
            {
                id = newid;
                name = newname;
            }
        }

        List<EmployeeType> empTypes;

        public PopupAddAccount()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
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

            this.comboboxEmpType.ItemsSource = empTypes;
            this.comboboxEmpType.Items.Refresh();
        }    

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            tbIDValidation.Text = tbPassValidation.Text = tbNameValidation.Text = tbGroupAccountValidation.Text = "";
            //Check if any field is empty
            if (tboxAccName.Text == "")
            {
                //Employee ID is empty
                tbIDValidation.Text = "Tài khoản không được để trống.";
                return;
            }

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
                if (empType.name == comboboxEmpType.Text)
                {
                    newEmpTypeID = empType.id;
                    break;
                }
            }

            if (newEmpTypeID == "")
            {
                //Employee Type not found
                tbGroupAccountValidation.Text = "Nhóm tài khoản không được để trống.";
                return;
            }

            DTO_Employees newEmp = new DTO_Employees(tboxAccName.Text, tboxEmpName.Text, newEmpTypeID, tboxPassword.Password);

            BUS_Employees busAcc = new BUS_Employees();
            if (busAcc.CreateEmployee(newEmp))
            {
                MessageBox.Show($"Đã thêm tài khoản {tboxAccName.Text} cho nhân viên {tboxEmpName.Text}");
                Window.GetWindow(this).Close();
            }     
            else MessageBox.Show($"Tên tài khoản bị trùng với một trong những tài khoản đã được tạo"); 
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

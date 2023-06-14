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
using CoffeeShop.BUS;
using CoffeeShop.DTO;

namespace CoffeeShop.IncomeAndPayment
{
    /// <summary>
    /// Interaction logic for PopupPaymentAdd.xaml
    /// </summary>
    public partial class PopupPaymentAdd : UserControl
    {
        BUS_Payment bus;
        MainWindow mainWindow;
        public PopupPaymentAdd()
        {
            InitializeComponent();
            bus = new BUS_Payment();
        }

        public PopupPaymentAdd(MainWindow main)
        {
            InitializeComponent();
            bus = new BUS_Payment();
            mainWindow = main;
            tbEmployeeName.Text = main.GetCurrentEmpName();
        }
        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            tbMoneyValidation.Text = "";
            if (tbMoney.Text == "")
            {
                tbMoneyValidation.Text = "Số tiền không được để trống.";
                return;
            }
            DTO_Payment dto = new DTO_Payment();
            dto.PaymentID = bus.createID();
            dto.EmployeeID = mainWindow.GetCurrentEmpID();
            dto.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt");
            dto.TotalAmount = float.Parse(tbMoney.Text);
            dto.Description = tbDescription.Text;
            if (bus.createNewPayment(dto) > 0)
            {
                MessageBox.Show($"Đã thêm phiếu chi {dto.PaymentID}.");
                Window.GetWindow(this).Close();
            }
            else
                MessageBox.Show("Đã có lỗi trong quá trình thêm phiếu chi.");
        }

        private void tbMoney_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberCheck.IsNumber(e.Text);
        }
       
    }
}

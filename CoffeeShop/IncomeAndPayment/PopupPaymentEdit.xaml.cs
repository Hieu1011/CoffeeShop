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
using CoffeeShop.BUS;
using CoffeeShop.DTO;
namespace CoffeeShop.IncomeAndPayment
{
    /// <summary>
    /// Interaction logic for PopupPaymentEdit.xaml
    /// </summary>
    public partial class PopupPaymentEdit : UserControl
    {
        BUS_Payment bus = new BUS_Payment();
        string ID;
        public PopupPaymentEdit()
        {
            InitializeComponent();
        }
        public PopupPaymentEdit(DTO_Payment payment)
        {
            InitializeComponent();
            ID = payment.PaymentID;
            DataTable tb = bus.findPaymentbyID(payment.PaymentID);
            foreach (DataRow row in tb.Rows)
            {
                tbMoney.Text = row["TotalAmount"].ToString();
                tbTime.Text = row["Time"].ToString();
                tbEmployeeName.Text = row["EmployeeName"].ToString();
                tbDescription.Text = row["Description"].ToString();

            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            tbMoneyValidation.Text = "";
            if (tbMoney.Text == "")
            {
                tbMoneyValidation.Text = "Số tiền không được để trống.";
                return;
            }
            DTO_Payment Payment = new DTO_Payment();
            Payment.PaymentID = ID;
            Payment.EmployeeID = bus.getEmployeeID(tbEmployeeName.Text);
            Payment.TotalAmount = float.Parse(tbMoney.Text);
            Payment.Time = tbTime.Text;
            Payment.Description = tbDescription.Text;
            if (bus.editPayment(Payment) > 0)
            {
                MessageBox.Show($"Đã sửa phiếu chi {Payment.PaymentID}.");
                Window.GetWindow(this).Close();
            }
            else
                MessageBox.Show("Đã có lỗi trong quá trình sửa phiếu chi.");
        }
    
        private void tbPrice_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberCheck.IsNumber(e.Text);
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}

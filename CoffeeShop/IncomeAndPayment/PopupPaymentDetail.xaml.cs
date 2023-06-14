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
    /// Interaction logic for PopupPaymentDetail.xaml
    /// </summary>
    public partial class PopupPaymentDetail : UserControl
    {
        BUS_Payment bus;
        public PopupPaymentDetail()
        {
            InitializeComponent();
        }
        public PopupPaymentDetail(DTO_Payment dto)
        {
            InitializeComponent();
            bus = new BUS_Payment();
            DataTable tb = bus.findPaymentbyID(dto.PaymentID);
            foreach (DataRow row in tb.Rows)
            {
                tbAmount.Text = row["TotalAmount"].ToString();
                tbTime.Text = row["Time"].ToString();
                tbPaymentID.Text = row["PaymentID"].ToString();
                tbEmployeeName.Text = row["EmployeeName"].ToString();
                tbDescription.Text = row["Description"].ToString();

            }
           
        }
    }
}

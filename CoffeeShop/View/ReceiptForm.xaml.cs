using CoffeeShop.BUS;
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
using static CoffeeShop.IncomeAndPayment.ReceiptDetail;

namespace CoffeeShop.View
{
    /// <summary>
    /// Interaction logic for ReceiptForm.xaml
    /// </summary>
    public partial class ReceiptForm : UserControl
    {
        public ReceiptForm()
        {
            InitializeComponent();
        }

        public void LoadData(string id)
        {
            BUS_ReceiptDetail busReceiptDetail = new BUS_ReceiptDetail();
            DataTable receiptDetail = busReceiptDetail.GetDetailByID(id);

            if (receiptDetail.Rows.Count < 1) return;

            tblockCreater.Text = receiptDetail.Rows[0]["EmployeeName"].ToString();
            tblockReceiptID.Text = id;
            tblockTime.Text = TimeZone.CurrentTimeZone.ToLocalTime((DateTime)receiptDetail.Rows[0]["Time"]).ToString();

            List<DetailItem> details = new List<DetailItem>();
            int total = 0;
            foreach (DataRow row in receiptDetail.Rows)
            {
                string bevName = row["BeverageName"].ToString();
                int amount = Int32.Parse(row["Amount"].ToString());
                int price = Int32.Parse(row["Total"].ToString());
                int unitprice = Int32.Parse(row["UnitPrice"].ToString());
                total += price;
                details.Add(new DetailItem(bevName, amount, unitprice, price));
            }

            dgBill.ItemsSource = details;
            dgBill.Items.Refresh();

            tblockTotal.Text = MoneyToString(total) + " VNĐ";

            BUS_Discount busDis = new BUS_Discount();
            int disValue = Int32.Parse(busDis.findDiscount(receiptDetail.Rows[0]["DiscountID"].ToString()).DiscountValue.ToString());
            tblockDisAmount.Text = MoneyToString((int)(total * (disValue / 100.0))) + " VNĐ"; 
            tblockToTalPay.Text = MoneyToString((int)(total * (1 - disValue / 100.0))) + " VNĐ";
        }

        private string MoneyToString(int amount)
        {
            string result = Math.Abs(amount).ToString();
            int start = result.Length % 3;
            if (start == 0)
                start = 3;

            for (int i = start; i < result.Length - 1; i = i + 4)
            {
                result = result.Insert(i, ",");
            }
            if (amount < 0)
                result = "-" + result;
            return result;
        }
    }
}

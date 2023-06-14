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

namespace CoffeeShop.IncomeAndPayment
{
    /// <summary>
    /// Interaction logic for ReceiptDetail.xaml
    /// </summary>
    public partial class ReceiptDetail : UserControl
    {
        public class DetailItem
        {
            public string bevName { get; set; }
            public int amount { get; set; }
            public int unitprice { get; set; }
            public int price { get; set; }

            public DetailItem(string newBevName, int newAmount, int newUnitPrice, int newPrice)
            {
                bevName = newBevName;
                amount = newAmount;
                unitprice = newUnitPrice;
                price = newPrice;
            }    
        }    
        public ReceiptDetail(string id)
        {
            InitializeComponent();
            dgReceiptDetail.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            LoadData(id);
        }

        void LoadData(string id)
        {
            tbReceiptID.Text = id;

            BUS_ReceiptDetail busReceiptDetail = new BUS_ReceiptDetail();
            DataTable detailData = busReceiptDetail.GetDetailByID(id);

            if (detailData.Rows.Count < 1) return;

            tbDate.Text = TimeZone.CurrentTimeZone.ToLocalTime((DateTime)detailData.Rows[0]["Time"]).ToString("dd/MM/yyyy");
            tbEmployeeName.Text = detailData.Rows[0]["EmployeeName"].ToString();
            List<DetailItem> detailItems = new List<DetailItem>();
            int total = 0;
            foreach (DataRow row in detailData.Rows)
            {
                string bevName = row["BeverageName"].ToString();
                int amount = Int32.Parse(row["Amount"].ToString());
                int price = Int32.Parse(row["Total"].ToString());
                int unitprice = Int32.Parse(row["UnitPrice"].ToString());
                total += price;
                detailItems.Add(new DetailItem(bevName, amount, unitprice, price));
            }

            dgReceiptDetail.ItemsSource = detailItems;
            dgReceiptDetail.Items.Refresh();

            tbTotal.Text = MoneyToString(total);

            BUS_Discount busDis = new BUS_Discount();
            int disValue = Int32.Parse(busDis.findDiscount(detailData.Rows[0]["DiscountID"].ToString()).DiscountValue.ToString());
            tbDiscount.Text = disValue.ToString();

            tbTotalPay.Text = MoneyToString((int)(total * (1 - disValue / 100.0)));
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

        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
    }
}

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
namespace CoffeeShop.Discount
{
    /// <summary>
    /// Interaction logic for PopupDiscountEdit.xaml
    /// </summary>
    public partial class PopupDiscountAdd : UserControl
    {
        BUS_Discount busDiscount;
        MainWindow mainWindow;
        public PopupDiscountAdd()
        {
            InitializeComponent();
            busDiscount = new BUS_Discount();
        }
        public PopupDiscountAdd(MainWindow window)
        {
            InitializeComponent();
            busDiscount = new BUS_Discount();
            mainWindow = window;
        }
        private void tbPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberCheck.IsNumber(e.Text);
        }


        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            tbNameValidation.Text = tbDateValidation.Text = tbRateValidation.Text = "";
            tbDateValidation.HorizontalAlignment = HorizontalAlignment.Right;
            if (tbName.Text == "")
            {
                tbNameValidation.Text = "Tên ưu đãi không được để trống.";
                return;
            }
            if (dpStartDate.SelectedDate == null)
            {
                tbDateValidation.Text = "Ngày bắt đầu không được để trống.";
                tbDateValidation.HorizontalAlignment = HorizontalAlignment.Left;
                return;
            }

            if (DateTime.Compare((DateTime)dpStartDate.SelectedDate, DateTime.Now.Date) <= 0)
            {
                tbDateValidation.Text = "Ngày bắt đầu phải sau ngày hiện tại.";
                tbDateValidation.HorizontalAlignment = HorizontalAlignment.Left;
                return;
            }

            if (dpEndDate.SelectedDate == null)
            {
                tbDateValidation.Text = "Ngày kết thúc không được để trống.";
                return;
            }

            if (DateTime.Compare((DateTime)dpStartDate.SelectedDate, (DateTime)dpEndDate.SelectedDate)  > 0)
            {
                tbDateValidation.Text = "Ngày kết thúc phải sau ngày bắt đầu.";
                return;
            }

            if (tbPrice.Text == "")
            {
                tbRateValidation.Text = "Mức ưu đãi không được để trống.";
                return;
            }

            DTO_Discount discount = new DTO_Discount();
            discount.DiscountID = busDiscount.ID();
            discount.DiscountName = tbName.Text;
            discount.DiscountValue = float.Parse(tbPrice.Text);
            discount.StartDate = dpStartDate.SelectedDate.Value.ToString("dd/MM/yyyy");
            discount.EndDate = dpEndDate.SelectedDate.Value.ToString("dd/MM/yyyy");
            discount.Description = tpDescription.Text;
            if (busDiscount.createNewDiscount(discount) > 0)
            {
                MessageBox.Show($"Đã thêm ưu đãi {tbName.Text}");
                Window.GetWindow(this).Close();
            }
            else
                MessageBox.Show($"Đã có lỗi trong quá trình tạo {tbName.Text}");
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
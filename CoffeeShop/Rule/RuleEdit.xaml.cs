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

namespace CoffeeShop.Rule
{
    /// <summary>
    /// Interaction logic for RuleEdit.xaml
    /// </summary>
    public partial class RuleEdit : UserControl
    {
        static class Constants
        {
            public const string ROWINLIST = "RowInList";
            public const string DAYDELETERECEIPT = "DayDeleteReceipt";
            public const string DAYDELETEPAYMENT = "DayDeletePayment";
            public const string DAYDELETEIMPORT = "DayDeleteImport";
            public const string DAYDELETEEXPORT = "DayDeleteExport";
        }
        MainWindow _context;
        public RuleEdit(MainWindow mainWindow)
        {
            InitializeComponent();
            _context = mainWindow;
            Loaded += LoadData;
        }

        public void LoadData(Object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            BUS_Parameter busParameter = new BUS_Parameter();
            tbMaxNumRow.Text = busParameter.GetValue(Constants.ROWINLIST).ToString();
            tbMaxDaysPayment.Text = busParameter.GetValue(Constants.DAYDELETEPAYMENT).ToString();
            tbMaxDaysBill.Text = busParameter.GetValue(Constants.DAYDELETERECEIPT).ToString();
            tbMaxDaysExport.Text = busParameter.GetValue(Constants.DAYDELETEEXPORT).ToString();
            tbMaxDaysImport.Text = busParameter.GetValue(Constants.DAYDELETEIMPORT).ToString();
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            tbMaxNumRowValidation.Text = tbMaxDaysBillValidation.Text = tbMaxDaysImportValidation.Text = tbMaxDaysExportValidation.Text = tbMaxDaysPaymentValidation.Text = "";

            if (tbMaxNumRow.Text == "")
            {
                ///Validation for max num row empty or == 0
                tbMaxNumRowValidation.Text = "Số dòng tối đa không được để trống.";
                return;
            }
            if (Int32.Parse(tbMaxNumRow.Text) == 0)
            {
                ///Validation for max num row empty or == 0
                tbMaxNumRowValidation.Text = "Số dòng tối đa phải lớn hơn 0.";
                return;
            }

            if (tbMaxDaysBill.Text == "")
            {
                ///Validation for max num row empty or == 0
                tbMaxDaysBillValidation.Text = "Thời gian được sửa/xóa hóa đơn không được để trống.";
                return;
            }
            if (Int32.Parse(tbMaxDaysBill.Text) == 0)
            {
                ///Validation for max num row empty or == 0
                tbMaxDaysBillValidation.Text = "Thời gian được sửa/xóa hóa đơn phải lớn hơn 0.";
                return;
            }

            if (tbMaxDaysImport.Text == "")
            {
                ///Validation for max num row empty or == 0
                tbMaxDaysImportValidation.Text = "Thời gian được sửa/xóa phiếu nhập kho không được để trống.";
                return;
            }
            if (Int32.Parse(tbMaxDaysImport.Text) == 0)
            {
                ///Validation for max num row empty or == 0
                tbMaxDaysImportValidation.Text = "Thời gian được sửa/xóa phiếu nhập kho phải lớn hơn 0.";
                return;
            }

            if (tbMaxDaysExport.Text == "")
            {
                ///Validation for max num row empty or == 0
                tbMaxDaysExportValidation.Text = "Thời gian được sửa/xóa phiếu xuất kho không được để trống.";
                return;
            }
            if (Int32.Parse(tbMaxDaysExport.Text) == 0)
            {
                ///Validation for max num row empty or == 0
                tbMaxDaysExportValidation.Text = "Thời gian được sửa/xóa phiếu xuất kho phải lớn hơn 0.";
                return;
            }

            if (tbMaxDaysPayment.Text == "")
            {
                ///Validation for max num row empty or == 0
                tbMaxDaysPaymentValidation.Text = "Thời gian được sửa/xóa phiếu chi không được để trống.";
                return;
            }
            if (Int32.Parse(tbMaxDaysPayment.Text) == 0)
            {
                ///Validation for max num row empty or == 0
                tbMaxDaysPaymentValidation.Text = "Thời gian được sửa/xóa phiếu chi phải lớn hơn 0.";
                return;
            }

            BUS_Parameter busParameter = new BUS_Parameter();
            bool result = true;
            result = result && busParameter.SetValue(Constants.ROWINLIST, Int32.Parse(tbMaxNumRow.Text));
            result = result && busParameter.SetValue(Constants.DAYDELETEPAYMENT, Int32.Parse(tbMaxDaysPayment.Text));
            result = result && busParameter.SetValue(Constants.DAYDELETERECEIPT, Int32.Parse(tbMaxDaysBill.Text));
            result = result && busParameter.SetValue(Constants.DAYDELETEEXPORT, Int32.Parse(tbMaxDaysExport.Text));
            result = result && busParameter.SetValue(Constants.DAYDELETEIMPORT, Int32.Parse(tbMaxDaysImport.Text));
            if (result)
            {
                MessageBox.Show("Thay đổi quy định thành công!");
            }
            else
            {
                MessageBox.Show("Đã xảy ra lỗi trong quá trình thay đổi quy định!");
            }    
            var screen = new Rule(_context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            var screen = new Rule(_context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }

        private void tbMaxNumRow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void tbMaxNumRow_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.Any(x => Char.IsDigit(x));
            if (e.Text.Contains(" "))
                e.Handled = false;
        }

        private void tbMaxDaysBill_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void tbMaxDaysBill_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.Any(x => Char.IsDigit(x));
            if (e.Text.Contains(" "))
                e.Handled = false;
        }

        private void tbMaxDaysImport_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void tbMaxDaysImport_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.Any(x => Char.IsDigit(x));
            if (e.Text.Contains(" "))
                e.Handled = false;
        }

        private void tbMaxDaysExport_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void tbMaxDaysExport_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.Any(x => Char.IsDigit(x));
            if (e.Text.Contains(" "))
                e.Handled = false;
        }

        private void tbMaxDaysPayment_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void tbMaxDaysPayment_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.Any(x => Char.IsDigit(x));
            if (e.Text.Contains(" "))
                e.Handled = false;
        }
    }
}

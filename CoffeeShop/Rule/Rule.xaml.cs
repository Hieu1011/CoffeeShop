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
    static class Constants
    {
        public const string ROWINLIST = "RowInList";
        public const string DAYDELETERECEIPT = "DayDeleteReceipt";
        public const string DAYDELETEPAYMENT = "DayDeletePayment";
        public const string DAYDELETEIMPORT = "DayDeleteImport";
        public const string DAYDELETEEXPORT = "DayDeleteExport";
    }
    /// <summary>
    /// Interaction logic for Rule.xaml
    /// </summary>
    public partial class Rule : UserControl
    {
        MainWindow _context;
        public Rule(MainWindow mainWindow)
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
            

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var screen = new RuleEdit(_context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }
    }
}

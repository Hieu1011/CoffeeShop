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

namespace CoffeeShop.View
{
    /// <summary>
    /// Interaction logic for Printing.xaml
    /// </summary>
    public partial class Printing : UserControl
    {
        public Printing()
        {
            InitializeComponent();
        }

        public void LoadData(string id)
        {
            ReceiptForm.LoadData(id);
        }    

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog myPrintDialog = new PrintDialog();
            if (myPrintDialog.ShowDialog() == true)
            {
                myPrintDialog.PrintVisual(ReceiptForm, "Hóa đơn");
            }
        }
    }
}

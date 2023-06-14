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
using CoffeeShop.DTO;

namespace CoffeeShop.View
{
    /// <summary>
    /// Interaction logic for PopupDeleteConfirm.xaml
    /// </summary>
    public partial class PopupDeleteConfirm : UserControl
    {
        public PopupDeleteConfirm()
        {
            InitializeComponent();
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}

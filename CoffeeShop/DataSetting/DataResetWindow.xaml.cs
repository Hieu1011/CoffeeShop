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
using System.Windows.Shapes;

namespace CoffeeShop.DataSetting
{
    /// <summary>
    /// Interaction logic for DataResetWindow.xaml
    /// </summary>
    public partial class DataResetWindow : Window
    {
        public DataResetWindow()
        {
            InitializeComponent();

            btnConfirm.Click += BtnConfirm_Click;
            btnClose.Click += BtnClose_Click;
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            ResetData();
        }

        private async Task ResetData()
        {
            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(btnConfirm, true);
            btnConfirm.IsEnabled = false;
            btnClose.IsEnabled = false;

            await Task.Run(DataSettingUtils.ResetData);

            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(btnConfirm, false);
            btnConfirm.IsEnabled = true;
            btnClose.IsEnabled = true;
            this.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

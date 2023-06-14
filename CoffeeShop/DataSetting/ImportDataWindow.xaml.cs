using CoffeeShop.Database;
using CoffeeShop.Database.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CoffeeShop.DataSetting
{
    /// <summary>
    /// Interaction logic for ImportDataWindow.xaml
    /// </summary>
    public partial class ImportDataWindow : Window
    {
        public ImportDataWindow()
        {
            InitializeComponent();

            btnSelect.Click += BtnSelect_Click;
            btnConfirm.Click += BtnConfirm_Click;
            btnClose.Click += BtnClose_Click;
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            var selectedPath = DataSettingUtils.PickFolder();
            txtPath.Text = selectedPath;
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            var rs = MessageBox.Show("Dữ liệu hiện tại sẽ mất để khôi phục dữ liệu cũ.\nThao tác không thể hoàn tác.", "Cảnh báo mất dữ liệu", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (rs == MessageBoxResult.OK)
            {
                ImportData();
            }
        }

        private async Task ImportData()
        {
            var selectedPath = txtPath.Text;
            if (!Directory.Exists(selectedPath))
            {
                MessageBox.Show("Dữ liệu không hợp lệ");
                return;
            }

            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(btnConfirm, true);
            btnConfirm.IsEnabled = false;
            btnClose.IsEnabled = false;

            try
            {
                await Task.Run(() => DataSettingUtils.ImportData(selectedPath));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dữ liệu thiếu hoặc không hợp lệ", "Có lỗi xảy ra", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            

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

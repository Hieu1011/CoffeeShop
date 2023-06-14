using CoffeeShop.Database;
using CsvHelper;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Dynamic;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using CoffeeShop.Database.Models;
using CsvHelper.Configuration;
using System.Data.Entity;
using System.Threading.Tasks;

namespace CoffeeShop.DataSetting
{
    /// <summary>
    /// Interaction logic for ExportDataWindow.xaml
    /// </summary>
    public partial class ExportDataWindow : Window
    {
        public ExportDataWindow()
        {
            InitializeComponent();


            btnSelect.Click += BtnSelect_Click;
            btnConfirm.Click += BtnExport_Click;
            btnClose.Click += BtnClose_Click;
        }


        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            ExportData();
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            var selectedPath = DataSettingUtils.PickFolder();
            txtPath.Text = selectedPath;
        }

        private async Task ExportData()
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

            await Task.Run(() => DataSettingUtils.ExportData(selectedPath));

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

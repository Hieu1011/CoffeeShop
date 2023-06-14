using LiveCharts.Wpf;
using LiveCharts;
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
using LiveCharts.Definitions.Charts;
using CoffeeShop.Database;
using System.Globalization;

namespace CoffeeShop.DataSetting
{
    /// <summary>
    /// Interaction logic for DataSetting.xaml
    /// </summary>
    public partial class DataSetting : UserControl
    {
        MainWindow _context;
        public DataSetting(MainWindow context)
        {
            InitializeComponent();
            this._context = context;

            Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0} KB", chartPoint.Y.ToString("N0", CultureInfo.CreateSpecificCulture("ru-RU")), chartPoint.Participation);
            pieChart.Series.Add(new PieSeries
            {
                Title = "Dữ liệu",
                Fill = Brushes.Blue,
                StrokeThickness = 0,
                DataLabels = true,
                LabelPoint = labelPoint,
                Values = new ChartValues<double> { 1 }
            }); 
            pieChart.Series.Add(new PieSeries
            {
                Title = "Ứng dụng",
                Fill = Brushes.Red,
                StrokeThickness = 0,
                DataLabels = true,
                LabelPoint = labelPoint,
                Values = new ChartValues<double> { 1 }
            });

            IsBusy = false;

            btnExport.Click += BtnExport_Click;
            btnImport.Click += BtnImport_Click;
            btnResetData.Click += BtnResetData_Click;
            Loaded += DataSetting_Loaded;
        }

        public bool IsBusy { get; set; }

        private void BtnResetData_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new DataResetWindow();
            dialog.ShowDialog();
            LoadChartData();
        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ImportDataWindow();
            dialog.ShowDialog();
            LoadChartData();
        }

        private void DataSetting_Loaded(object sender, RoutedEventArgs e)
        {
            LoadChartData();
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ExportDataWindow();
            dialog.ShowDialog();
            LoadChartData();
        }

        private async Task LoadChartData()
        {
            if (IsBusy)
            {
                // Avoid multiple
                return;
            }

            IsBusy = true;
            double sizeApp = await Task.Run<double>(() => DataSettingUtils.GetAppSizeInKB());
            double sizeDB = await Task.Run<double>(() =>
            {
                if (DBConfig.EnsureSqlLocalDb())
                {
                    return DataSettingUtils.GetFileSizeInKB(DBConfig.GetDBFilePath());
                }
                return 0;
            });

            pieChart.Series[0].Values = new ChartValues<double> { sizeDB };
            pieChart.Series[1].Values = new ChartValues<double> { sizeApp - sizeDB };
            IsBusy = false;
        }
    }
}

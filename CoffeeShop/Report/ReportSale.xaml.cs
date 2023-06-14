using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
using LiveCharts;
using LiveCharts.Wpf;

namespace CoffeeShop.Report
{
    /// <summary>
    /// Interaction logic for ReportSale.xaml
    /// </summary>
    public partial class ReportSale : UserControl
    {
        DateTime start;
        DateTime end;
        public ReportSale()
        {
            InitializeComponent(); 
            DataContext = this;

            start = new DateTime(2021, 1, 1, 0, 0, 0);
            end = DateTime.Today;
            LoadSaleChart();
            LoadProfitChart();
            DataContext = this;
        }
        private void LoadSaleChart()
        {
            BUS_Beverage busBev = new BUS_Beverage();
            DataTable bevData = busBev.GetBeverageOrderBySellAmount(start, end);
            int countValue = bevData.Rows.Count;

            Labels = new List<string>();
            ChartValues<int> values = new ChartValues<int>();

            for (int i = 0; i < countValue; i++)
            {
                values.Add(Int32.Parse(bevData.Rows[i]["SellAmount"].ToString()));
                Labels.Add(bevData.Rows[i]["BeverageName"].ToString());
            }    

            SaleChart = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "",
                    Values = values,
                    Fill = Brushes.Orange
                }
            };
            Formatter = value => value.ToString("N");
            saleChart.Height = (Labels.Count + 20) * 20;
            saleChart.Series = SaleChart;

            _axisY_Sale.Labels = Labels;
            _axisX_Sale.LabelFormatter = Formatter;

            saleChart.Update();
            
        }
        private void LoadProfitChart()
        {
            BUS_Beverage busBev = new BUS_Beverage();
            DataTable bevProfitData = busBev.GetBeverageOrderBySellIncome(start, end);
            int countValueProfit = bevProfitData.Rows.Count;

            LabelsProfitChart = new List<string>();
            ChartValues<int> valuesProfit = new ChartValues<int>();

            for (int i = 0; i < countValueProfit; i++)
            {
                valuesProfit.Add(Int32.Parse(bevProfitData.Rows[i]["SellIncome"].ToString()));
                LabelsProfitChart.Add(bevProfitData.Rows[i]["BeverageName"].ToString());
            }

            ProfitChart = new SeriesCollection
            {
                 new RowSeries
                 {
                    Title = "",
                    Values = valuesProfit,
                    Fill = Brushes.Orange
                 }
            };
            FormatterProfitChart = value => value.ToString("N", CultureInfo.CreateSpecificCulture("en-US"));
            profitChart.Height = (LabelsProfitChart.Count + 20) * 20; //Number of labels * 100
            profitChart.Series = ProfitChart;

            _axisY_Profit.Labels = LabelsProfitChart;
            _axisX_Profit.LabelFormatter = FormatterProfitChart;

            profitChart.Update();
        }

        public SeriesCollection SaleChart { get; set; }
        public SeriesCollection ProfitChart { get; set; }
        public List<string> Labels { get; set; }
        public List<string> LabelsProfitChart { get; set; }
        public Func<double, string> Formatter { get; set; }
        public Func<double, string> FormatterProfitChart { get; set; }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            DateTime? datepicker = tbDateStart.SelectedDate;
            if (datepicker.ToString() == "")
                start = new DateTime(2021, 1, 1, 0, 0, 0);
            else
                start = datepicker.Value;

            datepicker = tbDateEnd.SelectedDate;
            if (datepicker.ToString() == "")
                end = DateTime.Today;
            else
                end = datepicker.Value; 

            if (DateTime.Compare(start, end) > 0)
            {
                MessageBox.Show("Ngày bắt đầu phải trước ngày kết thúc.");
                return;
            }    

            LoadSaleChart();
            LoadProfitChart();

        }

        private void btnPrintSale_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog myPrintDialog = new PrintDialog();
            if (myPrintDialog.ShowDialog() == true)
            {
                myPrintDialog.PrintVisual(saleChartBody, "Báo cáo số lượng bán của từng món");
            }
        }
        private void btnPrintProfit_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog myPrintDialog = new PrintDialog();
            if (myPrintDialog.ShowDialog() == true)
            {
                myPrintDialog.PrintVisual(profitChartBody, "Báo cáo doanh thu của từng món");
            }
        }

        private void tbDateStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void tbDateStart_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void tbDateEnd_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void tbDateEnd_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }
    }
}

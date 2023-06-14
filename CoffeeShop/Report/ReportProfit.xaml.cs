using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for ReportProfit.xaml
    /// </summary>
    public partial class ReportProfit : UserControl
    {
        private int months = 12;
        private int dates = 30;
        public ReportProfit()
        {
            InitializeComponent();
            Loaded += LoadData;
            cbYearMonth.SelectedIndex = 1;
            LoadGeneralChartYear();
            generalChart.Update();
            DataContext = this;
        }

        void LoadData(Object sender, RoutedEventArgs e)
        {
            cbYearMonth.SelectedIndex = 1;
            LoadGeneralChartYear();
            generalChart.Update();
            DataContext = this;
        }    
        private void LoadGeneralChartYear()
        {
            GeneralChart = new SeriesCollection();
            LabelGeneralCharts = new List<string>();
            ChartValues<int> Income = new ChartValues<int>();
            ChartValues<int> Outcome = new ChartValues<int>();
            ChartValues<int> Profit = new ChartValues<int>();
            for (int i = 0; i < months; i++)
            {
                LabelGeneralCharts.Add("Tháng " + (i + 1));
                Income.Add(0);
                Outcome.Add(0);
                Profit.Add(0);
            }

            BUS_Receipt busRec = new BUS_Receipt();
            DataTable RecData = busRec.GetTotalIncomeByYear(dpMonthYear.SelectedDate.GetValueOrDefault().Year);
            foreach (DataRow row in RecData.Rows)
            {
                Income[Int32.Parse(row["Month"].ToString()) - 1] = Int32.Parse(row["TotalAfterDis"].ToString());
                Profit[Int32.Parse(row["Month"].ToString()) - 1] += Int32.Parse(row["TotalAfterDis"].ToString());
            }

            BUS_InventoryImport busInvImp = new BUS_InventoryImport();
            DataTable InvImpData = busInvImp.GetTotalAmountByYear(dpMonthYear.SelectedDate.GetValueOrDefault().Year);
            foreach (DataRow row in InvImpData.Rows)
            {
                Outcome[Int32.Parse(row["Month"].ToString()) - 1] = Int32.Parse(row["TotalAmount"].ToString());
                Profit[Int32.Parse(row["Month"].ToString()) - 1] -= Int32.Parse(row["TotalAmount"].ToString());
            }

            BUS_Payment busPay = new BUS_Payment();
            DataTable PayData = busPay.GetTotalAmountByYear(dpMonthYear.SelectedDate.GetValueOrDefault().Year);
            foreach (DataRow row in PayData.Rows)
            {
                Outcome[Int32.Parse(row["Month"].ToString()) - 1] += Int32.Parse(row["Total"].ToString());
                Profit[Int32.Parse(row["Month"].ToString()) - 1] -= Int32.Parse(row["Total"].ToString());
            }

            GeneralChart.Add(new ColumnSeries
            {
                Title = "Doanh thu ",
                Values = Income,
                MaxColumnWidth = 16
            });
            GeneralChart.Add(new ColumnSeries
            {
                Title = "Chi phí",
                Values = Outcome,
                MaxColumnWidth = 16
            });
            GeneralChart.Add(new ColumnSeries
            {
                Title = "Lợi nhuận",
                Values = Profit,
                MaxColumnWidth = 16
            });

            FormatterGeneralCharts = value => value.ToString("N");

            generalChart.AxisX.Clear();
            generalChart.AxisX.Add(new Axis
            {
                Title = "",
                Name = "_axisX_generalChart",
                LabelFormatter = value => "Tháng " + (value + 1).ToString(),
                FontSize = 15,
                Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#6B6158")
            });
            generalChart.Series = GeneralChart;
            generalChart.Width = LabelGeneralCharts.Count * 150;

            _axisY_generalChart.LabelFormatter = value => value.ToString("N");
            _axisX_generalChart.Labels = LabelGeneralCharts;

            generalChartTitle.Text = $"Biểu đồ thống kê tổng quát năm {dpMonthYear.SelectedDate.GetValueOrDefault().Year}";
        }

        private void LoadGeneralChartMonth()
        {
            GeneralChart = new SeriesCollection();

            ChartValues<int> Income = new ChartValues<int>();
            ChartValues<int> Outcome = new ChartValues<int>();
            ChartValues<int> Profit = new ChartValues<int>();

            dates = DateTime.DaysInMonth(dpMonthYear.SelectedDate.GetValueOrDefault().Year, dpMonthYear.SelectedDate.GetValueOrDefault().Month);
            LabelGeneralCharts = new List<string>();
            for (int i = 0; i < dates; i++)
            {
                LabelGeneralCharts.Add("Ngày " + (i + 1));
                Income.Add(0);
                Outcome.Add(0);
                Profit.Add(0);
            }

            BUS_Receipt busRec = new BUS_Receipt();
            DataTable RecData = busRec.GetTotalIncomeByMonth(dpMonthYear.SelectedDate.GetValueOrDefault().Month, dpMonthYear.SelectedDate.GetValueOrDefault().Year);
            foreach (DataRow row in RecData.Rows)
            {
                Income[Int32.Parse(row["Day"].ToString()) - 1] = Int32.Parse(row["TotalAfterDis"].ToString());
                Profit[Int32.Parse(row["Day"].ToString()) - 1] += Int32.Parse(row["TotalAfterDis"].ToString());
            }

            BUS_InventoryImport busInvImp = new BUS_InventoryImport();
            DataTable InvImpData = busInvImp.GetTotalAmountByMonth(dpMonthYear.SelectedDate.GetValueOrDefault().Month, dpMonthYear.SelectedDate.GetValueOrDefault().Year);
            foreach (DataRow row in InvImpData.Rows)
            {
                Outcome[Int32.Parse(row["Day"].ToString()) - 1] = Int32.Parse(row["TotalAmount"].ToString());
                Profit[Int32.Parse(row["Day"].ToString()) - 1] -= Int32.Parse(row["TotalAmount"].ToString());
            }

            BUS_Payment busPay = new BUS_Payment();
            DataTable PayData = busPay.GetTotalAmountByMonth(dpMonthYear.SelectedDate.GetValueOrDefault().Month, dpMonthYear.SelectedDate.GetValueOrDefault().Year);
            foreach (DataRow row in PayData.Rows)
            {
                Outcome[Int32.Parse(row["Day"].ToString()) - 1] += Int32.Parse(row["Total"].ToString());
                Profit[Int32.Parse(row["Day"].ToString()) - 1] -= Int32.Parse(row["Total"].ToString());
            }

            GeneralChart.Add(new ColumnSeries
            {
                Title = "Doanh thu ",
                Values = Income,
                MaxColumnWidth = 16
            });
            GeneralChart.Add(new ColumnSeries
            {
                Title = "Chi phí",
                Values = Outcome,
                MaxColumnWidth = 16
            });
            GeneralChart.Add(new ColumnSeries
            {
                Title = "Lợi nhuận",
                Values = Profit,
                MaxColumnWidth = 16
            });
            //FormatterGeneralCharts = value => value.ToString("N");
            generalChart.AxisX.Clear();
            generalChart.AxisX.Add(new Axis
            {
                Title = "",
                LabelFormatter = value => "Ngày " + (value + 1).ToString(),
                FontSize = 15,
                Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#6B6158")
            });
            generalChart.Series = GeneralChart;
            generalChart.Width = LabelGeneralCharts.Count * 80;
            generalChartTitle.Text = $"Biểu đồ thống kê tổng quát {dpMonthYear.SelectedDate.GetValueOrDefault().Month}/{dpMonthYear.SelectedDate.GetValueOrDefault().Year}";
        }

        public SeriesCollection GeneralChart { get; set; }
        public List<string> LabelGeneralCharts { get; set; }
        public Func<int, string> FormatterGeneralCharts { get; set; }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            if (cbYearMonth.Text == "Theo tháng")
            {
                LoadGeneralChartMonth();
                generalChart.Update();
            } else
            {
                LoadGeneralChartYear();
                generalChart.Update();
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            generalChartScroll.ScrollToLeftEnd();
            PrintDialog myPrintDialog = new PrintDialog();
            if (myPrintDialog.ShowDialog() == true)
            {
                myPrintDialog.PrintVisual(generalChartBody, "Báo cáo tổng quan");
            }
        }

        private void cbYearMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string value = ((ComboBoxItem)(((ComboBox)sender).SelectedItem)).Tag.ToString();
            dpMonthYear.SelectedDate = DateTime.Now;
        }
    }
}

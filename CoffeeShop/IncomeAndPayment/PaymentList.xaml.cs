using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CoffeeShop.BUS;
using CoffeeShop.DTO;
using System.Data;

namespace CoffeeShop.IncomeAndPayment
{
    /// <summary>
    /// Interaction logic for PaymentList.xaml
    /// </summary>
    public partial class PaymentList : UserControl
    {
        MainWindow _context;
        BUS_Payment bus;
        bool find = false;
        int maxNumpage;
        int numRow;
        int currentNumpage;
        int limitRow;
        public PaymentList()
        {
            InitializeComponent();
            Loaded += LoadData;
            dgPayment.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            bus = new BUS_Payment();
            currentNumpage = 1;
        }
        public PaymentList(MainWindow window)
        {
            InitializeComponent();
            Loaded += LoadData;
            dgPayment.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            bus = new BUS_Payment();
            _context = window;
            currentNumpage = 1;
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
        public void LoadData(Object sender, RoutedEventArgs e)
        {
            loaddata();
        }
        void loaddata()
        {
            BUS_Parameter busParameter = new BUS_Parameter();
            limitRow = busParameter.GetValue("RowInList");
            var list = new ObservableCollection<DTO_Payment>();
            DataTable temp = bus.getAllPayment();
            int rowNumber = Int32.Parse(tbNumPage.Text);
            int count = 1;
            foreach (DataRow row in temp.Rows)
            {
                string id = row["PaymentID"].ToString();
                float money = float.Parse(row["TotalAmount"].ToString());
                string employeename = row["EmployeeName"].ToString();
                string time = row["Time"].ToString();
                if (count >= (rowNumber - 1) * limitRow + 1 && count <= rowNumber * limitRow)
                {
                    list.Add(new DTO_Payment() { PaymentID = id, Time = time, EmployeeID = employeename, TotalAmount = money });
                    count++;
                }
                else count++;
            }
            numRow = temp.Rows.Count;
            dgPayment.ItemsSource = list;
            setNumPage();
        }
        void setNumPage()
        {
            if (numRow % limitRow == 0)
            {
                maxNumpage = numRow / limitRow;
            }
            else
                maxNumpage = numRow / limitRow + 1;

            lblMaxPage.Content = maxNumpage.ToString();
            if (maxNumpage == 0)
            {
                tbNumPage.Text = "0";
                btnPageNext.IsEnabled = false;
            }
            if (currentNumpage == maxNumpage)
                btnPageNext.IsEnabled = false;
            else
                btnPageNext.IsEnabled = true;
            if (currentNumpage == 1)
                btnPagePre.IsEnabled = false;
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Lập phiếu chi",
                Content = new PopupPaymentAdd(_context),
                Width = 460,
                Height = 420,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            numRow++;
            setNumPage();
            if (maxNumpage < int.Parse(tbNumPage.Text))
                tbNumPage.Text = (int.Parse(tbNumPage.Text)-1).ToString();
            loaddata();
        }

        private void btnWatch_Click(object sender, RoutedEventArgs e)
        {
            DTO_Payment dto = (DTO_Payment)dgPayment.SelectedItem;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Chi tiết phiếu chi",
                Content = new PopupPaymentDetail(dto),
                Height = 440,
                Width = 460,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DTO_Payment row = (DTO_Payment)dgPayment.SelectedItem;
            DateTime importDate = DateTime.ParseExact(row.Time, "dd/MM/yyyy HH:mm:ss tt", null);

            BUS_Parameter busParameter = new BUS_Parameter();
            int limitDay = busParameter.GetValue("DayDeletePayment");

            if ((DateTime.Now - importDate) > TimeSpan.FromDays(limitDay))
            {
                MessageBox.Show($"Không thể chỉnh sửa phiếu đã được tạo cách đây hơn {limitDay} ngày.");
                return;
            }
            DTO_Payment dto = (DTO_Payment)dgPayment.SelectedItem;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Sửa phiếu chi",
                Content = new PopupPaymentEdit(dto),
                Width = 460,
                Height = 420,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            loaddata();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DTO_Payment row = (DTO_Payment)dgPayment.SelectedItem;
            DateTime importDate = DateTime.ParseExact(row.Time, "dd/MM/yyyy HH:mm:ss tt", null);

            BUS_Parameter busParameter = new BUS_Parameter();
            int limitDay = busParameter.GetValue("DayDeletePayment");

            if ((DateTime.Now - importDate) > TimeSpan.FromDays(limitDay))
            {
                MessageBox.Show($"Không thể xóa phiếu đã được tạo cách đây hơn {limitDay} ngày.");
                return;
            }

            DTO_Payment dto = (DTO_Payment)dgPayment.SelectedItem;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Xóa phiếu chi",
                Content = new PopupDeleteConfirm("Dữ liệu về " + dto.PaymentID + " sẽ bị xóa vĩnh viễn.\nBạn chắc chắn muốn xóa?", dto.PaymentID,2),
                Width = 420,
                Height = 210,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            numRow--;
            setNumPage();
            if (maxNumpage < int.Parse(tbNumPage.Text))
                tbNumPage.Text = (int.Parse(tbNumPage.Text) - 1).ToString();
            loaddata();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            if (dpDateStart.SelectedDate.ToString() != "" && dpDateEnd.SelectedDate.ToString() != ""
                && DateTime.Compare((DateTime)dpDateStart.SelectedDate, (DateTime)dpDateEnd.SelectedDate) > 0)
            {
                MessageBox.Show("Ngày bắt đầu phải trước ngày kết thúc.");
                return;
            }
            currentNumpage = 1;
            tbNumPage.Text = "1";
            findPayment();
        }
        void findPayment()
        {
            DateTime timestart = new DateTime(01 / 01 / 2021);
            if (dpDateStart.SelectedDate != null)
            {
                timestart = (DateTime)dpDateStart.SelectedDate;
            }
            DateTime timeend = DateTime.Now;
            if (dpDateEnd.SelectedDate != null)
            {
                timeend = (DateTime)dpDateEnd.SelectedDate;
            }
            var list = new ObservableCollection<DTO_Payment>();
            DataTable temp = bus.findPaymentbyID(tbNameFind.Text);
            int rowNumber = Int32.Parse(tbNumPage.Text);
            int count = 1;
            foreach (DataRow row in temp.Rows)
            {
                try
                {
                    string id = row["PaymentID"].ToString();
                    float money = float.Parse(row["TotalAmount"].ToString());
                    string employeename = row["EmployeeName"].ToString();
                    string time = row["Time"].ToString();
                    //DateTime timefind = DateTime.ParseExact(time, "dd/MM/yyyy", null);
                    DateTime timefind = DateTime.ParseExact(time, "dd/MM/yyyy HH:mm:ss tt", null);
                    if (count >= (rowNumber - 1) * limitRow + 1 && count <= rowNumber * limitRow)
                    {
                        if (DateTime.Compare(timefind, timeend) <= 0 && DateTime.Compare(timefind, timestart) >= 0)
                        {
                            list.Add(new DTO_Payment() { PaymentID = id, Time = time, EmployeeID = employeename, TotalAmount = money });
                            count++;
                        }
                    }
                    else count++;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            dgPayment.ItemsSource = list;
            find = true;
            numRow = temp.Rows.Count;
            setNumPage();
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            currentNumpage = Int32.Parse(tbNumPage.Text);
            if (Int32.Parse(tbNumPage.Text) == 1)
            {
                btnPagePre.IsEnabled = false;
            }
            else
            {
                tbNumPage.Text = (Int32.Parse(tbNumPage.Text) - 1).ToString();
                currentNumpage--;
                btnPageNext.IsEnabled = true;
                if (!find)
                    loaddata();
                else
                    findPayment();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            currentNumpage = Int32.Parse(tbNumPage.Text);
            if (Int32.Parse(tbNumPage.Text)==maxNumpage)
            {
                btnPageNext.IsEnabled = false;
            }
            else
            {
                tbNumPage.Text = (Int32.Parse(tbNumPage.Text) + 1).ToString();
                currentNumpage++;
                btnPagePre.IsEnabled = true;
                if (!find)
                    loaddata();
                else
                    findPayment();
            }
        }

        private void tbNumPage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
            if (e.Key == Key.Enter)
            {
                if (tbNumPage.Text.Length != 0 && int.Parse(tbNumPage.Text)<=maxNumpage && int.Parse(tbNumPage.Text) >0)
                {
                    int newPage = Int32.Parse(tbNumPage.Text);
                    currentNumpage = newPage;
                    if (currentNumpage == 1)
                        btnPagePre.IsEnabled = false;
                    else
                        btnPagePre.IsEnabled = true;
                    if (currentNumpage == maxNumpage)
                        btnPageNext.IsEnabled = false;
                    else
                        btnPageNext.IsEnabled = true;
                    loaddata();
                }
                else
                {
                    MessageBox.Show("Không có trang này!");
                    tbNumPage.Text = currentNumpage.ToString();
                    loaddata();
                    if (!find)
                        loaddata();
                    else
                        return;
                }    
            }    
        }

        private void tbNumPage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.Any(x => Char.IsDigit(x));
            if (e.Text.Contains(" "))
                e.Handled = false;
        }

        private void dpDateStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void dpDateStart_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void dpDateEnd_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void dpDateEnd_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }
    }
}

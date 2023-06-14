using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using CoffeeShop.DTO;
using CoffeeShop.View;
namespace CoffeeShop.Discount
{
    /// <summary>
    /// Interaction logic for DiscountList.xaml
    /// </summary>
    public partial class DiscountList : UserControl
    {
        BUS_Discount bus = new BUS_Discount();
        MainWindow _context;
        bool find = false;
        int maxNumpage;
        int numRow;
        int currentNumpage;
        int limitRow;
        class Discount : DTO_Discount
        {
            public String Status { get; set; }
        }

        public DiscountList()
        {
            InitializeComponent();
            Loaded += LoadData;
            dgDiscount.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            limitRow = 20;
        }
        public DiscountList(MainWindow window)
        {
            InitializeComponent();
            Loaded += LoadData;
            dgDiscount.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            currentNumpage = 1;
            limitRow = 20;
            this._context = window;
        }
        public void LoadData(Object sender, RoutedEventArgs e)
        {
            loadData();
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
        void loadData()
        {
            var list = new ObservableCollection<Discount>();
            DataTable temp = bus.getAllDiscount();
            int rowNumber = Int32.Parse(tbNumPage.Text);
            int count = 1;
            foreach (DataRow row in temp.Rows)
            {
                string name = row["DiscountName"].ToString();
                string id = row["DiscountID"].ToString();
                int value = Int32.Parse(row["DiscountValue"].ToString());
                string startdate = row["startdate"].ToString();
                string enddate = row["enddate"].ToString();
                string status = "";
                string description = row["Description"].ToString();
                DateTime time = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
                if (DateTime.Compare(time, DateTime.Now.Date) >= 0 && DateTime.Compare(DateTime.ParseExact(startdate, "dd/MM/yyyy", null), DateTime.Now.Date) <= 0)
                {
                    status = "Đang diễn ra";
                }
                else if (DateTime.Compare(time, DateTime.Now.Date) < 0)
                {
                    status = "Đã hết hạn";
                }
                else if (DateTime.Compare(DateTime.ParseExact(startdate, "dd/MM/yyyy", null), DateTime.Now.Date) > 0)
                {
                    status = "Sắp diễn ra";
                }
                if (count >= (rowNumber - 1) * limitRow + 1 && count <= rowNumber * limitRow)
                {
                    list.Add(new Discount() { DiscountID = id, DiscountName = name, DiscountValue = value, StartDate = startdate, EndDate = enddate, Status = status, Description = description });
                    count++;
                }
                else count++;
            }
            numRow = temp.Rows.Count;
            dgDiscount.ItemsSource = list;
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
            if(maxNumpage == 0)
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
        void findDiscount(string startdatefind, string enddatefind)
        {
            find = true;
            var list = new ObservableCollection<Discount>();
            DataTable temp = bus.getAllDiscount();
            int count = 1;
            foreach (DataRow row in temp.Rows)
            {
                string name = row["DiscountName"].ToString();
                string id = row["DiscountID"].ToString();
                int value = Int32.Parse(row["DiscountValue"].ToString());
                string startdate = row["startdate"].ToString();
                string enddate = row["enddate"].ToString();
                string status = "";
                DateTime timeend = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
                DateTime timestart = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
                DateTime timeendfind = DateTime.ParseExact(enddatefind, "dd/MM/yyyy", null);
                DateTime timestartfind = DateTime.ParseExact(startdatefind, "dd/MM/yyyy", null);
                int rowNumber = Int32.Parse(tbNumPage.Text);
                DateTime time = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
                if (DateTime.Compare(time, DateTime.Now.Date) >= 0 && DateTime.Compare(DateTime.ParseExact(startdate, "dd/MM/yyyy", null), DateTime.Now.Date) <= 0)
                {
                    status = "Đang diễn ra";
                }
                else if (DateTime.Compare(time, DateTime.Now.Date) < 0)
                {
                    status = "Đã hết hạn";
                }
                else if (DateTime.Compare(DateTime.ParseExact(startdate, "dd/MM/yyyy", null), DateTime.Now.Date) > 0)
                {
                    status = "Sắp diễn ra";
                }
                if ((DateTime.Compare(timeend, timestartfind) < 0) || (DateTime.Compare(timestart, timeendfind) > 0))
                {

                }
                else
                {

                    if (count >= (rowNumber - 1) * limitRow + 1 && count <= rowNumber * limitRow)
                    {
                        list.Add(new Discount() { DiscountID = id, DiscountName = name, DiscountValue = value, StartDate = startdate, EndDate = enddate, Status = status });
                        count++;
                    }
                    else count++;
                }

            }
            numRow = count;
            setNumPage();
            dgDiscount.ItemsSource = list;
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Thêm ưu đãi",
                Content = new PopupDiscountAdd(_context),
                Width = 460,
                Height = 505,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            loadData();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            currentNumpage = 1;
            tbNumPage.Text = "1";
            if (tbDateStart.SelectedDate.ToString() != "" && tbDateEnd.SelectedDate.ToString() != "" 
                && DateTime.Compare((DateTime)tbDateStart.SelectedDate, (DateTime)tbDateEnd.SelectedDate) > 0)
            {
                MessageBox.Show("Ngày bắt đầu phải trước ngày kết thúc.");
                return;
            }

            if (tbDateStart.SelectedDate == null || tbDateEnd.SelectedDate == null)
            {
                return;
            }

            this.findDiscount(tbDateStart.SelectedDate.Value.ToString("dd/MM/yyyy"), tbDateEnd.SelectedDate.Value.ToString("dd/MM/yyyy"));
            //bus.findDiscount1(tbName.Text);
        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DTO_Discount row = (DTO_Discount)dgDiscount.SelectedItem;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Xóa ưu đãi",
                Content = new PopupDeleteConfirm(row, _context),
                Width = 420,
                Height = 210,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            setNumPage();
            if (maxNumpage < int.Parse(tbNumPage.Text))
                tbNumPage.Text = (int.Parse(tbNumPage.Text)-1).ToString();
            loadData();
        }

        private void btnWatch_Click(object sender, RoutedEventArgs e)
        {
            DTO_Discount row = (DTO_Discount)dgDiscount.SelectedItem;
            var rowView = dgDiscount.SelectedItem;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Chi tiết ưu đãi",
                Content = new PopupDiscountDetail(row.DiscountID),
                Width = 460,
                Height = 440,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DTO_Discount row = (Discount)dgDiscount.SelectedItem;
            if(DateTime.Compare(DateTime.ParseExact(row.EndDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),DateTime.Now.Date)<0)
                MessageBox.Show("Không thể sửa ưu đãi đã diễn ra");
            else 
            {
                System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
                ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
                ((MainWindow)App.Current.MainWindow).Effect = objBlur;
                Window window = new Window
                {
                    ResizeMode = ResizeMode.NoResize,
                    WindowStyle = WindowStyle.None,
                    Title = "Sửa ưu đãi",
                    Content = new PopupDiscountEdit(row.DiscountID, row.DiscountName, row.StartDate, row.EndDate, row.DiscountValue.ToString(), row.Description, _context),
                    Width = 460,
                    Height = 505,
                    Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 460) / 2,
                    Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 500) / 2,
                };
                window.ShowDialog();
                ((MainWindow)App.Current.MainWindow).Opacity = 1;
                ((MainWindow)App.Current.MainWindow).Effect = null;
                loadData();
            }
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
                    loadData();
                else
                    findDiscount(tbDateStart.SelectedDate.Value.ToString("dd/MM/yyyy"), tbDateEnd.SelectedDate.Value.ToString("dd/MM/yyyy"));
            }
            
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            currentNumpage = Int32.Parse(tbNumPage.Text);
            if (Int32.Parse(tbNumPage.Text) == maxNumpage)
            {
                btnPageNext.IsEnabled = false;
            }
            else
            {
                tbNumPage.Text = (Int32.Parse(tbNumPage.Text) + 1).ToString();
                currentNumpage++;
                btnPagePre.IsEnabled = true;
                if (!find)
                    loadData();
                else
                    findDiscount(tbDateStart.SelectedDate.Value.ToString("dd/MM/yyyy"), tbDateEnd.SelectedDate.Value.ToString("dd/MM/yyyy"));
            }
            

        }

        private void tbNumPage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
            if (e.Key == Key.Enter)
            {
                if (tbNumPage.Text.Length != 0 && int.Parse(tbNumPage.Text) <= maxNumpage && int.Parse(tbNumPage.Text) > 0)
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
                    loadData();
                }
                else
                {
                    MessageBox.Show("Không có trang này!");
                    tbNumPage.Text = currentNumpage.ToString();
                    if (!find)
                        loadData();
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

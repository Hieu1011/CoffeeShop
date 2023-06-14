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
using CoffeeShop.BUS;
namespace CoffeeShop.Menu
{
    /// <summary>
    /// Interaction logic for PopupBeverageType.xaml
    /// </summary>
    public partial class PopupBeverageType : UserControl
    {
        BUS_BeverageType bus;
        static string check = "";
        public PopupBeverageType()
        {
            InitializeComponent();
            bus = new BUS_BeverageType();
            dgBeverageType.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            loadData();
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
        void loadData()
        {
            dgBeverageType.ItemsSource = bus.getBeverageType();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            DTO_BeverageType dto = new DTO_BeverageType();
            try
            {
                dto = (DTO_BeverageType)dgBeverageType.SelectedItem;
            }
             catch (Exception ex)
            {

            }
            if (dto.BeverageTypeName == null)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn thêm loại món này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    object item = dgBeverageType.SelectedItem;
                    string specs = (VisualTreeHelper.GetChild(dgBeverageType.Columns[0].GetCellContent(item), 0) as TextBox).Text;
                    dto.BeverageTypeName = specs;
                    dto.BeverageTypeID = bus.createID();
                    if (dto.BeverageTypeName.Length == 0)
                        MessageBox.Show("Tên món là bắt buộc.");
                    else
                    {
                        if (bus.createNewBeverageType(dto) > 0)
                            MessageBox.Show("Thêm thành công");
                        else
                            MessageBox.Show("Thêm không thành công");
                    }
                }
            }
            else 
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn sửa loại món này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {

                }
                else
                {
                    if (dto.BeverageTypeName.Length > 0)
                    {
                        if (bus.editBeverageType(dto) > 0)
                            MessageBox.Show("Sửa thành công");
                        else
                            MessageBox.Show("Sửa không thành công");
                    }
                    else
                        MessageBox.Show("Tên loại món là bắt buộc");
                }
            }
            loadData();
            
        }
        
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa loại món này?", "Xác nhận nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {

            }
            else
            {
                DTO_BeverageType dto = (DTO_BeverageType)dgBeverageType.SelectedItem;
                if (bus.deleteBeverageType(dto.BeverageTypeID) > 0)
                        MessageBox.Show("Xóa thành công");
                    else
                        MessageBox.Show("Xóa không thành công");
               
            }
            loadData();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            if (tbFind.Text.Length == 0)
                loadData();
            else
                dgBeverageType.ItemsSource = bus.findBeverageType(tbFind.Text);
        }

        private void tbType_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}

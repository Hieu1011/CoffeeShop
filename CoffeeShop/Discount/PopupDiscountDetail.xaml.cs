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
using CoffeeShop.BUS;
using CoffeeShop.DTO;
namespace CoffeeShop.Discount
{
    /// <summary>
    /// Interaction logic for PopupDiscountDetail.xaml
    /// </summary>
    public partial class PopupDiscountDetail : UserControl
    {
        BUS_Discount bus;
        DTO_Discount dto;
        public PopupDiscountDetail()
        {
            InitializeComponent();
        }
        public PopupDiscountDetail(string ID)
        {
            InitializeComponent();
            bus = new BUS_Discount();
            dto = bus.findDiscount(ID);
            tbDiscountName.Text = dto.DiscountName;
            tbStartDate.Text = dto.StartDate;
            tbEndDate.Text = dto.EndDate;
            tbDiscountRate.Text = dto.DiscountValue.ToString();
            tbDescription.Text = dto.Description;
        }
    }
}

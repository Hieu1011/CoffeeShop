using CoffeeShop.BUS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace CoffeeShop.Inventory
{
    /// <summary>
    /// Interaction logic for PopupEditMaterial.xaml
    /// </summary>
    public partial class PopupEditMaterial : UserControl
    {
        String name;
        public class MaterialObject
        {
            public String Name { get; set; }
            public String Unit { get; set; }
        }
        public PopupEditMaterial()
        {
            InitializeComponent();
        }
        public PopupEditMaterial(String oldName, String unit)
        {
           
            InitializeComponent();
            MaterialObject temp = new MaterialObject();
            temp.Name = oldName;
            temp.Unit = unit;
            this.name= oldName;
            this.DataContext = temp;
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            tbNameValidation.Text = tbUnitValidation.Text = "";
            if (tbName.Text == "")
            {
                tbNameValidation.Text = "Tên nguyên vật liệu, thiết bị không được để trống.";
                return;
            }
            if (tbUnit.Text == "")
            {
                tbUnitValidation.Text = "Đơn vị tính không được để trống.";
                return;
            }
            BUS_Material material = new BUS_Material();
            bool result = material.Update(this.name,tbName.Text, tbUnit.Text);
            if (result)
            {
                MessageBox.Show($"Đã sửa thông tin của {tbName.Text} trong kho.");
                Window.GetWindow(this).Close();
            }
            else 
                MessageBox.Show("Đã có lỗi trong quá trình sửa thông tin của nguyên vật liệu, thiết bị.");
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
        private void tbName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[a-zA-Zàáạảãâầấậẩẫăằắặẳẵèéẹẻẽêềếệểễìíịỉĩòóọỏõôồốộổỗơờớợởỡùúụủũưừứựửữỳýỵỷỹđ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbUnit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[a-zA-Zàáạảãâầấậẩẫăằắặẳẵèéẹẻẽêềếệểễìíịỉĩòóọỏõôồốộổỗơờớợởỡùúụủũưừứựửữỳýỵỷỹđ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
    }
}

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

namespace CoffeeShop.Inventory
{
    /// <summary>
    /// Interaction logic for PopupDeleteConfirm.xaml
    /// </summary>
    public partial class PopupDeleteConfirm : UserControl
    {
        private object _context;
        private string deleteID;
        private string page;
        public PopupDeleteConfirm()
        {
            InitializeComponent();
        }
        public PopupDeleteConfirm(object context, string deleteID)
        {
            InitializeComponent();
            this._context = context;
            this.deleteID = deleteID;
            switch (_context)
            {
                case InventoryImport import:
                    {
                        //this.tbContent.HorizontalAlignment = HorizontalAlignment.Center;
                        this.tbContent.Text = $"Dữ liệu về phiếu nhập kho {deleteID} sẽ \nbị xóa vĩnh viễn. Bạn chắc chắn muốn xóa?";
                        break;
                    }
                case InventoryExport export:
                    {
                        //this.tbContent.HorizontalAlignment = HorizontalAlignment.Center;
                        this.tbContent.Text = $"Dữ liệu về phiếu xuất kho {deleteID} sẽ \nbị xóa vĩnh viễn. Bạn chắc chắn muốn xóa?";
                        break;
                    }
                case MaterialList list:
                    {
                        //this.tbContent.HorizontalAlignment = HorizontalAlignment.Center;
                        this.tbContent.Text = $"Dữ liệu về vật liệu {deleteID} sẽ \nbị xóa vĩnh viễn. Bạn chắc chắn muốn xóa?";
                        break;
                    }
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            switch(_context)
            {
                case InventoryImport import:
                    {
                        import.Delete(deleteID);
                        Window.GetWindow(this).Close();
                        break;
                    }
                case InventoryExport export:
                    {
                        export.Delete(deleteID);
                        Window.GetWindow(this).Close();
                        break;
                    }
                case MaterialList materList:
                    {
                        materList.Delete(deleteID);
                        Window.GetWindow(this).Close();
                        break;
                    }

            }
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {            
            Window.GetWindow(this).Close();
        }
    }
}

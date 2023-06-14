using CoffeeShop.BUS;
using CoffeeShop.DTO;
using System;
using System.Collections.Generic;
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

namespace CoffeeShop.Account
{
    /// <summary>
    /// Interaction logic for PopupAddGroupAccount.xaml
    /// </summary>
    public partial class PopupAddGroupAccount : UserControl
    {
        public class AccessPermissionName
        {
            public string name { get; set; }
            public AccessPermissionName(string newName)
            {
                name = newName;
            }
        }    
        public PopupAddGroupAccount()
        {
            InitializeComponent();
            LoadData();
        }

        List<AccessPermissionName> dgUnselectedList;
        List<AccessPermissionName> dgSelectedList;
        AccessPermissionName seletedItemLeft;
        AccessPermissionName seletedItemRight;
        DataTable AccPersData;
        public void LoadData()
        {
            BUS_AccessPermission busAccPer = new BUS_AccessPermission();
            AccPersData = busAccPer.GetAccessPermission();
            dgUnselectedList = new List<AccessPermissionName>();
            dgSelectedList = new List<AccessPermissionName>();
            foreach (DataRow row in AccPersData.Rows)
            {
                string name = row["AccessPermissionName"].ToString();
                dgUnselectedList.Add(new AccessPermissionName(name));
            }
            
            this.dgUnselected.ItemsSource = dgUnselectedList;
            this.dgSelected.ItemsSource = dgSelectedList;
            this.dgUnselected.Items.Refresh();
            this.dgSelected.Items.Refresh();
        }    

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            tbGroupAccountValidation.Text = "";
            if (tbName.Text == "")
            {
                // Name of Employee Type is empty
                tbGroupAccountValidation.Text = "Tên nhóm tài khoản không dược để trống.";
                return;
            }
            DTO_EmployeeType newEmpType = new DTO_EmployeeType("id", tbName.Text);
            BUS_EmployeeType busEmpType = new BUS_EmployeeType();
            string newID = busEmpType.CreateEmployeeTypes(newEmpType);
            if (newID == "")
            {
                tbGroupAccountValidation.Text = "Tên nhóm tài khoản này đã được tạo trước đây.";
            }
            else
            {
                BUS_AccessPermissionGroup busAccPerGr = new BUS_AccessPermissionGroup();
                foreach (AccessPermissionName item in dgSelectedList)
                {
                    foreach (DataRow row in AccPersData.Rows)
                    {
                        if (item.name == row[1].ToString())
                        {
                            busAccPerGr.CreateAccessPermissionGroup(new DTO_AccessPermissionGroup(row[0].ToString(), newID));
                            break;
                        }    
                    }    
                }    

                MessageBox.Show($"Nhóm tài khoản {newEmpType.EmployeeTypeName} đã được tạo.");
                Window.GetWindow(this).Close();
            }
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void btnSelectedToRight_Click(object sender, RoutedEventArgs e)
        {
            if (seletedItemLeft == null)
                return;
            foreach (AccessPermissionName accPerName in dgSelectedList)
            {
                if (accPerName.name == seletedItemLeft.name)
                    return;
            }    
            dgSelectedList.Add(seletedItemLeft);
            dgUnselectedList.Remove(seletedItemLeft);
            this.dgUnselected.Items.Refresh();
            this.dgSelected.Items.Refresh();
        }

        private void btnAllToRight_Click(object sender, RoutedEventArgs e)
        {
            foreach (AccessPermissionName item in dgUnselectedList)
            {
                dgSelectedList.Add(item);
            }
            dgUnselectedList.Clear();
            this.dgSelected.Items.Refresh();
            this.dgUnselected.Items.Refresh();
        }

        private void btnSelectedToLeft_Click(object sender, RoutedEventArgs e)
        {
            if (seletedItemRight == null)
                return;
            foreach (AccessPermissionName accPerName in dgUnselectedList)
            {
                if (accPerName.name == seletedItemRight.name)
                    return;
            }
            dgUnselectedList.Add(seletedItemRight);
            dgSelectedList.Remove(seletedItemRight);
            this.dgSelected.Items.Refresh();
            this.dgUnselected.Items.Refresh();
        }

        private void btnAllToLeft_Click(object sender, RoutedEventArgs e)
        {
            foreach (AccessPermissionName item in dgSelectedList)
            {
                dgUnselectedList.Add(item);
            }
            dgSelectedList.Clear();
            this.dgSelected.Items.Refresh();
            this.dgUnselected.Items.Refresh();
        }

        private void dgUnselected_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgUnselected.SelectedItems.Count > 0)
            {
                seletedItemLeft = (AccessPermissionName)dgUnselected.SelectedItems[0];  
            }    
        }

        private void dgSelected_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgSelected.SelectedItems.Count > 0)
            {
                seletedItemRight = (AccessPermissionName)dgSelected.SelectedItems[0];
            }
        }

        private void tbName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"\p{L}"))
            {
                e.Handled = true;
            }
        }
    }
}
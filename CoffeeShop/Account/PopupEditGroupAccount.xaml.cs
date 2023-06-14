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
using static CoffeeShop.Account.GroupAccountList;
using static CoffeeShop.Account.PopupAddGroupAccount;

namespace CoffeeShop.Account
{
    /// <summary>
    /// Interaction logic for PopupEditGroupAccount.xaml
    /// </summary>
    public partial class PopupEditGroupAccount : UserControl
    {
        List<AccessPermissionName> dgUnselectedList;
        List<AccessPermissionName> dgSelectedList;
        AccessPermissionName seletedItemLeft;
        AccessPermissionName seletedItemRight;
        DataTable AccPersData;
        GroupAccountInfo editGrAccInfo;
        public PopupEditGroupAccount()
        {
            InitializeComponent();
        }

        public PopupEditGroupAccount(GroupAccountInfo grAccInfo)
        {
            InitializeComponent();
            LoadData(grAccInfo);
        }    

        public void LoadData(GroupAccountInfo grAccInfo)
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

            editGrAccInfo = grAccInfo;
            tbName.Text = editGrAccInfo.name;
            

            #region change list item
            if (grAccInfo.cashier == true)
            {
                dgSelectedList.Add(new AccessPermissionName(Constants.CASHIER));
                AccessPermissionName item = dgUnselectedList.Find(x => x.name == Constants.CASHIER);
                dgUnselectedList.Remove(item);
            }

            if (grAccInfo.account == true)
            {
                dgSelectedList.Add(new AccessPermissionName(Constants.ACCOUNT));
                AccessPermissionName item = dgUnselectedList.Find(x => x.name == Constants.ACCOUNT);
                dgUnselectedList.Remove(item);
            }

            if (grAccInfo.accountType == true)
            {
                dgSelectedList.Add(new AccessPermissionName(Constants.ACCOUNTTYPE));
                AccessPermissionName item = dgUnselectedList.Find(x => x.name == Constants.ACCOUNTTYPE);
                dgUnselectedList.Remove(item);
            }

            if (grAccInfo.inventory == true)
            {
                dgSelectedList.Add(new AccessPermissionName(Constants.INVENTORY));
                AccessPermissionName item = dgUnselectedList.Find(x => x.name == Constants.INVENTORY);
                dgUnselectedList.Remove(item);
            }

            if (grAccInfo.cost == true)
            {
                dgSelectedList.Add(new AccessPermissionName(Constants.COST));
                AccessPermissionName item = dgUnselectedList.Find(x => x.name == Constants.COST);
                dgUnselectedList.Remove(item);
            }

            if (grAccInfo.menu == true)
            {
                dgSelectedList.Add(new AccessPermissionName(Constants.MENU));
                AccessPermissionName item = dgUnselectedList.Find(x => x.name == Constants.MENU);
                dgUnselectedList.Remove(item);
            }

            if (grAccInfo.discount == true)
            {
                dgSelectedList.Add(new AccessPermissionName(Constants.DISCOUNT));
                AccessPermissionName item = dgUnselectedList.Find(x => x.name == Constants.DISCOUNT);
                dgUnselectedList.Remove(item);
            }

            if (grAccInfo.report == true)
            {
                dgSelectedList.Add(new AccessPermissionName(Constants.REPORT));
                AccessPermissionName item = dgUnselectedList.Find(x => x.name == Constants.REPORT);
                dgUnselectedList.Remove(item);
            }

            if (grAccInfo.rule == true)
            {
                dgSelectedList.Add(new AccessPermissionName(Constants.RULE));
                AccessPermissionName item = dgUnselectedList.Find(x => x.name == Constants.RULE);
                dgUnselectedList.Remove(item);
            }
            #endregion

            this.dgUnselected.ItemsSource = dgUnselectedList;
            this.dgSelected.ItemsSource = dgSelectedList;
            this.dgUnselected.Items.Refresh();
            this.dgSelected.Items.Refresh();
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

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            tbGroupAccountValidation.Text = "";
            if (tbName.Text == "")
            {
                // Name of Employee Type is empty
                tbGroupAccountValidation.Text = "Tên nhóm tài khoản không dược để trống.";
                return;
            }

            BUS_EmployeeType busEmpType = new BUS_EmployeeType();
            string editID = busEmpType.GetIDByName(editGrAccInfo.name);
            bool result = busEmpType.EditEmployeeType(new DTO_EmployeeType(editID, tbName.Text));
            if (!result)
            {
                tbGroupAccountValidation.Text = "Tên nhóm tài khoản bị trùng với một nhóm tài khoản khác.";
                return;
            }    
            BUS_AccessPermissionGroup busAccPerGr = new BUS_AccessPermissionGroup();
            #region Get List Permission
            List<AccessPermissionName> oldPerList = new List<AccessPermissionName>();

            if (editGrAccInfo.cashier)
                oldPerList.Add(new AccessPermissionName(Constants.CASHIER));

            if (editGrAccInfo.account)
                oldPerList.Add(new AccessPermissionName(Constants.ACCOUNT));

            if (editGrAccInfo.accountType)
                oldPerList.Add(new AccessPermissionName(Constants.ACCOUNTTYPE));

            if (editGrAccInfo.inventory)
                oldPerList.Add(new AccessPermissionName(Constants.INVENTORY));

            if (editGrAccInfo.cost)
                oldPerList.Add(new AccessPermissionName(Constants.COST));

            if (editGrAccInfo.menu)
                oldPerList.Add(new AccessPermissionName(Constants.MENU));

            if (editGrAccInfo.discount)
                oldPerList.Add(new AccessPermissionName(Constants.DISCOUNT));

            if (editGrAccInfo.report)
                oldPerList.Add(new AccessPermissionName(Constants.REPORT));

            if (editGrAccInfo.rule)
                oldPerList.Add(new AccessPermissionName(Constants.REPORT));

            #endregion
            // per deleted
            foreach (AccessPermissionName name in oldPerList)
            {
                if (!dgSelectedList.Contains(name))
                {
                    foreach (DataRow row in AccPersData.Rows)
                    {
                        if (name.name == row[1].ToString())
                        {
                            result = busAccPerGr.DeleteAccessPermissionGroup(new DTO_AccessPermissionGroup(row[0].ToString(), editID));
                            if (!result)
                            {
                                MessageBox.Show($"Đã xảy ra lỗi do quyền bị xóa đi trong quá trình sửa nhóm tài khoản {editGrAccInfo.name}.");
                            }
                            break;
                        }
                    }
                }    
            }

            // per added
            foreach (AccessPermissionName item in dgSelectedList)
            {
                if (!oldPerList.Contains(item))
                {
                    foreach (DataRow row in AccPersData.Rows)
                    {
                        if (item.name == row[1].ToString())
                        {
                            result = busAccPerGr.CreateAccessPermissionGroup(new DTO_AccessPermissionGroup(row[0].ToString(), editID));
                            if (!result)
                            {
                                MessageBox.Show($"Đã xảy ra lỗi do quyền được thêm vào trong quá trình sửa nhóm tài khoản {editGrAccInfo.name}.");
                            }
                            break;
                        }
                    }
                }
            }

            MessageBox.Show($"Đã sửa thành công nhóm tài khoản {editGrAccInfo.name}.");
            Window.GetWindow(this).Close();
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
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

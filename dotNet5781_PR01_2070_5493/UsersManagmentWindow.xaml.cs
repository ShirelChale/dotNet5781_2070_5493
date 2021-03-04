using BL.BLAPI;
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
using System.Windows.Shapes;

namespace dotNet5781_PR01_2070_5493
{
    /// <summary>
    /// Interaction logic for UsersManagmentWindow.xaml
    /// </summary>
    public partial class UsersManagmentWindow : Window
    {
        IBL bl;
        BL.BO.User user;
        public UsersManagmentWindow(IBL _bl)
        {
            InitializeComponent();
            this.bl = _bl;
            userDataGrid.ItemsSource = bl.GetAllUsers();
            cbUserName.ItemsSource = bl.getAllpropertyPerUser();
        }


        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateBack();
        }

        private void cbUserName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string name = cbUserName.SelectedItem as string;
            this.userDataGrid.ItemsSource = bl.GetAllUsers(_user => _user.UserName == name);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.userDataGrid.ItemsSource = bl.GetAllUsers();
            this.chbAdminDisplay.IsChecked = false;
        }

        private void btnGivePermission_Click(object sender, RoutedEventArgs e)
        {
            if (this.user != null)
                bl.UpdateUser(this.user.UserName, true);
            this.userDataGrid.ItemsSource = bl.GetAllUsers();
            MessageBox.Show("Permission granted.");
            this.btnGivePermission.IsEnabled = false;
            this.btnRevokePermission.IsEnabled = false;
        }

        private void btnRevokePermission_Click(object sender, RoutedEventArgs e)
        {
            if (this.user != null)
                bl.UpdateUser(this.user.UserName, false);
            this.userDataGrid.ItemsSource = bl.GetAllUsers();
            MessageBox.Show("Permission revoked.");
            this.btnGivePermission.IsEnabled = false;
            this.btnRevokePermission.IsEnabled = false;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
           this.userDataGrid.ItemsSource= bl.GetAllUsers(_user => _user.Admin == true);
        }

        private void userDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.user = userDataGrid.SelectedItem as BL.BO.User;
            btnGivePermission.IsEnabled = true;
            btnRevokePermission.IsEnabled = true;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == false)
                this.userDataGrid.ItemsSource = bl.GetAllUsers();
        }
    }
}

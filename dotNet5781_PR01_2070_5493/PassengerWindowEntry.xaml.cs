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
    /// Interaction logic for PassengerWindowEntry.xaml
    /// </summary>
    public partial class PassengerWindowEntry : Window
    {
        IBL bl;
        string password;
        string userName;
        public PassengerWindowEntry(IBL _bl)
        {
            InitializeComponent();
            this.bl = _bl;
            this.userName = string.Empty;
            this.password = string.Empty;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateBack();
        }

        private void tbUserName_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text.Text == "User name")
                text.Text = string.Empty;
        }

        private void tbPassword_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text.Text == "Password")
                text.Text = string.Empty;

        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (registrationCheck())
                NavigationService.NavigateTo(new PassengerWindow(this.bl, this.userName));
            else
            {
                MessageBox.Show("One or more field are wrong");
                btnSignIn.IsEnabled = false;
                tbPassword.Text = "Password";
                tbUserName.Text = "User name";
            }

        }

        private void signInBtnCheck()
        {
            if (this.userName != string.Empty && this.password != string.Empty)
                btnSignIn.IsEnabled = true;
        }

        private bool registrationCheck()
        {

            BL.BO.User _user = bl.GetUser(this.userName);
            if (_user != null)
            {
                if (_user.Password == this.password)
                    return true;
            }
            return false;
        }

        private void tbUserName_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text.Text == string.Empty)
                text.Text = "User name";
            else
            {
                this.userName = text.Text;
                this.signInBtnCheck();
            }
        }

        private void tbPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text.Text == string.Empty)
                text.Text = "Password";
            else
            {
                this.password = text.Text;
                this.signInBtnCheck();
            }
        }
    }
}

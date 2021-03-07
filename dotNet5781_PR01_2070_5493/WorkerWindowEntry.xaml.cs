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
using BL.BLAPI;

namespace dotNet5781_PR01_2070_5493
{
    /// <summary>
    /// Interaction logic for WorkerWindowEntry.xaml
    /// </summary>
    public partial class WorkerWindowEntry : Window
    {
        IBL bl;
        string password;
        string userName;

        public WorkerWindowEntry(IBL _bl)
        {
            InitializeComponent();
            this.bl = _bl;
            this.userName = string.Empty;
            this.password = string.Empty;
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

        private void tbUserName_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
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
        private void signInBtnCheck()
        {
            if (this.userName != string.Empty && this.password != string.Empty)
                btnSignIn.IsEnabled = true;
        }

        private void tbPassword_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
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

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (registrationCheck())
                NavigationService.NavigateTo(new WorkerWindow(this.bl, this.userName));
            else
            {
                tbPassword.Text = "Password";
                tbUserName.Text = "User name";
                btnSignIn.IsEnabled = false;
            }
        }

        private bool registrationCheck()
        {

            BL.BO.User _user = bl.GetUser(this.userName);
            if (_user != null)
            {
                if (_user.Password == this.password)
                {
                    if (_user.Admin)
                        return true;
                    else
                    {
                        MessageBox.Show("You don't have permission to enter!");
                        return false;
                    }
                }

            }
            MessageBox.Show("One or more field are wrong");
            return false;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateBack();
        }
    }
}

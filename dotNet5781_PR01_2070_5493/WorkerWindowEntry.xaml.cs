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
                this.userName = text.Text;
        }

        private void tbPassword_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text.Text == string.Empty)
                text.Text = "Password";
            else
                this.password = text.Text;
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (registrationCheck())
                NavigationService.NavigateTo(new WorkerWindow(this.bl));
            else
            {
                MessageBox.Show("One or more field are wrong");
                tbPassword.Text = "Password";
                tbUserName.Text = "User name";
            }
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

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateBack();
        }
    }
}

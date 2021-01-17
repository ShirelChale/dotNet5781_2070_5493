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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        IBL bl;
        string password;
        string userName;
        string redoPassword;
        public RegisterWindow(IBL _bl)
        {
            InitializeComponent();
            this.bl = _bl;
            btnSignUp.IsEnabled = false;
            this.password = "";
            this.userName = "";
            this.redoPassword = "";
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
            {
                text.Text = "User name";
                btnSignUp.IsEnabled = false;
                this.userName = "";
            }
            else
                this.userName = text.Text;
        }

        private void tbPassword_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text.Text == string.Empty)
            {
                text.Text = "Password";
                btnSignUp.IsEnabled = false;
                this.password = "";
            }
            else
                this.password = text.Text;
        }

        private void tbPasswordRedo_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text.Text == "Redo password")
                text.Text = string.Empty;
        }

        private void tbPasswordRedo_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text.Text == string.Empty)
            {
                text.Text = "Redo password";
                btnSignUp.IsEnabled = false;
                this.redoPassword = "";
            }
            else
                this.redoPassword = text.Text;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateBack();
        }

        private bool registrationCheck()
        {
            if (this.password == this.redoPassword)
            {
                if (bl.GetUser(this.userName).UserName == null)
                {
                    bl.AddUser(this.userName, this.password);
                    return true;
                }
                MessageBox.Show("User name already exist");
                return false;
            }
            MessageBox.Show("Password not match");
            return false;
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            if (registrationCheck())
            {
                MessageBox.Show("Registration secceded!");
                NavigationService.NavigateBack();
            }
            else
            {
                tbPassword.Text = "Password";
                tbUserName.Text = "User name";
                tbPasswordRedo.Text = "Redo password";
            }
        }

        private void chbTermsAgreement_Checked(object sender, RoutedEventArgs e)
        {
            if (this.userName != "" && this.password != "" && this.redoPassword!="")
                btnSignUp.IsEnabled = true;
        }
    }
}

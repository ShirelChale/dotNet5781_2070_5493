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
    /// Interaction logic for PassengerWindow.xaml
    /// </summary>
    public partial class PassengerWindow : Window
    {
        IBL bl;
        public PassengerWindow(IBL _bl, string _userName)
        {
            InitializeComponent();
            this.bl = _bl;
            this.lHelloUser.Content = "Hello " + _userName + "!";
        }


        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateBack();
        }

        private void btnSimulation_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateTo(new SimulationWindow(this.bl));
        }

        private void btnScheduleDisplay_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateTo(new LineTripWindow(this.bl, false));
        }
    }
}

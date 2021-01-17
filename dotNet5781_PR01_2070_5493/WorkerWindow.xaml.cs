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
    /// Interaction logic for WorkerWindow.xaml
    /// </summary>
    public partial class WorkerWindow : Window
    {
        IBL bl;
        public WorkerWindow(IBL _bl)
        {
            InitializeComponent();
            this.bl = _bl;
        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateBack();
        }

        private void btnStationsDisplay_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateTo(new DisplayStationWindow(this.bl)); 
        }

        private void btnLinesDisplay_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateTo(new DisplayLineWindow(this.bl));
        }

        private void btnBusesDisplay_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateTo(new DisplayBusWindow(this.bl));
        }
    }
}

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
    /// Interaction logic for DisplayBusWindow.xaml
    /// </summary>
    public partial class DisplayBusWindow : Window
    {
        IBL bl;

        public DisplayBusWindow(IBL _bl)
        {
            InitializeComponent();
            this.bl = _bl;
            busDataGrid.ItemsSource = bl.GetAllBuses();
        }

       

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateBack();
        }
    }
}

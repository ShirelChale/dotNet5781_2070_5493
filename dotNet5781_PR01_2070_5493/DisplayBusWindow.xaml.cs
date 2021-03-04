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
        BL.BO.Bus bus;
        IEnumerable<BL.BO.Bus> buses;
        public DisplayBusWindow(IBL _bl)
        {
            InitializeComponent();
            this.bl = _bl;
            this.buses = bl.GetAllBuses();
            busDataGrid.ItemsSource = this.buses;
            this.cbLicenseNum.ItemsSource = bl.GetAllpropertiesToBuses("LicenceNum");
            this.cbStatus.ItemsSource = Enum.GetValues(typeof(BL.BO.BusStatus));
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateBack();
        }

        private void btnDeleteBus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.bus != null)
                {
                    bl.DeleteBus(this.bus.LicenceNum);
                    this.busDataGrid.ItemsSource = bl.GetAllBuses();
                    MessageBox.Show("Bus deleted successfully!");
                }

            }
            catch (BL.BO.BadBusException ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void btnUpdateBus_Click(object sender, RoutedEventArgs e)
        {
            if (this.busDataGrid.SelectedItem != null)
            {
                UpdateBusWindow window = new UpdateBusWindow(this.bl, this.bus);
                window.Show();
                window.Closing += Window_Closing;
            }
        }

        private void btnAddBus_Click(object sender, RoutedEventArgs e)
        {
            AddBusWindow window = new AddBusWindow(this.bl);
            window.Show();
            window.Closing += Window_Closing;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            busDataGrid.ItemsSource = bl.GetAllBuses();
            this.btnUpdateBus.IsEnabled = false;
            this.btnDeleteBus.IsEnabled = false;
        }

        private void cbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.buses = bl.GetAllBuses(bus => (int)bus.Status == (int)cbStatus.SelectedItem, this.buses);
            busDataGrid.ItemsSource = this.buses;
        }

        private void dpFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime date = (DateTime)dpFromDate.SelectedDate;
            this.buses = bl.GetAllBuses(bus => bus.FromDate.Date >= date, this.buses);
            busDataGrid.ItemsSource = this.buses;
        }

        private void cbLicenseNum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.buses = bl.GetAllBuses(bus => bus.LicenceNum == (int)cbLicenseNum.SelectedItem, this.buses);
            busDataGrid.ItemsSource = this.buses;
        }

        private void busDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.bus = (BL.BO.Bus)this.busDataGrid.SelectedItem;
            this.btnDeleteBus.IsEnabled = true;
            this.btnUpdateBus.IsEnabled = true;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            busDataGrid.ItemsSource = bl.GetAllBuses();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for DisplayStationWindow.xaml
    /// </summary>
    public partial class DisplayStationWindow : Window
    {
        IBL bl;
        BL.BO.Station station;
        //ObservableCollection<PO.Station> stationsData;
        public DisplayStationWindow(IBL _bl)
        {
            InitializeComponent();
            this.bl = _bl;
            //stationsData = new ObservableCollection<PO.Station>();
            stationDataGrid.ItemsSource = bl.GetAllStations();
            cbStationCode.ItemsSource = bl.GetAllPropertyStations("Code");
            cbStationName.ItemsSource = bl.GetAllPropertyStations("Name");
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateBack();
        }

        private void btStationLins_Click(object sender, RoutedEventArgs e)
        {
            this.station = (sender as Button).DataContext as BL.BO.Station;
            Station_Details_Window window = new Station_Details_Window(this.bl, this.station);
            window.Show();
        }

        private void btnDeleteStation_Click(object sender, RoutedEventArgs e)
        {

            var result = MessageBox.Show("Are you sure you want to delete this station?", "Delete Station", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                if (bl.DeleteStation(this.station))
                    MessageBox.Show("Station deleted successfully");
                else
                    MessageBox.Show("Station not exist");
            }

        }

        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {
            AddStationWindow window = new AddStationWindow(this.bl);
            window.Show();
        }

        private void btnUpdateStation_Click(object sender, RoutedEventArgs e)
        {
            UpdateStationWindow window = new UpdateStationWindow(this.bl, this.station);
            window.Show();
        }

        private void stationDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.station = (sender as DataGrid).SelectedItem as BL.BO.Station;
            btnUpdateStation.IsEnabled = true;
            btnDeleteStation.IsEnabled = true;
        }

        private void cbStationCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selecteStationCode = (int)cbStationCode.SelectedValue;
            if (selecteStationCode != 0)
            {
                stationDataGrid.ItemsSource = bl.GetAllStations(station => station.Code == selecteStationCode);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            stationDataGrid.ItemsSource = bl.GetAllStations();

        }
    }
}

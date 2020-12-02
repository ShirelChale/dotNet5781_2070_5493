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
using System.Windows.Navigation;
using System.Windows.Shapes;
using dotNet5781_02_2070_5493;

namespace dotNet5781_03_2070_5493
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusesLineCollection busLines = new BusesLineCollection();
        private BusesLine currentDisplayBusLine;

        public MainWindow()
        { // Contructor
            InitializeComponent();
            busLines = setBusLinecollection(setStationsList());
            cbBusLines.ItemsSource = busLines;
            cbBusLines.DisplayMemberPath = " BusLine";
            cbBusLines.SelectedIndex = 0;
        }

        /// <summary>
        /// Sets stations list
        /// </summary>
        /// <returns>List of stations</returns>
        private static List<LineStation> setStationsList()
        {
            List<LineStation> stationsList = new List<LineStation>(); // Data
            double _busStationKey = 100000;
            Random randomLandmark = new Random();
            double _latitude;
            double _longitude;

            for (int i = 0; i < 40; i++)
            {
                _latitude = randomLandmark.Next(31, 33) + randomLandmark.NextDouble();
                _longitude = randomLandmark.Next(34, 35) + randomLandmark.NextDouble();
                LineStation newStation = new LineStation(_busStationKey + i, _latitude, _longitude, "");
                stationsList.Add(newStation);
            }
            return stationsList;
        }

        /// <summary>
        /// Sets buses line list
        /// </summary>
        /// <param name="stationsList"></param>
        /// <returns>Collction of buses line</returns>
        private static BusesLineCollection setBusLinecollection(List<LineStation> stationsList)
        {
            BusesLineCollection busCollection = new BusesLineCollection();
            double _busLine = 1;
            AreaNum _area = 0;
            Random randomArea = new Random(0);
            List<LineStation> rode = new List<LineStation>();
            for (int i = 0; i < 10; i++)
            {
                _area = (AreaNum)randomArea.Next(0, 3);
                BusesLine bus = new BusesLine(_busLine + i, stationsList[i],_area);
                busCollection.addLine(bus);
            }
            int j = 9;
            for (int i = 0; i < 10; i++)
            {
                busCollection[j--].addStation(stationsList[i]);
            }
            Random randomLine = new Random(0);
            foreach (LineStation station in stationsList)
            {
                int lineIndex = randomLine.Next(0, 9);
                busCollection[lineIndex].addStation(station);
            }
            return busCollection;
        }

        /// <summary>
        /// Selection Changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbBusLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusLine((cbBusLines.SelectedValue as BusesLine).BusLine);
        }

        /// <summary>
        /// Displays Bus line details
        /// </summary>
        /// <param name="index"></param>
        private void ShowBusLine(double index)
        {
            currentDisplayBusLine = busLines[(int)index - 1];

            UpGrid.DataContext = currentDisplayBusLine;

            lbBusLineStations.DataContext = currentDisplayBusLine.Stations;
        }

    }
}

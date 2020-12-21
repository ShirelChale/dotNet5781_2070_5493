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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;

namespace dotNet5781_04_2070_5493
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Bus> busesData;
        Bus currentBus;
        public MainWindow()
        { // Constructor.
            InitializeComponent();
            this.BusesData = new ObservableCollection<Bus>();
            busSetting(BusesData);
            displayedBusesList.ItemsSource = BusesData;
        }

        public ObservableCollection<Bus> BusesData
        { // Property.
            get => busesData;
            set => busesData = value;
        }

        /// <summary>
        /// Sets 10 buses.
        /// </summary>
        /// <param name="data"></param>
        public void busSetting(ObservableCollection<Bus> data)
        {
            Random rand = new Random();
            DateTime startingDate;
            for (int i = 0; i < 10; i++)
            {
                startingDate = new DateTime(rand.Next(1998, 2020), rand.Next(1, 13), rand.Next(1, 28));
                Bus newBus = new Bus(rand.Next(1000000, 100000000).ToString(),
                   startingDate, rand.Next(1, 20000), rand.Next(1, 1200));
                newBus.DateTreatment = DateTime.Now;
                newBus.KilometersAfterTreatment = rand.Next(1, (int)newBus.TotalKilometers);
                data.Add(newBus);
            }
            // First 3 buses has special conditions according to requirements.
            data[0].Fuel = 10;
            data[1].KilometersAfterTreatment = 19999;
            data[2].DateTreatment.AddYears(1);
        }

        /// <summary>
        /// Driving button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void driveButton_Click(object sender, RoutedEventArgs e)
        {
            currentBus = (sender as Button).DataContext as Bus;
            if (currentBus.Timerworker.IsBusy)
            { // Check if bus is busy.
                MessageBox.Show("Bus number " + currentBus.LicenseNumber + " can't accomplish drive right now.");
                return;
            }
            DriveWindow window = new DriveWindow(currentBus);
            window.Closed += DriveWindow_Closed;
            window.ShowDialog();

        }

        /// <summary>
        /// Drive window closed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DriveWindow_Closed(object sender, EventArgs e)
        {
            // Checks if drive window closed but kilometers amount not exist or was wrong.
            if (currentBus.Counter != -1 && currentBus.Counter != 0)
            {
                currentBus.BusStatusColor = "#FFF98F8F"; // Color change(red).
                currentBus.StopWatch = new DispatcherTimer(new TimeSpan(0, 0, currentBus.Counter),
                    DispatcherPriority.Normal, delegate { }, Application.Current.Dispatcher); // Time for ride.
                currentBus.StopWatch.Start();
                currentBus.BusStatus = status.duringRide;
                currentBus.Timerworker.RunWorkerAsync(); // Run thread.
            }
        }

        /// <summary>
        /// Add button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewBusWindow window = new AddNewBusWindow(busesData);
            window.ShowDialog();
        }

        /// <summary>
        /// Refueling button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refuelButton_Click(object sender, RoutedEventArgs e)
        {
            Bus busToUpdate = (sender as Button).DataContext as Bus;
            busToUpdate.Counter = 13;
            busToUpdate.BusStatusColor = "#FFF98F8F"; // Color change(red).
            busToUpdate.StopWatch = new DispatcherTimer(new TimeSpan(0, 0, busToUpdate.Counter),
                DispatcherPriority.Normal, delegate { }, Application.Current.Dispatcher);
            busToUpdate.StopWatch.Start();
            busToUpdate.BusStatus = status.onRefueling;
            busToUpdate.Timerworker.RunWorkerAsync(); // Run thread.
            return;
        }

        /// <summary>
        /// Line mouse double click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displayedBusesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Bus bus = displayedBusesList.SelectedItem as Bus;
            if (bus != null)
            {
                DisplayWindow window = new DisplayWindow(bus);
                window.Show();
            }
        }
    }
}

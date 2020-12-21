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
        {
            InitializeComponent();
            this.BusesData = new ObservableCollection<Bus>();
            busSetting(BusesData);
            displayedBusesList.ItemsSource = BusesData;
        }

        public ObservableCollection<Bus> BusesData
        {
            get => busesData;
            set => busesData = value;
        }

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
            data[0].Fuel = 10;
            data[1].KilometersAfterTreatment = 19999;
            data[2].DateTreatment.AddYears(1);

        }


        private void driveButton_Click(object sender, RoutedEventArgs e)
        {
            currentBus = (sender as Button).DataContext as Bus;
            if (currentBus.Timerworker.IsBusy)
            {
                MessageBox.Show("Bus number " + currentBus.LicenseNumber + " can't accomplish drive right now.");
                return;
            }
            currentBus.BusStatusColor = "#FFF98F8F";
            DriveWindow window = new DriveWindow(currentBus);
            window.Closed += DriveWindow_Closed;
            window.ShowDialog();

        }

        private void DriveWindow_Closed(object sender, EventArgs e)
        {

            if (currentBus.Counter != -1 && currentBus.Counter != 0)
            {
                currentBus.StopWatch = new DispatcherTimer(new TimeSpan(0, 0, currentBus.Counter),
                    DispatcherPriority.Normal, delegate { }, Application.Current.Dispatcher);
                currentBus.StopWatch.Start();
                currentBus.BusStatus = status.duringRide;
                currentBus.Timerworker.RunWorkerAsync();
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewBusWindow window = new AddNewBusWindow(busesData);
            window.ShowDialog();
        }

        private void refuelButton_Click(object sender, RoutedEventArgs e)
        {
            Bus busToUpdate = (sender as Button).DataContext as Bus;
            busToUpdate.Counter = 13;
            busToUpdate.BusStatusColor = "#FFF98F8F";
            busToUpdate.StopWatch = new DispatcherTimer(new TimeSpan(0, 0, busToUpdate.Counter),
                DispatcherPriority.Normal, delegate { }, Application.Current.Dispatcher);
            busToUpdate.StopWatch.Start();
            busToUpdate.BusStatus = status.onRefueling;
            busToUpdate.Timerworker.RunWorkerAsync();
           // MessageBox.Show("Bus number " + busToUpdate.LicenseNumber + " finish and ready");
            return;
        }

        private void displayedBusesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Bus bus = displayedBusesList.SelectedItem as Bus;
            if (bus != null)
            {
                DisplayWindow window = new DisplayWindow(bus);
                window.Show();
            }
        }

        internal static void printMessege(string messege)
        {
            MessageBox.Show(messege);
        }
    }
}

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
using System.Windows.Threading;

namespace dotNet5781_04_2070_5493
{
    /// <summary>
    /// Interaction logic for DisplayWindow.xaml
    /// </summary>
    public partial class DisplayWindow : Window
    {
        Bus displayedBus;
        public DisplayWindow(Bus _displayedBus)
        {
            InitializeComponent();
            displayedBus = _displayedBus;
            DataContext = displayedBus;
        }

        /// <summary>
        /// Refueling button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefueling_Click(object sender, RoutedEventArgs e)
        {
            Bus busToUpdate = (sender as Button).DataContext as Bus;
            busToUpdate.Counter = 12; // Time for refueling.
            busToUpdate.BusStatusColor = "#FFF98F8F"; // Color change (red).
            busToUpdate.StopWatch = new DispatcherTimer(new TimeSpan(0, 0, busToUpdate.Counter),
                DispatcherPriority.Normal, delegate { }, Application.Current.Dispatcher);
            busToUpdate.StopWatch.Start();
            busToUpdate.BusStatus = status.onRefueling;
            busToUpdate.Timerworker.RunWorkerAsync(); // Run thread.
        }

        /// <summary>
        /// Treatment button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTreatment_Click(object sender, RoutedEventArgs e)
        {
            Bus busToUpdate = (sender as Button).DataContext as Bus;
            busToUpdate.Counter = 144; // Time for treatment.
            busToUpdate.BusStatusColor = "#FFF98F8F"; // Color change (red).
            busToUpdate.StopWatch = new DispatcherTimer(new TimeSpan(0, 0, busToUpdate.Counter),
                DispatcherPriority.Normal, delegate { }, Application.Current.Dispatcher);
            busToUpdate.StopWatch.Start();
            busToUpdate.BusStatus = status.onTreatment;
            busToUpdate.Timerworker.RunWorkerAsync(); // Run thread.
        }

    }
}

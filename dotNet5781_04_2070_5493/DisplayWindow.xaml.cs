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

        private void btnRefueling_Click(object sender, RoutedEventArgs e)
        {
            Bus busToUpdate = (sender as Button).DataContext as Bus;
            busToUpdate.Counter = 12;
            busToUpdate.BusStatusColor = "#FFF98F8F";
            busToUpdate.StopWatch = new DispatcherTimer(new TimeSpan(0, 0, busToUpdate.Counter),
                DispatcherPriority.Normal, delegate { }, Application.Current.Dispatcher);
            busToUpdate.StopWatch.Start();
            busToUpdate.BusStatus = status.onRefueling;
            busToUpdate.Timerworker.RunWorkerAsync();
        }

        private void btnTreatment_Click(object sender, RoutedEventArgs e)
        {
            Bus busToUpdate = (sender as Button).DataContext as Bus;
            busToUpdate.Counter = 144;
            busToUpdate.BusStatusColor = "#FFF98F8F";
            busToUpdate.StopWatch = new DispatcherTimer(new TimeSpan(0, 0, busToUpdate.Counter),
                DispatcherPriority.Normal, delegate { }, Application.Current.Dispatcher);
            busToUpdate.StopWatch.Start();
            busToUpdate.BusStatus = status.onTreatment;
            busToUpdate.Timerworker.RunWorkerAsync();
        }
    }
}

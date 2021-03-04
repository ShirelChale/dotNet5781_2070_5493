using BL.BLAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;

namespace dotNet5781_PR01_2070_5493
{
    /// <summary>
    /// Interaction logic for SimulationWindow.xaml
    /// </summary>
    public partial class SimulationWindow : Window
    {
        IBL bl;
        int stationCode;
        string stationName;
        TimeSpan currentTime;
        int speed;
        int hour;
        int minutes;
        int seconds;
        BackgroundWorker simulationTimer;
        Stopwatch stopwatch;
        bool isTimeRun;
        public SimulationWindow(IBL _bl)
        {
            InitializeComponent();
            this.bl = _bl;
            this.currentTime = DateTime.Now.TimeOfDay;
            tbTime.Text = this.currentTime.ToString().Substring(0, 8);
            cbStationCode.ItemsSource = bl.GetAllPropertyStations("Code");
            this.hour = this.currentTime.Hours;
            this.minutes = this.currentTime.Minutes;
            this.seconds = this.currentTime.Seconds;
            this.speed = 1;//1 second. 
            this.simulationTimer = new BackgroundWorker();
            this.simulationTimer.DoWork += SimulationTimer_DoWork;
            this.simulationTimer.ProgressChanged += SimulationTimer_ProgressChanged;
            this.simulationTimer.WorkerReportsProgress = true;
            this.simulationTimer.RunWorkerCompleted += SimulationTimer_RunWorkerCompleted;
            this.isTimeRun = true;
            this.stopwatch = new Stopwatch();
        }

        private void SimulationTimer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.tbHours.IsEnabled = true;
            this.tbMinutes.IsEnabled = true;
            this.tbSeconds.IsEnabled = true;
            this.tbSpeed.IsEnabled = true;
            this.btnStartOrStop.Content = "Start";
            this.hour = DateTime.Now.Hour;
            this.minutes = DateTime.Now.Minute;
            this.seconds = DateTime.Now.Second;
            this.tbHours.Text = string.Empty;
            this.tbMinutes.Text = string.Empty;
            this.tbSeconds.Text = string.Empty;
        }

        private void SimulationTimer_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.currentTime = new TimeSpan(this.currentTime.Hours, this.currentTime.Minutes, this.currentTime.Seconds + this.speed);
            tbTime.Text = this.currentTime.ToString().Substring(0, 8);
            lineTimingDataGrid.ItemsSource = bl.GetAllLineTimingPerStation(this.stationCode, this.currentTime);

        }

        private void SimulationTimer_DoWork(object sender, DoWorkEventArgs e)
        {

            while (this.isTimeRun)
            {
                this.simulationTimer.ReportProgress(1);
                Thread.Sleep(1000); // Sleep for a second.  

            }
            //e.Cancel = true;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateBack();
        }

        private void cbStationCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.stationCode = (int)cbStationCode.SelectedItem;
            }
            catch (NullReferenceException ex)
            {
                return;
            }
            if (this.stationCode.ToString().Length == 6)
            {
                if (bl.GetAllPropertyStations("Code").ToList().Find(_stationCode => (int)_stationCode == this.stationCode) != null)
                {
                    this.btnStartOrStop.IsEnabled = true;
                    tbStationName.Text = bl.GetStation(this.stationCode).Name;
                    this.tbHours.IsEnabled = true;
                    this.tbMinutes.IsEnabled = true;
                    this.tbSeconds.IsEnabled = true;
                    this.tbSpeed.IsEnabled = true;
                }
                else
                {
                    this.cbStationCode.Text = string.Empty;
                    this.tbHours.IsEnabled = false;
                    this.tbMinutes.IsEnabled = false;
                    this.tbSeconds.IsEnabled = false;
                    this.tbSpeed.IsEnabled = false;
                    this.btnStartOrStop.IsEnabled = false;
                }
            }
            return;
        }

        private void cbStationName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.stationName = (string)cbStationCode.SelectedItem;
        }

        private void tbHours_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }

        private void tbMinutes_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }

        private void tbSeconds_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }

        private void tbSpeed_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }

        private void btnStartOrStop_Click(object sender, RoutedEventArgs e)
        {
            if (this.btnStartOrStop.Content.ToString() == "Start")
            {
                this.currentTime = new TimeSpan(this.hour, this.minutes, this.seconds);
                stopwatch.Restart();
                this.tbHours.IsEnabled = false;
                this.tbMinutes.IsEnabled = false;
                this.tbSeconds.IsEnabled = false;
                this.tbSpeed.IsEnabled = false;
                this.btnStartOrStop.Content = "Stop";
                this.isTimeRun = true;
                this.simulationTimer.RunWorkerAsync();
            }
            else
            {
                this.isTimeRun = false;
                this.stopwatch.Stop();
            }

        }


        private void integrityInputCheck(KeyEventArgs e)
        {
            // Allow errows, Back and delete keys:
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Right || e.Key == Key.Left)
                return;
            // Allow only digits:
            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);
            if (char.IsDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return;
            e.Handled = true;
            MessageBox.Show("Only digits alowed!");
        }

        private void tbHours_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (this.tbHours.Text != string.Empty)
                this.hour = int.Parse(this.tbHours.Text);
        }

        private void tbSpeed_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (this.tbSpeed.Text != string.Empty)
                this.speed = int.Parse(this.tbSpeed.Text);
        }

        private void tbMinutes_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (this.tbMinutes.Text != string.Empty)
                this.minutes = int.Parse(this.tbMinutes.Text);
        }

        private void tbSeconds_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (this.tbSeconds.Text != string.Empty)
                this.seconds = int.Parse(this.tbSeconds.Text);
        }

        private void tbHours_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(this.tbHours.Text, out this.hour);
        }

        private void tbMinutes_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(this.tbMinutes.Text, out this.minutes);
        }

        private void tbSeconds_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(this.tbSeconds.Text, out this.seconds);
        }

        private void tbSpeed_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(this.tbSpeed.Text, out this.speed);
        }
    }
}

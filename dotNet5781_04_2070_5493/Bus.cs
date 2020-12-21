using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
//using  System.Windows.Forms;


namespace dotNet5781_04_2070_5493
{
    public enum status { ready, duringRide, onRefueling, onTreatment }
    public class Bus : DependencyObject
    {

        private string licenseNumber;
        private DateTime startingDate;
        private DispatcherTimer stopWatch;
        BackgroundWorker timerworker;
       
        public Bus(string _licenseNumber, DateTime _startingDate,
            double _totalKilometers = 0, double _fuel = 1200)
        { // A constructor.
            this.LicenseNumber = _licenseNumber;
            this.StartingDate = _startingDate;
            this.TotalKilometers = _totalKilometers;
            this.Fuel = _fuel;
            this.BusStatus = status.ready;
            this.KilometerPerRide = 0;
            this.Counter = 0;
            Timerworker = new BackgroundWorker();
            Timerworker.DoWork += Worker_DoWork;
            Timerworker.ProgressChanged += Worker_ProgressChanged;
            Timerworker.WorkerReportsProgress = true;
            Timerworker.RunWorkerCompleted += Timerworker_RunWorkerCompleted;
            
        }
        public Bus()
        {

        }
        // Properties:
        public string LicenseNumber
        {
            get => licenseNumber;
            set => licenseNumber = value;
        }
        public DateTime StartingDate
        {
            get => startingDate;
            set => startingDate = value;

        }
        public status BusStatus
        {
            get { return (status)GetValue(BusStatusProperty); }
            set { SetValue(BusStatusProperty, value); }
        }
        // Using a DependencyProperty as the backing store for BusStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BusStatusProperty =
            DependencyProperty.Register("BusStatus", typeof(status), typeof(Bus), new PropertyMetadata(status.ready));
        public BackgroundWorker Timerworker { get => timerworker; set => timerworker = value; }
        public int Counter { get; set; }
        public int KilometerPerRide { get; set; }

        public double TotalKilometers
        {
            get { return (double)GetValue(TotalKilometersProperty); }
            set { SetValue(TotalKilometersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalKilometers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalKilometersProperty =
            DependencyProperty.Register("TotalKilometers", typeof(double), typeof(Bus), new PropertyMetadata(default(double)));

        public string TimerText
        {
            get { return (string)GetValue(TimerTextProperty); }
            set { SetValue(TimerTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TimerText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimerTextProperty =
            DependencyProperty.Register("TimerText", typeof(string), typeof(Bus), new PropertyMetadata(""));

        public double Fuel
        {
            get { return (double)GetValue(FuelProperty); }
            set { SetValue(FuelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Fuel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FuelProperty =
            DependencyProperty.Register("Fuel", typeof(double), typeof(Bus), new PropertyMetadata(default(double)));

        public DateTime DateTreatment
        {
            get { return (DateTime)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(DateTime), typeof(Bus), new PropertyMetadata(default(DateTime)));

        public double KilometersAfterTreatment
        {
            get { return (double)GetValue(KilometersAfterTreatmentProperty); }
            set { SetValue(KilometersAfterTreatmentProperty, value); }
        }

        public DispatcherTimer StopWatch { get => stopWatch; set => stopWatch = value; }

        // Using a DependencyProperty as the backing store for KilometersAfterTreatment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KilometersAfterTreatmentProperty =
            DependencyProperty.Register("KilometersAfterTreatment", typeof(double), typeof(Bus), new PropertyMetadata(default(double)));


        public string BusStatusColor
        {
            get
            {
             return (string)GetValue(BusStatusColorProperty);
            }
            set { SetValue(BusStatusColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BusStatusColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BusStatusColorProperty =
            DependencyProperty.Register("BusStatusColor", typeof(string), typeof(Bus), new PropertyMetadata(default(string)));



        /// <summary>
        /// Checks if ride can be completed successtfuly.
        /// </summary>
        /// <param name="ride"></param>
        /// <returns> 
        /// 0 - Ride completed successtfuly.
        /// 1 - There's not enough fuel for the ride.
        /// 2 - It's been a year and the bus needs treatment.
        /// 3 - Bus isn't ready for a ride.
        /// </returns>
        public int integrityCheck(double ride)
        {
            if (this.BusStatus != status.ready)
                return 3;
            if (this.Fuel - ride < 0)
                return 1;
            System.TimeSpan resultDate = DateTime.Now.Subtract(this.DateTreatment);
            if (resultDate.TotalDays >= 365 || (this.KilometersAfterTreatment + ride) > 20000)
                return 2;
            return 0;
        }
        public void afterTreatment()
        { // Updates the treatment date.
            this.DateTreatment = DateTime.Now;
            this.KilometersAfterTreatment = 0;
        }
        public void afterRefueling()
        { // Refuel.
            this.Fuel = 1200;
        }

        //--------
        // Thread:
        //--------

        /// <summary>
        /// Thread DoWork
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= this.Counter; i++)
            { // Going down through seconds counter.
                Timerworker.ReportProgress(1);
                Thread.Sleep(1000); // Sleep for a second.
            }

        }
        /// <summary>
        /// Thread ProgressChange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            StopWatch.Interval += new TimeSpan(0, 0, -1); // Reduces 1 second from stop watch.
            string timerText = StopWatch.Interval.ToString();
            this.TimerText = timerText;
        }

        /// <summary>
        /// Thread RunWorkerCompleted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timerworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StopWatch.Stop();
            MessageBox.Show("Bus number " + this.LicenseNumber + " finish and ready"); 
            // Status checks for appropriate operation:
            if (this.BusStatus == status.duringRide)
            {
                this.TotalKilometers += this.KilometerPerRide; // Add ride to kilometer.
                this.Fuel -= this.KilometerPerRide; // Substract fuel of ride.
                this.KilometersAfterTreatment += this.KilometerPerRide; // Add ride to Kilometers After Treatment
            }
            else if (this.BusStatus == status.onRefueling)
                this.afterRefueling(); // Refueling.
            else if (this.BusStatus == status.onTreatment)
                this.afterTreatment(); // Treat
            this.BusStatus = status.ready;
            this.BusStatusColor = ""; // Reset color of line on buses list.
            this.KilometerPerRide = 0;
            this.Counter = 0;
        }

        //---------------
        // End of Thread.
        //---------------

        public override string ToString()
        {
            return this.LicenseNumber;
        }
    }
}

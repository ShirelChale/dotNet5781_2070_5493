using BL.BLAPI;
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

namespace dotNet5781_PR01_2070_5493
{
    /// <summary>
    /// Interaction logic for LineTripWindow.xaml
    /// </summary>
    public partial class LineTripWindow : Window
    {
        IBL bl;
        IEnumerable<BL.BO.LineTrip> lineTrips;
        BL.BO.LineTrip lineTrip;
        List<TimeSpan> time;
        int lineID;
        int lineCode;
        BL.BO.Areas area;
        TimeSpan from;
        TimeSpan to;
        public LineTripWindow(IBL _bl, bool _admin)
        {
            InitializeComponent();
            if (!_admin)
            {
                btnAddLineTrip.Visibility = Visibility.Hidden;
                btnDeleteLineTrip.Visibility = Visibility.Hidden;
                btnUpdateLineTrip.Visibility = Visibility.Hidden;
                tbLineID.Visibility = Visibility.Hidden;
                cbLineID.Visibility = Visibility.Hidden;
            }
            this.bl = _bl;
            this.lineTrips = bl.GetAllLinesTrip();
            lineTripDataGrid.ItemsSource = this.lineTrips;
            cbArea.ItemsSource = Enum.GetValues(typeof(BL.BO.Areas));
            cbLineCode.ItemsSource = bl.GetAllPropertyLine("Code");
            cbLineID.ItemsSource = bl.GetAllPropertyLine("LineID");
            this.settine();
            cbFrom.ItemsSource = this.time;
            cbTo.ItemsSource = this.time;
            this.lineCode = -1;
            this.lineID = -1;
            this.from = TimeSpan.Zero;
            this.to = TimeSpan.Zero;
        }

        private void settine()
        {
            this.time = new List<TimeSpan>();
            for (int i = 6; i < 24; i++)
            {
                this.time.Add(new TimeSpan(i, 0, 0));
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateBack();
        }
        private void btnAddLineTrip_Click(object sender, RoutedEventArgs e)
        {
            AddLineTripWindow window = new AddLineTripWindow(this.bl);
            window.Show();
            window.Closing += Window_Closing;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            lineTripDataGrid.ItemsSource = bl.GetAllLinesTrip();
            this.btnUpdateLineTrip.IsEnabled = false;
            this.btnDeleteLineTrip.IsEnabled = false;
        }

        private void btnDeleteLineTrip_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this line trip?", "Delete Station", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                if (bl.DeleteLineTrip(this.lineTrip.LineTripID))
                {
                    MessageBox.Show("Line trip deleted successfully");
                    lineTripDataGrid.ItemsSource = bl.GetAllLinesTrip();
                }
                else
                    MessageBox.Show("Line trip not exist");
            }

        }

        private void lineTripDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.lineTrip = (sender as DataGrid).SelectedItem as BL.BO.LineTrip;
            btnUpdateLineTrip.IsEnabled = true;
            btnDeleteLineTrip.IsEnabled = true;
        }

        private void btnUpdateLineTrip_Click(object sender, RoutedEventArgs e)
        {
            if (lineTripDataGrid.SelectedItem == null)
                return;
            UpdateLineTripWindow window = new UpdateLineTripWindow(this.bl, this.lineTrip);
            window.Show();
            window.Closing += Window_Closing;
        }

        private void cbLineID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            try
            {
                this.lineID = (int)cbLineID.SelectedValue;
                this.lineTrips = bl.GetAllLinesTrip(_lineTrip =>
                _lineTrip.LineID == this.lineID, this.lineTrips);
                lineTripDataGrid.ItemsSource = this.lineTrips;
            }
            catch (BL.BO.BadLineStationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (BL.BO.BadLineIDException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbLineCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.lineCode = (int)cbLineCode.SelectedValue;
                this.lineTrips = bl.GetAllLinesTrip(_lineTrip =>
                _lineTrip.LineCode == this.lineCode, this.lineTrips);
                lineTripDataGrid.ItemsSource = this.lineTrips;

            }
            catch (BL.BO.BadLineTripException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.area = (BL.BO.Areas)(int)cbArea.SelectedValue;
                this.lineTrips = bl.GetAllLinesTrip(_lineTrip =>
                _lineTrip.Area == (BL.BO.Areas)this.area, this.lineTrips);
                lineTripDataGrid.ItemsSource = this.lineTrips;
            }
            catch (BL.BO.BadLineTripException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.from = (TimeSpan)cbFrom.SelectedValue;
            this.lineTrips = bl.GetAllLinesTrip(_lineTrip =>
            _lineTrip.StartAt.Hours > this.from.Hours, this.lineTrips);
            lineTripDataGrid.ItemsSource = this.lineTrips;

        }

        private void cbTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.to = (TimeSpan)cbTo.SelectedValue;
            this.lineTrips = bl.GetAllLinesTrip(_lineTrip =>
            _lineTrip.StartAt.Hours < this.to.Hours, this.lineTrips);
            lineTripDataGrid.ItemsSource = this.lineTrips;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.lineTrips = bl.GetAllLinesTrip();
            this.lineTripDataGrid.ItemsSource = this.lineTrips;
        }
    }
}

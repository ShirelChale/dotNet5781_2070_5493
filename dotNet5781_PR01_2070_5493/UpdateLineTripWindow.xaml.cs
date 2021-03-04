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
    /// Interaction logic for UpdateLineTripWindow.xaml
    /// </summary>
    public partial class UpdateLineTripWindow : Window
    {
        IBL bl;
        int lineID;
        List<int> Hours;
        List<int> Minutes;
        int hours;
        int minutes;
        BL.BO.LineTrip lineTripToUpdate;

        public UpdateLineTripWindow(IBL _bl, BL.BO.LineTrip _lineTripToUpdate)
        {
            InitializeComponent();
            this.bl = _bl;
            this.lineTripToUpdate = _lineTripToUpdate;
            try
            {
                lineTripToUpdate = bl.GetLineTrip(this.lineTripToUpdate.LineID, this.lineTripToUpdate.StartAt);
            }
            catch (BL.BO.BadLineTripException ex)
            {

                MessageBox.Show(ex.Message);
                this.Close();
            }
            BL.BO.Line line = bl.GetLine(this.lineTripToUpdate.LineID);
            tbLineCode.Text = line.Code.ToString();
            tbLineID.Text = line.LineID.ToString();
            this.settingTime();
            cbHour.ItemsSource = this.Hours;
            cbMinutes.ItemsSource = this.Minutes;
            this.hours = this.lineTripToUpdate.StartAt.Hours;
            this.minutes = this.lineTripToUpdate.StartAt.Minutes;

        }
        private void settingTime()
        {
            this.Hours = new List<int>();
            for (int i = 6; i < 24; i++)
            {
                this.Hours.Add(i);
            }
            this.Minutes = new List<int>();
            for (int i = 0; i < 60; i += 10)
            {
                this.Minutes.Add(i);
            }

        }


        private void cbHour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.hours = (int)cbHour.SelectedItem;
        }

        private void cbMinutes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.minutes = (int)cbMinutes.SelectedItem;
        }


        private void btnUpadateLine_Click(object sender, RoutedEventArgs e)
        {
            this.lineTripToUpdate.StartAt = new TimeSpan(this.hours, this.minutes, 0);
            try
            {
                bl.UpdateLineTrip(this.lineTripToUpdate);
                MessageBox.Show("Line trip updated successfully!");
            }
            catch (BL.BO.BadLineTripException ex)
            {

                MessageBox.Show(ex.Message);
            }
            this.Close();
        }
    }
}

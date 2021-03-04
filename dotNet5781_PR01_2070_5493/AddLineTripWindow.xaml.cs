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
    /// Interaction logic for AddLineTripWindow.xaml
    /// </summary>
    public partial class AddLineTripWindow : Window
    {
        IBL bL;
        List<int> Hours;
        List<int> Minutes;
        BL.BO.LineTrip newLineTrip;
        bool[] fieldSelected;

        public AddLineTripWindow(IBL _bl)
        {
            InitializeComponent();
            this.bL = _bl;
            newLineTrip = new BL.BO.LineTrip()
            {
                LineID = -1
            };
            cbLineID.ItemsSource = bL.GetAllPropertyLine("LineID");
            this.settingTime();
            cbHour.ItemsSource = this.Hours;
            cbMinutes.ItemsSource = this.Minutes;
            this.fieldSelected = new bool[3] { false, false, false };
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

        private void cbLineID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int _lineID = (int)cbLineID.SelectedItem;
            this.newLineTrip.LineID = _lineID;
            tbLineCode.Text = bL.GetLine(_lineID).Code.ToString();
            this.fieldSelected[0] = true;
            if (this.check())
                btnCreateLineTrip.IsEnabled = true;

        }

        private void btnCreateLine_Click(object sender, RoutedEventArgs e)
        {
            bL.AddLineTrip(newLineTrip);
            MessageBox.Show("Line trip added successfully!");
            this.Close();
        }
        private bool check()
        {
            for (int i = 0; i < 3; i++)
            {
                if (!this.fieldSelected[i])
                    return false;
            }
            return true;
        }

        private void cbHour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int hour = (int)cbHour.SelectedItem;
            this.newLineTrip.StartAt += new TimeSpan(hour, 0, 0);
            this.fieldSelected[1] = true;
            if (this.check())
                btnCreateLineTrip.IsEnabled = true;

        }

        private void cbMinutes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int minutes = (int)cbMinutes.SelectedItem;
            this.newLineTrip.StartAt += new TimeSpan(0, minutes, 0);
            this.fieldSelected[2] = true;
            if (this.check())
                btnCreateLineTrip.IsEnabled = true;
        }
    }
}

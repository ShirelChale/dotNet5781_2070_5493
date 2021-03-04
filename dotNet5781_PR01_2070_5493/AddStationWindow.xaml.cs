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
using BL.BLAPI;
namespace dotNet5781_PR01_2070_5493
{
    /// <summary>
    /// Interaction logic for AddStationWindow.xaml
    /// </summary>
    public partial class AddStationWindow : Window
    {
        IBL bl;
        BL.BO.Station newStation;
        public AddStationWindow(IBL _bl)
        {
            InitializeComponent();
            this.bl = _bl;
            this.newStation = new BL.BO.Station();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lattitudeTextBox.Text != ""
                && longitudeTextBox.Text != "" && nameTextBox.Text != "")
            {
                this.newStation.Lattitude = double.Parse(lattitudeTextBox.Text);
                this.newStation.Longitude = double.Parse(longitudeTextBox.Text);
                if (!(this.newStation.Lattitude >= 31 && this.newStation.Lattitude <= 33.3) ||
                    (this.newStation.Longitude >= 34.3 && this.newStation.Longitude <= 35.5))
                {
                    MessageBox.Show("Lattitude or longitude aren't in Israel");
                    return;
                }
                this.newStation.Name = nameTextBox.Text;
                if (this.bl.AddStation(this.newStation))
                {
                    this.Close();
                    MessageBox.Show("Station added successfully");
                }
                else
                    MessageBox.Show("Station already exists");
            }
            else
            {
                MessageBox.Show("One or more missing details");
            }

        }


        private void integrityInputCheck(KeyEventArgs e)
        {
            // Allow errows, Back and delete keys:
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Right || e.Key == Key.Left || e.Key==Key.Decimal)
                return;
            // Allow only digits:
            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);
            if (char.IsDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return;
            e.Handled = true;
            MessageBox.Show("Only digits alowed!");
        }

        private void lattitudeTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }

        private void longitudeTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);

        }

        private void nameTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Allow errows, Back and delete keys:
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Right || e.Key == Key.Left)
                return;
        }
    }

}

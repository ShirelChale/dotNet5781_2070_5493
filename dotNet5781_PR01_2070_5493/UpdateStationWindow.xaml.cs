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
    /// Interaction logic for UpdateStationWindow.xaml
    /// </summary>
    public partial class UpdateStationWindow : Window
    {
        IBL bl;
        BL.BO.Station originalStation;
        public UpdateStationWindow(IBL _bl, BL.BO.Station _station)
        {
            InitializeComponent();
            this.bl = _bl;
            this.originalStation = _station;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BL.BO.Station updatedStation=new BL.BO.Station();
            if (originalStation == null)
                return;
            if (int.TryParse(codeTextBox.Text, out int _code))
            {
                if (_code.ToString().Length != 6)
                {
                    MessageBox.Show("Code must be 6 digits long");
                    return;
                }
                updatedStation.Code = _code;
            }
            if (int.TryParse(lattitudeTextBox.Text, out int _lattitude))
            {
                if (!(_lattitude >= 31 && _lattitude <= 33.3))
                {
                    MessageBox.Show("lattitude isn't in Israel");
                    return;
                }
                updatedStation.Lattitude = _lattitude;
            }
            if (int.TryParse(longitudeTextBox.Text, out int _longitude))
            {
                if (!(_longitude >= 34.3 && _longitude <= 35.5))
                {
                    MessageBox.Show("longitude isn't in Israel");
                    return;
                }
                updatedStation.Lattitude = _longitude;
            }

            if (nameTextBox.Text != "")
                updatedStation.Name = nameTextBox.Text;
            if (this.bl.UpdateStation(this.originalStation,updatedStation))
            {
                this.Close();
                MessageBox.Show("Station updated successfully");
            }
            else
                MessageBox.Show("Station not exists");
        }



        private void codeTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
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

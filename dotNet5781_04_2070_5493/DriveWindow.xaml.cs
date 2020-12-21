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
using System.Threading;

namespace dotNet5781_04_2070_5493
{
    /// <summary>
    /// Interaction logic for DriveWindow.xaml
    /// </summary>
    public partial class DriveWindow : Window
    {
        private Bus busToUpdate;
        public DriveWindow(Bus sourceBus)
        {
            InitializeComponent();
            busToUpdate = sourceBus;
            tbKilometersAmount.Visibility = Visibility.Visible;
        }

        private void tbKilometersAmount_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text.Text == "Kilometers amount")
                text.Text = string.Empty;
        }

        private void tbKilometersAmount_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Right || e.Key == Key.Left)
            {
                busToUpdate.Counter = -1;
                return;
            }
            if (e.Key == Key.Enter)
            {
                TextBox text = sender as TextBox;
                if (text.Text == string.Empty)
                {
                    busToUpdate.Counter = -1;
                    return;
                }

                e.Handled = true;
                if (!driving(text.Text))
                {
                    this.Close();
                    busToUpdate.Counter = -1;
                    return;
                }
                Random rand = new Random();
                busToUpdate.KilometerPerRide = int.Parse(tbKilometersAmount.Text);
                int speed = busToUpdate.KilometerPerRide / rand.Next(20, 51) * 6;
                busToUpdate.Counter = speed;
                this.Close();
                return;
            }

            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);
            if (char.IsDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                {
                    busToUpdate.Counter = -1;
                    return;
                }

            e.Handled = true;
            MessageBox.Show("Only digits alowed!");
        }

        private bool driving(string _kilometers)
        {
            double ride;
            double.TryParse(_kilometers, out ride);
            int integrityResult = busToUpdate.integrityCheck(ride);
            if (integrityResult == 1)
            {
                MessageBox.Show("There's not enough fuel for this ride.");
                return false;
            }
            if (integrityResult == 2)
            {
                MessageBox.Show("It's been a year and the bus needs treatment.");
                return false;
            }
            if (integrityResult == 3)
            {
                MessageBox.Show("Bus isn't ready for a ride.");
                return false;
            }
            //Thread drivingTimerThread = new Thread(;
            //Need to check if bus needs refueling and treatment.
            return true;
        }
    }
}

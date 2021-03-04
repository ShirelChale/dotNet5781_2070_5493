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
    /// Interaction logic for UpdateBusWindow.xaml
    /// </summary>
    public partial class UpdateBusWindow : Window
    {
        IBL bl;
        BL.BO.Bus busToUpdate;

        public UpdateBusWindow(IBL _bl, BL.BO.Bus _bus)
        {
            InitializeComponent();
            if (_bus == null)
                return;
            this.bl = _bl;
            this.busToUpdate = _bus;
            DataContext = this.busToUpdate;
            cbStatus.ItemsSource = Enum.GetValues(typeof(BL.BO.BusStatus));
            this.licenseNumberTextBlock.Text = _bus.LicenceNum.ToString();


        }
        private void startingDateDatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            if (startingDateDatePicker.SelectedDate > DateTime.Today)
            {
                MessageBox.Show("Starting date can't be later than today\n");
                this.startingDateDatePicker.Text = DateTime.Today.ToString();
                e.Handled = true;
                //startingDateDatePicker.Text = string.Empty;
                return;
            }
            this.busToUpdate.FromDate = (DateTime)startingDateDatePicker.SelectedDate;
        }
        private void fuelTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int text;
            if (int.TryParse(fuelTextBox.Text, out text))
            {
                if (text > 1200)
                {
                    MessageBox.Show("Fuel can't be over 1200");
                    e.Handled = true;
                    fuelTextBox.Text = string.Empty;
                    return;
                }
                if (text < 0)
                {
                    MessageBox.Show("Fuel can't be lower than 0");
                    e.Handled = true;
                    fuelTextBox.Text = string.Empty;
                    return;
                }
                this.busToUpdate.FuelRemain = text;
            }
        }
        private void totalKilometersTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int text;
            if (int.TryParse(totalKilometersTextBox.Text, out text))
            {
                if (text - this.busToUpdate.TotalTrip < 0)
                {
                    MessageBox.Show("Total kilometers can't be less than kilometers after treatment");
                    e.Handled = true;
                    totalKilometersTextBox.Text = string.Empty;
                    return;
                }
                this.busToUpdate.TotalTrip = text;
            }
        }

        private void newBusBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.bl.UpdateBus(this.busToUpdate);
                MessageBox.Show("Bus updated successfully!");
            }
            catch (BL.BO.BadBusException ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Close();
        }

        private void cbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            this.busToUpdate.Status = (BL.BO.BusStatus)cbStatus.SelectedItem;
            bl.UpdateBus(this.busToUpdate);
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

        private void fuelTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }

        private void totalKilometersTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }
    }

}


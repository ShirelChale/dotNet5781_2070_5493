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
using System.Collections.ObjectModel;

namespace dotNet5781_04_2070_5493
{
    /// <summary>
    /// Interaction logic for AddNewBusWindow.xaml
    /// </summary>
    public partial class AddNewBusWindow : Window
    {
        private Bus newBus;
        bool[] checkAllField;
        ObservableCollection<Bus> data;
        public AddNewBusWindow(ObservableCollection<Bus> _data)
        {
            InitializeComponent();
            newBus = new Bus();
            DataContext = newBus;
            checkAllField = new bool[6] { false, false, false, false, false, false };
            data = _data;
        }

        private void startingDateDatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            if (startingDateDatePicker.SelectedDate > DateTime.Today)
            {
                MessageBox.Show("Starting date can't be later than today\n");
                e.Handled = true;
                startingDateDatePicker.Text = string.Empty;
                checkAllField[0] = false;
                return;
            }
            newBus.StartingDate = (DateTime)startingDateDatePicker.SelectedDate;
            checkAllField[0] = true;
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
                    checkAllField[1] = false;
                    return;
                }
                if (text < 0)
                {
                    MessageBox.Show("Fuel can't be lower than 0");
                    e.Handled = true;
                    fuelTextBox.Text = string.Empty;
                    checkAllField[1] = false;
                    return;
                }
                newBus.Fuel = text;
                checkAllField[1] = true;
            }
        }

        private void kilometersAfterTreatmentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int text;
            if (int.TryParse(kilometersAfterTreatmentTextBox.Text, out text))
            {
                if (text >= 20000)
                {
                    MessageBox.Show("Kilometers not allowed");
                    e.Handled = true;
                    kilometersAfterTreatmentTextBox.Text = string.Empty;
                    checkAllField[2] = false;
                    return;
                }
                newBus.TotalKilometers += text;
                newBus.KilometersAfterTreatment = text;
                checkAllField[2] = true;
            }
        }

        private void licenseNumberTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (licenseNumberTextBox.Text.Length < 7 || licenseNumberTextBox.Text.Length > 8)
            {
                MessageBox.Show("License number can't be less than 7 or greater than 8");
                e.Handled = true;
                licenseNumberTextBox.Text = string.Empty;
                checkAllField[3] = false;
                return;
            }
            newBus.LicenseNumber = licenseNumberTextBox.Text;
            checkAllField[3] = true;
        }

        private void totalKilometersTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int text;
            if (int.TryParse(totalKilometersTextBox.Text, out text))
            {
                if (text - newBus.KilometersAfterTreatment < 0)
                {
                    MessageBox.Show("Total kilometers can't be less than kilometers after treatment");
                    e.Handled = true;
                    totalKilometersTextBox.Text = string.Empty;
                    checkAllField[4] = false;
                    return;
                }
                newBus.TotalKilometers = text;
                checkAllField[4] = true;
            }
        }
        private void dateTreatmentDatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dateTreatmentDatePicker.SelectedDate < startingDateDatePicker.SelectedDate)
            {
                MessageBox.Show("Treatment date can't be earlier than starting date.\n");
                e.Handled = true;
                dateTreatmentDatePicker.Text = string.Empty;
                checkAllField[5] = false;
                return;
            }
            newBus.DateTreatment = (DateTime)dateTreatmentDatePicker.SelectedDate;
            checkAllField[5] = true;
        }

        private void newBusBtn_Click(object sender, RoutedEventArgs e)
        {
            bool allCheck = true;
            if (checkAllField[0] == false)
            {
                MessageBox.Show("Starting date must be picked.");
                allCheck = false;
            }
            if (checkAllField[1] == false)
            {
                MessageBox.Show("Fuel field must be entered.");
                allCheck = false;
            }
            if (checkAllField[2] == false)
            {
                MessageBox.Show("Kilometers after treatment field must be entered.");
                allCheck = false;
            }
            if (checkAllField[3] == false)
            {
                MessageBox.Show("License number field must be entered.");
                allCheck = false;
            }
            if (checkAllField[4] == false)
            {
                allCheck = false;
                MessageBox.Show("Total kilometers field must be entered.");
            }
            if (checkAllField[5] == false)
            {
                MessageBox.Show("Treatment date must be picked.");
                allCheck = false;
            }
            if (allCheck)
            {
                data.Add(newBus);
                this.Close();
            }
        }

    }
}

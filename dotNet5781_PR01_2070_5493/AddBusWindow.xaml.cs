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
    /// Interaction logic for AddBusWindow.xaml
    /// </summary>
    public partial class AddBusWindow : Window
    {
        IBL bl;
        BL.BO.Bus newBus;
        bool[] checkAllField = new bool[5] { false, false, false, false, false };
        public AddBusWindow(IBL _bl)
        {
            InitializeComponent();
            this.bl = _bl;
            this.newBus = new BL.BO.Bus();
            DataContext = this.newBus;
        }

        private void startingDateDatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            if (startingDateDatePicker.SelectedDate > DateTime.Today)
            {
                MessageBox.Show("Starting date can't be later than today\n");
                this.startingDateDatePicker.Text = DateTime.Today.ToString();
                e.Handled = true;
                //startingDateDatePicker.Text = string.Empty;
                checkAllField[0] = false;
                return;
            }
            newBus.FromDate = (DateTime)startingDateDatePicker.SelectedDate;
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
                newBus.FuelRemain = text;
                checkAllField[1] = true;
            }
        }


        private void totalKilometersTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int text;
            if (int.TryParse(totalKilometersTextBox.Text, out text))
            {
                if (text - newBus.TotalTrip < 0)
                {
                    MessageBox.Show("Total kilometers can't be less than kilometers after treatment");
                    e.Handled = true;
                    totalKilometersTextBox.Text = string.Empty;
                    checkAllField[3] = false;
                    return;
                }
                newBus.TotalTrip = text;
                checkAllField[3] = true;
            }
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
            if (checkAllField[3] == false)
            {
                allCheck = false;
                MessageBox.Show("Total kilometers field must be entered.");
            }
            if (allCheck)
            {
                try
                {
                    this.bl.AddBus(this.newBus);
                    MessageBox.Show("Bus added successfully!");
                }
                catch (BL.BO.BadBusException ex)
                {

                    MessageBox.Show(ex.Message);
                }
                this.Close();
            }
        }
    }
}

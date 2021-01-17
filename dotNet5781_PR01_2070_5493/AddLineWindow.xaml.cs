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
    /// Interaction logic for AddLineWindow.xaml
    /// </summary>
    public partial class AddLineWindow : Window
    {
        IBL bl;
        List<int> rodeLine;
        int selectedStationCode;
        int code;
        BL.BO.Areas area;
        public AddLineWindow(IBL _bl)
        {
            InitializeComponent();
            this.bl = _bl;
            cbCode.ItemsSource = bl.GetAllPropertyStations("Code");
            rodeLine = new List<int>();
            areaComboBox.ItemsSource= Enum.GetValues(typeof(BL.BO.Areas));
        }

        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectedStationCode != 0)
            {
                this.rodeLine.Add(this.selectedStationCode);
                MessageBox.Show("Station added to this line rode.");
            }
            if (int.TryParse(codeTextBox.Text, out this.code) && this.rodeLine.Count>=2)
                btnCreateLine.IsEnabled = true;

        }

        private void btnDeleteStation_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectedStationCode != 0)
            {
                this.rodeLine.Remove(this.selectedStationCode);
                MessageBox.Show("Station removed from this line rode.");
            }
        }

        private void btnCreateLine_Click(object sender, RoutedEventArgs e)
        {
            this.code = int.Parse( codeTextBox.Text);
            bl.AddLine(this.code, this.rodeLine, this.area);
            MessageBox.Show("Line added successfully!");
            this.Close();
        }

        private void cbCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.selectedStationCode = (int)cbCode.SelectedValue;
        }

        private void areaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.area = (BL.BO.Areas)areaComboBox.SelectedValue;
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

        private void codeTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }

        
    }
}

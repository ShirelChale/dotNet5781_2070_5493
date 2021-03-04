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
    /// Interaction logic for UpdateLineWindow.xaml
    /// </summary>
    public partial class UpdateLineWindow : Window
    {
        IBL bl;
        BL.BO.Line line;
        int stationCode;
        int index;
        int lineStationToDelete;
        bool isItemSourceChanged;
        public UpdateLineWindow(IBL _bl, BL.BO.Line _line)
        {
            InitializeComponent();
            this.bl = _bl;
            this.line = _line;
            lineStationDataGrid.ItemsSource = this.bl.GetRouteLine(this.line);
            cbCode.ItemsSource = bl.GetAllPropertyStations("Code");
            areaComboBox.ItemsSource = Enum.GetValues(typeof(BL.BO.Areas));
            codeTextBox.Text = this.line.Code.ToString();
            this.isItemSourceChanged = false;
        }

        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {

            if (tbIndex.Text != string.Empty)
            {
                this.index = int.Parse(tbIndex.Text);
                if (this.index < 0)
                {
                    MessageBox.Show("Index can't be negitive");
                    tbIndex.Text = string.Empty;
                    btnAddStation.IsEnabled = false;
                    return;
                }
                if (this.index > this.line.LastStation.LineStationIndex + 1)
                {
                    MessageBox.Show("Index can't be out of range");
                    tbIndex.Text = string.Empty;
                    btnAddStation.IsEnabled = false;
                    return;
                }

            }
            else
                return;
            try
            {
                bl.AddLineStation(this.line.LineID, this.index, this.stationCode);
                lineStationDataGrid.ItemsSource = this.bl.GetRouteLine(this.line);
                this.line = bl.GetLine(this.line.LineID);
            }
            catch (BL.BO.BadLineStationException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            MessageBox.Show("Station added successfully");
            tbIndex.Text = string.Empty;
            btnAddStation.IsEnabled = false;

        }

        private void btnDeleteStation_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this line station?", "Delete line station", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                try
                {
                    bl.DeleteLineStation(this.line.LineID, this.lineStationToDelete);
                }
                catch (BL.BO.BadLineStationException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                isItemSourceChanged = true;
                lineStationDataGrid.ItemsSource = this.bl.GetRouteLine(this.line);
                this.line = bl.GetLine(this.line.LineID);
                if (this.line.LastStation.LineStationIndex <= 1)
                    btnDeleteStation.IsEnabled = false;
                MessageBox.Show("Line station deleted successfully");
            }

        }

        private void areaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BL.BO.Areas area = (BL.BO.Areas)areaComboBox.SelectedIndex;
            bl.UpdateLine(this.line.LineID, area);
        }

        private void cbCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.stationCode = (int)cbCode.SelectedValue;
            tbIndex.IsEnabled = true;
            lbIndex.IsEnabled = true;


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

        private void tbIndex_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            integrityInputCheck(e);
            btnAddStation.IsEnabled = true;
        }

        private void lineStationDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.isItemSourceChanged && this.line.LastStation.LineStationIndex > 1)
            {
                this.lineStationToDelete = ((sender as DataGrid).SelectedItem as BL.BO.LineStation).Station;
                if (this.line.LastStation.LineStationIndex > 1)
                    btnDeleteStation.IsEnabled = true;
            }
            this.isItemSourceChanged = false;

        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            if (codeTextBox.Text != string.Empty && int.Parse(codeTextBox.Text) != this.line.Code)
                bl.UpdateLine(this.line.LineID, this.line.Area, int.Parse(codeTextBox.Text));
            MessageBox.Show("Line updated successfully!");
            this.Close();
        }

        private void codeTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.integrityInputCheck(e);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for DisplayLineWindow.xaml
    /// </summary>
    public partial class DisplayLineWindow : Window
    {
        IBL bl;
        BL.BO.Line line;
        IEnumerable<BL.BO.Line> lines;
        public DisplayLineWindow(IBL _bl)
        {
            InitializeComponent();
            this.bl = _bl;
            try
            {
                this.lines = bl.GetAllLines();
                lineDataGrid.ItemsSource = this.lines;
                cbArea.ItemsSource = Enum.GetValues(typeof(BL.BO.Areas));
                cbFirstStationCode.ItemsSource = bl.GetAllPropertyLine("FirstStation");
                cbLastStationCode.ItemsSource = bl.GetAllPropertyLine("LastStation");
                cbLineCode.ItemsSource = bl.GetAllPropertyLine("Code");
                cbLineID.ItemsSource = bl.GetAllPropertyLine("LineID");
            }
            catch (BL.BO.BadLineStationException e)
            {
                MessageBox.Show(e.Message);
            }
            catch (BL.BO.BadLineIDException e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateBack();
        }

        private void btShowRode_Click(object sender, RoutedEventArgs e)
        {
            BL.BO.Line line = lineDataGrid.SelectedItem as BL.BO.Line;
            ShowLineRodeWindow window = new ShowLineRodeWindow(this.bl, line);
            window.Show();
        }

        private void cbLineID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int selecteLineID = (int)cbLineID.SelectedValue;
                this.lines = bl.GetAllLines(line => line.LineID == selecteLineID,this.lines);
                this.lineDataGrid.ItemsSource = this.lines;
            }
            catch (BL.BO.BadLineStationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (BL.BO.BadLineIDException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbLineCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int selecteLineCode = (int)cbLineCode.SelectedValue;
                this.lines = bl.GetAllLines(line => line.Code == selecteLineCode, this.lines);
                this.lineDataGrid.ItemsSource = this.lines;

            }
            catch (BL.BO.BadLineStationException b)
            {
                MessageBox.Show(b.Message);
            }
            catch (BL.BO.BadLineIDException b)
            {
                MessageBox.Show(b.Message);
            }
        }

        private void cbArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int selecteArea = (int)cbArea.SelectedValue;
                this.lines= bl.GetAllLines(line => (int)line.Area == selecteArea, this.lines);
                this.lineDataGrid.ItemsSource = this.lines;
            }
            catch (BL.BO.BadLineStationException c)
            {
                MessageBox.Show(c.Message);
            }
            catch (BL.BO.BadLineIDException c)
            {
                MessageBox.Show(c.Message);
            }
        }

        private void cbFirstStationCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int selecteFirstStationCode = (int)cbFirstStationCode.SelectedValue;
                this.lines = bl.GetAllLines(selecteFirstStationCode, 0, this.lines);
                this.lineDataGrid.ItemsSource = this.lines;
            }
            catch (BL.BO.BadLineStationException d)
            {
                MessageBox.Show(d.Message);
            }
            catch (BL.BO.BadLineIDException d)
            {
                MessageBox.Show(d.Message);
            }
        }

        private void cbLastStationCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int selecteLastStationCode = (int)cbLastStationCode.SelectedValue;
                this.lines= bl.GetAllLines(selecteLastStationCode, 1, this.lines);
                this.lineDataGrid.ItemsSource = this.lines;
            }
            catch (BL.BO.BadLineStationException a)
            {
                MessageBox.Show(a.Message);
            }
            catch (BL.BO.BadLineIDException a)
            {
                MessageBox.Show(a.Message);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.lines = bl.GetAllLines();
                lineDataGrid.ItemsSource = this.lines;
            }
            catch (Exception g)
            {
                MessageBox.Show(g.Message);
            }
        }

        private void lineDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.line = (sender as DataGrid).SelectedItem as BL.BO.Line;
            btnUpdateLine.IsEnabled = true;
            btnDeleteLine.IsEnabled = true;
        }

        private void btnDeleteLine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Are you sure you want to delete this Line?", "Delete line", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    bl.DeleteLine(this.line.LineID);
                    MessageBox.Show("Line deleted successfully");
                }
            }
            catch (BL.BO.BadLineIDException h)
            {
                MessageBox.Show(h.Message);
            }
            lineDataGrid.DataContext = bl.GetAllLines();
        }

        private void btnAddLine_Click(object sender, RoutedEventArgs e)
        {
            AddLineWindow window = new AddLineWindow(this.bl);
            window.Show();
            window.Closing += Window_Closing;
        }

        private void btnUpdateLine_Click(object sender, RoutedEventArgs e)
        {
            UpdateLineWindow window = new UpdateLineWindow(this.bl, this.line);
            window.Show();
            window.Closing += Window_Closing;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            this.lines = bl.GetAllLines();
            lineDataGrid.ItemsSource = this.lines;
            this.btnUpdateLine.IsEnabled = false;
            this.btnDeleteLine.IsEnabled = false;
        }

        

       
    }
}

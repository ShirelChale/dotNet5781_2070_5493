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
    /// Interaction logic for ShowLineRodeWindow.xaml
    /// </summary>
    public partial class ShowLineRodeWindow : Window
    {
        IBL bl;
        BL.BO.Line line;
        public ShowLineRodeWindow(IBL _bl, BL.BO.Line _line)
        {
            InitializeComponent();
            this.bl = _bl;
            this.line = _line;
            try
            {
                lineStationDataGrid.DataContext = bl.GetRouteLine(this.line);
            }
            catch (BL.BO.BadStationException ex)
            {

                MessageBox.Show("Couldn't find line station: " + ex.Code);
            }

        }

    }
    
}

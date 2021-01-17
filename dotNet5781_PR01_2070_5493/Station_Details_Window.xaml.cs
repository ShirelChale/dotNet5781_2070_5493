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
    /// Interaction logic for Station_Details_Window.xaml
    /// </summary>
    public partial class Station_Details_Window : Window
    {
        IBL bl;
        BL.BO.Station station;
        public Station_Details_Window(IBL _bl, BL.BO.Station _station)
        {
            InitializeComponent();
            this.bl = _bl;
            this.station = _station;
            tbStation.Text = this.station.Code.ToString();
            lineDataGrid.ItemsSource = bl.GetAllLinesCodeLastStation(this.station);
        }
    }
}

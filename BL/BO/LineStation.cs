using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    public class LineStation
    {

        public int LineID { get; set; }
        public DLAPI.DO.Station Station { get; set; }
        public int LineStationIndex { get; set; }
        public double DistanceFrom { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public override string ToString()
        {
            return this.Station.Code.ToString();
        }
    }
}

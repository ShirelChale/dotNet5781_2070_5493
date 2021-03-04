using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    public class LineTrip
    {
        public int LineTripID { get; set; }
        public int LineID { get; set; }
        public int LineCode { get; set; }
        public string LastStationName { get; set; }
        public BO.Areas Area { get; set; }
        public TimeSpan StartAt { get; set; }
    }
}

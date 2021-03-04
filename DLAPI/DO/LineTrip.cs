using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLAPI.DO
{
    public class LineTrip
    {
        public int LineTripID { get; set; }
        public int LineID { get; set; }
        public TimeSpan StartAt { get; set; }
        public bool Active { get; set; }
    }
}

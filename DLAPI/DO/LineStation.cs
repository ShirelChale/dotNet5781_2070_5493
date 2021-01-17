using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLAPI.DO
{
    public class LineStation
    {
        public int LineID { get; set; }
        public int Station { get; set; }
        public int LineStationIndex { get; set; }
        public bool Active { get; set; }
    }
}

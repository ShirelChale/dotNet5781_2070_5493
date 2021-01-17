using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    public enum Areas { Jerusalem, Har_Nof, Givat_Shaul };
    public class Line
    {
        public int LineID { get; set; }
        public int Code { get; set; }
        public Areas Area { get; set; }
        public BO.LineStation FirstStation { get; set; }
        public BO.LineStation LastStation { get; set; }
        public IEnumerable<LineStation> StationsLine { get; set; }
    }
}

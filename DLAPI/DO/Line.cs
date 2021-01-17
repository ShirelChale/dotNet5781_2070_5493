using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLAPI.DO
{
    public enum Areas { Jerusalem, Har_Nof, Givat_Shaul };
    public class Line
    {
        public int LineID { get; set; }
        public int Code { get; set; } // Line number
        public Areas Area { get; set; }
        public int FirstStation { get; set; }
        public int LastStation { get; set; }
        public bool Active { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    public class Trip
    {
        public int ID { get; set; }
        public User UserName { get; set; }
        public int LineID { get; set; }
        public Station InStation { get; set; }
        public TimeSpan InAt { get; set; }
        public Station OutStation { get; set; }
        public TimeSpan OutAt { get; set; }
    }
}

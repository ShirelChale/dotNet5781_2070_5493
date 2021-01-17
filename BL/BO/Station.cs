using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    public class Station
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Lattitude { get; set; }
        public IEnumerable<Line> Lines { get; set; }

        public override string ToString()
        {
            return this.Code + " " + this.Name;
        }
    }
}

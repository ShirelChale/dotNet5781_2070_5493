using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    public class LineTiming
    {
        private static int counter = 0;
        public int ID; 
        public LineTiming() => ID = ++counter; //unique
        public TimeSpan TripStart { get; set; } //time of Line start the trip, taken from StartAt of LineTrip
        public int LineId { get; set; } //Line ID from Line
        public int LineCode { get; set; } //Line Number as understood by the people
        public string LastStation { get; set; }// Last station name - so the passengers will know better which direction it is
        public TimeSpan ExpectedTimeTillArrive { get; set; }//Expected time of arrival
    }
}

    

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_2070_5493
{
    /// <summary>
    /// Initializing a line station (inherits Station class) 
    /// with distance between 2 stations and the time that takes between them.
    /// </summary>
    public class LineStation : Station
    {
        private double distance; // Distance from one line station to its next.
        private double timeRide; // Time of ride from one line station to its next.

        public LineStation() : base()
        { // Default constructor.
            this.Distance = 0;
            this.TimeRide = 0;
        }
        public LineStation(double _busStationKey, double _latitude,
            double _longitude, string _station_address)
            : base(_busStationKey, _latitude, _longitude, _station_address)
        { // Constructor.
            this.Distance = 0;
            this.TimeRide = 0;
        }
        public LineStation(LineStation station)
        {
            BusStationKey = station.BusStationKey;
            Latitude = station.Latitude;
            Longitude = station.Longitude;
            StationAddress = station.StationAddress;
            Distance = station.distance;
            TimeRide = station.timeRide;
        }

        // Properties:
        public double Distance { get => distance; set => distance = value; }
        public double TimeRide { get => timeRide; set => timeRide = value; }
        override public string ToString()
        {
            TimeSpan time = new TimeSpan(0, (int)this.timeRide, 0);
            return base.ToString() + " " + time + "\n";
        }
    }

}

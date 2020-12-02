using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_2070_5493
{
    /// <summary>
    /// Initializing a station with station number, longitude, latitude and address(option).
    /// </summary>
    public class Station
    {
        private double busStationKey;
        private double latitude;
        private double longitude;
        private string stationAddress;

        public Station(double _busStationKey, double _latitude,
            double _longitude, string _station_address = "")
        { // Constructor.
            this.busStationKey = _busStationKey; // Station number.
            this.Latitude = _latitude; // Coordinate x axis.
            this.Longitude = _longitude; // Coordinate y axis.
            this.StationAddress = _station_address; // Station addresss.
        }
        public Station() { } // Default constructor.
       
        // Properties:
        public double BusStationKey
        {
            get => busStationKey;
            set => busStationKey = value;
        }
        public double Latitude
        {
            get => latitude;
            set => latitude = value;
        }
        public double Longitude
        {
            get => longitude;
            set => longitude = value;
        }
        public string StationAddress
        {
            get => stationAddress;
            set => stationAddress = value;
        }

        public override string ToString()
        {
            string stationProperties = "Bus Station Code: " +
                this.busStationKey.ToString() + ", " + this.Latitude.ToString() +
                "°N  " + this.Longitude.ToString() + "°E";
            return stationProperties;
        }
    }

}

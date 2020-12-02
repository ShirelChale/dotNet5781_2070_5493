using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_2070_5493
{
    public enum AreaNum { Jerusalem, Har_Nof, Givat_Shaul };
    /// <summary>
    /// Initializing Bus line with line number, its first and last station, 
    /// its area and rode (of stations).
    /// </summary>
    public class BusesLine : IComparable<BusesLine>
    {
        

        private double busLine;
        private LineStation firstStation;
        private LineStation lastStation;
        private AreaNum area;
        List<LineStation> stations;
        public BusesLine()
        { // Default constructor.
            this.BusLine = 0;
            this.FirstStation = null;
            this.LastStation = null;
            this.Area = 0;
            this.Stations = new List<LineStation>();
        }

        public BusesLine(double _busLine, LineStation _firstStation, LineStation _lastStation, AreaNum _area)
        { // Constructor without last station and rode.
            this.busLine = _busLine;
            this.firstStation = _firstStation;
            this.Stations = new List<LineStation>();
            stations.Add(firstStation);
            this.Area = _area;
            this.LastStation = _lastStation;
            stations.Add(lastStation);
        }
        public BusesLine(double _busLine, LineStation _firstStation, AreaNum _area)
        { // Constructor without last station and rode.
            this.busLine = _busLine;
            this.firstStation = _firstStation;
            this.Stations = new List<LineStation>();
            stations.Add(firstStation);
            this.Area = _area;
            this.LastStation = this.firstStation;
        }

        public BusesLine(double _busLine, LineStation _firstStation,
            LineStation _lastStation, AreaNum _area, List<LineStation> _stations)
        { // Constuctor
            this.BusLine = _busLine;
            this.FirstStation = _firstStation;
            this.LastStation = _lastStation;
            this.Area = _area;
            this.Stations = new List<LineStation>(_stations);
        }

        // Properties:
        public double BusLine
        {
            get => busLine;
            set => busLine = value;
        }
        internal List<LineStation> Stations
        {
            get => stations;
            set => stations = value;
        }
        internal LineStation FirstStation
        {
            get => firstStation;
            set => firstStation = value;
        }
        internal LineStation LastStation
        {
            get => lastStation;
            set => lastStation = value;
        }
        public AreaNum Area { 
            get => area; 
            set => area = value; 
        }

        public override string ToString()
        {
            string busLineProperties = "Bus number: " +
                 this.busLine.ToString() + "\nBus area: " + this.Area + "\nStations List for "
                 + this.busLine.ToString() + "\n";
            foreach (LineStation station in stations)
            {
                busLineProperties += station.BusStationKey.ToString() + "\n";
            }

            return busLineProperties;
        }

        /// <summary>
        /// Add a new station to bus line in the appropriate place. 
        /// If station exists, do not add it.
        /// </summary>
        /// <param name="newStation"></param>
        public void addStation(LineStation stationToSet)
        {
            LineStation newStation = new LineStation(stationToSet);
            List<LineStation>.Enumerator enumeratorList = stations.GetEnumerator();
            if (!enumeratorList.MoveNext())//The line has no stations.
            {
                stations.Add(newStation);
                this.lastStation = newStation;
                return;
            }
            LineStation stationA = enumeratorList.Current;// First station: Point A.
            while (enumeratorList.MoveNext())
            {
                LineStation stationB = enumeratorList.Current;// Next station: Point B.

                if (newStation.BusStationKey == stationA.BusStationKey ||
                    newStation.BusStationKey == stationB.BusStationKey)
                    return; // No need to add existed station. 
                double distanceAtoC = this.distanceCalculation(stationA, newStation);
                double distanceCtoB = this.distanceCalculation(newStation, stationB);
                if (distanceAtoC + distanceCtoB == this.distanceCalculation(stationA, stationB))
                {
                    stationB.Distance = distanceCtoB;
                    newStation.Distance = distanceAtoC;
                    stationB.TimeRide = timeRideCalculation(newStation, stationB);
                    newStation.TimeRide = timeRideCalculation(stationA, newStation);
                    stations.Insert(Stations.IndexOf(stationB), newStation);
                    // Push new station before station B.
                    return;
                }
                stationA = stationB;
            }
            if (distanceCalculation(newStation, stations.First()) < distanceCalculation(newStation, stations.Last()))
            { // In case that the new station needs to be the first station.
                stations.First().Distance = distanceCalculation(newStation, stations.First());
                stations.First().TimeRide = timeRideCalculation(newStation, stations.First());
                stations.Insert(Stations.IndexOf(stations.First()), newStation);
            }
            else
            { // In case that the new station needs to be the last station.
                newStation.Distance = distanceCalculation(newStation, stations.Last());
                newStation.TimeRide = timeRideCalculation(newStation, stations.Last());
                stations.Add(newStation);
                this.lastStation = newStation;
            }
        }

        /// <summary>
        /// Remove a station from bus line.
        /// </summary>
        /// <param name="deleteStation"></param>
        public void removeStation(LineStation deleteStation)
        {
            foreach (LineStation station in stations)
            {
                if (station.BusStationKey == deleteStation.BusStationKey)
                { // Chack if station exists.
                    stations.Remove(station);
                    return;
                }
            }
        }

        /// <summary>
        /// Search a station in the bus line.
        /// </summary>
        /// <param name="stationToFind"></param>
        /// <returns></returns>
        public bool search(double stationToFind)
        {
            foreach (LineStation station in stations)
            {
                if (station.BusStationKey == stationToFind)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Calculates distance between 2 stations.
        /// </summary>
        /// <param name="firstStation"></param>
        /// <param name="secondStation"></param>
        /// <returns></returns>
        public double distanceCalculation(LineStation firstStation, LineStation secondStation)
        {
            double rLatitude1 = Math.PI * firstStation.Latitude / 180;
            double rLatitude2 = Math.PI * secondStation.Latitude / 180;
            double theta = firstStation.Longitude - secondStation.Longitude;
            double rTheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rLatitude1) * Math.Sin(rLatitude2) + Math.Cos(rLatitude1) *
                Math.Cos(rLatitude2) * Math.Cos(rTheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            return dist * 1.609344;

            //return Math.Sqrt(Math.Pow(firstStation.Latitude - secondStation.Latitude, 2) +
            //     Math.Pow(firstStation.Longitude - secondStation.Longitude, 2));
        }

        /// <summary>
        /// Calculates the ride time between 2 stations. (20m per second)
        /// </summary>
        /// <param name="stationA"></param>
        /// <param name="stationB"></param>
        /// <returns></returns>
        public double timeRideCalculation(LineStation stationA, LineStation stationB)
        {
            double dis = this.distanceCalculation(stationA, stationB);
            double totalSeconds = dis*1000 / 20;
            return totalSeconds / 60;
        }

        /// <summary>
        /// Creates sub bus line between 2 stations. 
        /// </summary>
        /// <param name="stationA"></param>
        /// <param name="stationB"></param>
        /// <returns></returns>
        public BusesLine subBusLine(LineStation stationA, LineStation stationB)
        {
            List<LineStation> subList = new List<LineStation>();
            List<LineStation>.Enumerator enumeratorList = stations.GetEnumerator();
            while (enumeratorList.MoveNext())
            {
                if (enumeratorList.Current.BusStationKey == stationA.BusStationKey)
                {
                    subList.Add(enumeratorList.Current);
                    while (enumeratorList.Current.BusStationKey != stationB.BusStationKey)
                    {
                        enumeratorList.MoveNext();
                        subList.Add(enumeratorList.Current);
                    }
                }
            }
            return new BusesLine(this.busLine, stationA, stationB, this.Area, subList);
        }
        /// <summary>
        /// Compares between 2 bus line by time.
        /// </summary>
        /// <param name="bus"></param>
        /// <returns>(-1) if the first bus line is shorter, (1) if the second is shorter
        /// and (0) if equal.</returns>
        public int CompareTo(BusesLine bus)
        {
            if (this.timeRideCalculation(this.FirstStation, this.LastStation) <
                bus.timeRideCalculation(bus.FirstStation, bus.LastStation))
                return -1;
            if (this.timeRideCalculation(this.FirstStation, this.LastStation) >
                bus.timeRideCalculation(bus.FirstStation, bus.LastStation))
                return 1;
            return 0;
        }
    }
}

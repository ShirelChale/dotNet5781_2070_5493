using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

using DLAPI.DO;
namespace DS
{
    public static class DataSource
    {
        public static List<DLAPI.DO.Line> ListLines;
        public static List<DLAPI.DO.Station> ListStations;
        public static List<DLAPI.DO.Bus> ListBuses;
        public static List<DLAPI.DO.User> ListUsers;
        public static List<DLAPI.DO.AdjacentStations> ListAdjacentStations;
        public static List<DLAPI.DO.LineStation> ListLineStations;
        public static List<DLAPI.DO.Trip> ListTrips;
        public static List<DLAPI.DO.LineTrip> ListLinesTrip;

        static DataSource()
        {
            InitAllLists();
        }
        static void InitAllLists()
        {
            setStationsList();
            setListLines();
            ListUsers = new List<User>();
            ListUsers.Add(new User { Admin = true, Password = "Manager123", UserName = "Manager",Active=true });
            busSetting();
            //setListLineStation();
        }

        //private static void setListLineStation()
        //{
        //    ListLineStations = new List<LineStation>();
        //    foreach (var line in ListLines)
        //    {
        //        if (line != null)
        //        {
        //            DLAPI.DO.LineStation newLineStation = new LineStation { LineID = line.LineID, Station = line.FirstStation, LineStationIndex = 0 };
        //            ListLineStations.Add(newLineStation);
        //            newLineStation = new LineStation { LineID = line.LineID, Station = line.LastStation, LineStationIndex = 0 };
        //            ListLineStations.Add(newLineStation);
        //        }
        //    }
        //}

        /// <summary>
        /// Sets stations list
        /// </summary>
        /// <returns>List of stations</returns>
        private static void setStationsList()
        {
            
            ListStations= new List<Station>();//Data
            int _stationCode = 100000;
            Random randomark = new Random();

            for (int i = 0; i < 40; i++)
            {
                var coord = new GeoCoordinate(randomark.Next(31, 33)+randomark.NextDouble(), randomark.Next(34,35)+ randomark.NextDouble());
                DLAPI.DO.Station newStation = new Station();
                newStation.Code = _stationCode + i;
                newStation.Name = "";
                newStation.Lattitude = coord.Latitude;
                newStation.Longitude = coord.Longitude;
                newStation.Active = true;
                ListStations.Add(newStation);
            }
        }

        /// <summary>
        /// Sets lines list
        /// </summary>
        /// <param name="stationsList"></param>
        /// <returns>List of buses line</returns>
        private static void setListLines()
        {
            ListLines = new List<DLAPI.DO.Line>();
            ListLineStations = new List<LineStation>();
            ListAdjacentStations = new List<AdjacentStations>();
            int _lineNum = 10;
            int _lineCode = 1;
            int _area = 0;
            Random randomArea = new Random(0);
            for (int i = 0; i < 10; i++)
            {
                _area = randomArea.Next(0, 3);
                DLAPI.DO.Line newLine = new DLAPI.DO.Line();
                newLine.LineID = _lineCode++;
                newLine.Code = _lineNum + i;
                DLAPI.DO.LineStation newLineStation = new LineStation() { LineID = newLine.LineID, Station = ListStations[i].Code, LineStationIndex = 0,Active=true };
                ListLineStations.Add(newLineStation);
                newLine.FirstStation = ListStations[i].Code;
                newLineStation = new LineStation() { LineID = newLine.LineID, Station = ListStations[i + 1].Code, LineStationIndex = 1,Active=true};
                ListLineStations.Add(newLineStation);
                newLine.LastStation = ListStations[i + 1].Code;
                double _distance = distanceCalc(ListStations[i].Lattitude, ListStations[i].Longitude,
                    ListStations[i + 1].Lattitude, ListStations[i + 1].Longitude);
                DLAPI.DO.AdjacentStations newAdjacentStation = new AdjacentStations()
                {
                    Station1 = ListStations[i].Code,
                    Station2 = ListStations[i + 1].Code,
                    Distance = _distance,
                    Time = new TimeSpan(0, 0, (int)_distance * 1000 / 20 / 60),
                    Active = true
                };
                ListAdjacentStations.Add(newAdjacentStation);
                newLine.Active = true;
                ListLines.Add(newLine);
            }
        }

        private static double distanceCalc(double _latitude1, double _longitude1, double _latitude2 ,double _longitude2)
        {
            var sCoord = new GeoCoordinate(_latitude1, _longitude1);
            var eCoord = new GeoCoordinate (_latitude2, _longitude2);
            return sCoord.GetDistanceTo(eCoord);
            
        }

        public static void busSetting()
        {
            ListBuses = new List<DLAPI.DO.Bus>();
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                DateTime _FromDate = new DateTime(rand.Next(1998, 2021), rand.Next(1, 13), rand.Next(1, 28));
                Bus newBus = new Bus
                {
                    LicenceNum = rand.Next(1000000, 100000000),
                    FromDate = _FromDate,
                    TotalTrip = rand.Next(1, 20000),
                    FuelRemain = rand.Next(1, 1200),
                    Status = BusStatus.ready
                };
                //newBus.DateTreatment = DateTime.Now;
                //newBus.KilometersAfterTreatment = rand.Next(1, (int)newBus.TotalKilometers);
                ListBuses.Add(newBus);
            }
        }
    }
}


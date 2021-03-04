using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using DLXML;
using DLAPI.DO;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;
using DLAPI;

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
        public static List<DLAPI.DO.LineTrip> ListLinesTrip;
        public static List<int> list;

        public static string configPath = @"staticConfigXml.xml"; //XElement
        public static string LinesPath = @"LinesXml.xml"; //XMLSerializer
        public static string StationsPath = @"StationsXml.xml"; //XMLSerializer
        public static string BusesPath = @"BusesXml.xml"; //XMLSerializer
        public static string UsersPath = @"UsersXml.xml"; //XMLSerializer
        public static string LineStationsPath = @"LineStationsXml.xml"; //XMLSerializer
        public static string LinesTripPath = @"LinesTripXml.xml"; //XElement
        public static string AdjacentStationsPath = @"AdjacentStationsXml.xml"; //XElement

        static DataSource()
        {
            InitAllLists();
            //XMLTools.SaveListToXMLSerializer(ListStations, StationsPath);
            //XMLTools.SaveListToXMLSerializer(ListLines, LinesPath);
            //XMLTools.SaveListToXMLSerializer(ListBuses, BusesPath);
            //XMLTools.SaveListToXMLSerializer(ListUsers, UsersPath);
            //XMLTools.SaveListToXMLSerializer(ListLineStations, LineStationsPath);
            //setadjacentStationsList(AdjacentStationsPath);
            //setlineTripList(LinesTripPath);
            //list = new List<int>();
            //list.Add(Config.LineID);//0
            //list.Add(Config.LicenceNum);//1
            //list.Add(Config.LineTripID);//2
            //list.Add(Config.StationCode);//3
            //XMLTools.SaveListToXMLSerializer(list, configPath);
        }

        static void setlineTripList(string path)
        {
            XMLTools.LoadListFromXMLElement(path);
            foreach (var item in ListLinesTrip)
            {
                    AddLineTrip(item);
            }
        } 
        static void setadjacentStationsList(string path)
        {
            XMLTools.LoadListFromXMLElement(path);
            foreach (var item in ListAdjacentStations)
            {
                    AddAdjacentStations(item);
            }
        }
        static void InitAllLists()
        {
            setStationsList();
            setListLines();
            ListUsers = new List<User>();
            ListUsers.Add(new User { Admin = true, Password = "Manager123", UserName = "Manager",Active=true });
            ListUsers.Add(new User { Admin = false, Password = "pass123", UserName = "pass",Active=true });
            busSetting();
            lineTripSetting();
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
            Random randomark = new Random();

            for (int i = 0; i < 40; i++)
            {
                var coord = new GeoCoordinate(randomark.Next(31, 33)+randomark.NextDouble(), randomark.Next(34,35)+ randomark.NextDouble());
                DLAPI.DO.Station newStation = new Station();
                newStation.Code = Config.StationCode++;
                newStation.Name = 'a'+i.ToString();
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
            int _area = 0;
            Random randomArea = new Random(0);
            for (int i = 0; i < 10; i++)
            {
                _area = randomArea.Next(0, 3);
                DLAPI.DO.Line newLine = new DLAPI.DO.Line();
                newLine.LineID = Config.LineID++;
                newLine.Code = _lineNum + i;
                DLAPI.DO.LineStation newLineStation = new LineStation() { LineID = newLine.LineID, Station = ListStations[i].Code, LineStationIndex = 0,Active=true };
                ListLineStations.Add(newLineStation);
                newLine.FirstStation = ListStations[i].Code;
                newLineStation = new LineStation() { LineID = newLine.LineID, Station = ListStations[i + 1].Code, LineStationIndex = 1,Active=true};
                ListLineStations.Add(newLineStation);
                newLine.LastStation = ListStations[i + 1].Code;
                double _distance = distanceCalc(ListStations[i].Lattitude, ListStations[i].Longitude,
                    ListStations[i + 1].Lattitude, ListStations[i + 1].Longitude)/1000;
                DLAPI.DO.AdjacentStations newAdjacentStation = new AdjacentStations()
                {
                    Station1 = ListStations[i].Code,
                    Station2 = ListStations[i + 1].Code,
                    Distance = _distance,
                    Time = new TimeSpan(0,0, (int)(_distance * 0.3)),
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
                    Active=true,
                    LicenceNum = Config.LicenceNum++,
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

        public static void lineTripSetting()
        {
            ListLinesTrip = new List<LineTrip>();
            Random randTime = new Random();
            foreach (var line in ListLines)
            {
                int i = 0;
                while (i != 2)
                {
                    TimeSpan time = new TimeSpan(randTime.Next(6, 24), randTime.Next(0, 6) * 10, 0);
                    LineTrip newLineTrip = new LineTrip()
                    {
                        Active = true,
                        LineID = line.LineID,
                        LineTripID = Config.LineTripID++,
                        StartAt = time
                    };
                    ListLinesTrip.Add(newLineTrip);
                    i++;
                }
            }
        }
        public static void AddLineTrip(DLAPI.DO.LineTrip _lineTrip)
        {
            XElement lineTripsRootElem = XMLTools.LoadListFromXMLElement(LinesTripPath);
            XElement lineTripToAdd = (from lineTrip in lineTripsRootElem.Elements()
                                      where int.Parse(lineTrip.Element("LineID").Value) == _lineTrip.LineID && TimeSpan.ParseExact(lineTrip.Element("StartAt").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)==_lineTrip.StartAt
                                      && bool.Parse(lineTrip.Element("Active").Value)
                                      select lineTrip).FirstOrDefault();
            if (lineTripToAdd != null)
                throw new DO.BadLineTripException(_lineTrip.LineID, "LineTrip already exist");
            XElement newLineTripToAdd = new XElement("LineTrip", new XElement("LineTripID", _lineTrip.LineTripID),
                                        new XElement("LineID", _lineTrip.LineID),
                                        new XElement("StartAt", _lineTrip.StartAt.ToString()),
                                        new XElement("Active", _lineTrip.Active));
            lineTripsRootElem.Add(newLineTripToAdd);
            XMLTools.SaveListToXMLElement(lineTripsRootElem, LinesTripPath);
        }
        public static void AddAdjacentStations(DLAPI.DO.AdjacentStations _adjacentStations)
        {
            XElement adjacentStationsRootElem = XMLTools.LoadListFromXMLElement(AdjacentStationsPath);
            XElement adjacentStationToAdd = (from addAdjacentStations in adjacentStationsRootElem.Elements()
                                             where int.Parse(addAdjacentStations.Element("Station1").Value) == _adjacentStations.Station1 && int.Parse(addAdjacentStations.Element("Station2").Value) == _adjacentStations.Station2
                                             && bool.Parse(addAdjacentStations.Element("Active").Value)
                                             select addAdjacentStations).FirstOrDefault();
            if (adjacentStationToAdd != null)
                throw new DO.BadAdjacentStationsException(_adjacentStations.Station1, _adjacentStations.Station2, "LineTrip already exist");
            XElement newAdjacentStationsAdd = new XElement("AdjacentStations", new XElement("Station1", _adjacentStations.Station1),
                                        new XElement("Station2", _adjacentStations.Station2),
                                        new XElement("Distance", _adjacentStations.Distance),
                                        new XElement("Time", _adjacentStations.Time.ToString()),
                                        new XElement("Active", _adjacentStations.Active));
            adjacentStationsRootElem.Add(newAdjacentStationsAdd);
            XMLTools.SaveListToXMLElement(adjacentStationsRootElem, AdjacentStationsPath);
        }

        
    }
}


using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using DLAPI;
using DLAPI.DO;
using DLXML;

namespace DL
{

    sealed class DLXML : IDL    //internal
    {
        #region singelton
        static readonly DLXML instance = new DLXML();
        static DLXML() { }// static ctor to ensure instance init is done just before first usage
        DLXML() { } // default => private
        public static DLXML Instance { get => instance; }// The public Instance property to use
        #endregion

        #region DS XML Files

        string configPath = @"staticConfigXml.xml"; //XElement
        string LinesPath = @"LinesXml.xml"; //XMLSerializer
        string StationsPath = @"StationsXml.xml"; //XMLSerializer
        string BusesPath = @"BusesXml.xml"; //XMLSerializer
        string UsersPath = @"UsersXml.xml"; //XMLSerializer
        string LineStationsPath = @"LineStationsXml.xml"; //XMLSerializer
        string LinesTripPath = @"LinesTripXml.xml"; //XElement
        string AdjacentStationsPath = @"AdjacentStationsXml.xml"; //XElement

        #endregion

        #region Bus
        public void AddBus(DLAPI.DO.Bus _bus)
        {
            List<int> list = XMLTools.LoadListFromXMLSerializer<int>(configPath);
            _bus.LicenceNum = list[1]++;
            XMLTools.SaveListToXMLSerializer<int>(list, configPath);
            List<Bus> listBuses = XMLTools.LoadListFromXMLSerializer<Bus>(BusesPath);
            DLAPI.DO.Bus busToAdd = listBuses.Find(bus => bus.LicenceNum == _bus.LicenceNum);
            if (busToAdd == null)
                listBuses.Add(_bus);
            else if (busToAdd.Active)
                throw new DO.BadBusException(_bus.LicenceNum, "Bus dousn't exist");
            XMLTools.SaveListToXMLSerializer(listBuses, BusesPath);

        }
        public void UpdateBus(DLAPI.DO.Bus _bus)
        {
            List<Bus> listBuses = XMLTools.LoadListFromXMLSerializer<Bus>(BusesPath);

            DLAPI.DO.Bus busToUpdate = listBuses.Find(bus => bus.LicenceNum == _bus.LicenceNum && bus.Active);
            if (busToUpdate != null)
            {
                busToUpdate.Active = true;
                busToUpdate.FromDate = _bus.FromDate;
                busToUpdate.FuelRemain = _bus.FuelRemain;
                busToUpdate.TotalTrip = _bus.TotalTrip;
                busToUpdate.Status = _bus.Status;
            }
            else
                throw new DO.BadBusException(_bus.LicenceNum, "Bus not found");
            XMLTools.SaveListToXMLSerializer(listBuses, BusesPath);

        }
        public void DeleteBus(int _licenseNum)
        {
            List<Bus> listBuses = XMLTools.LoadListFromXMLSerializer<Bus>(BusesPath);
            try
            {
                listBuses.Find(bus => bus.LicenceNum == _licenseNum && bus.Active).Active = false;
            }
            catch (NullReferenceException ex)
            {

                throw new DO.BadBusException(_licenseNum, "Bus dosen't exist");
            }
            XMLTools.SaveListToXMLSerializer(listBuses, BusesPath);
        }
        public IEnumerable<DLAPI.DO.Bus> GetAllBuses()
        {
            List<Bus> listBuses = XMLTools.LoadListFromXMLSerializer<Bus>(BusesPath);
            return from bus in listBuses
                   where bus.Active
                   select bus;
        }

        public IEnumerable<object> GetAllproperties(string property)
        {
            List<Bus> listBuses = XMLTools.LoadListFromXMLSerializer<Bus>(BusesPath);
            return from bus in listBuses
                   where bus.Active
                   orderby bus.LicenceNum
                   select this.getPropBus(bus, property);
        }


        #endregion

        #region Station
        public DLAPI.DO.Station GetStation(int _code)
        {
            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            DLAPI.DO.Station _station = listStations.Find(station => station.Code == _code
             && station.Active);
            if (_station != null)
                return _station;
            else
                throw new DO.BadStationCodeException(_code);
        }

        public void AddStation(DLAPI.DO.Station _station)
        {
            List<int> list = XMLTools.LoadListFromXMLSerializer<int>(configPath);
            _station.Code = list[3]++;
            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            listStations.Add(_station);
            XMLTools.SaveListToXMLSerializer(listStations, StationsPath);
            XMLTools.SaveListToXMLSerializer<int>(list, configPath);

        }
        public bool UpdateStation(DLAPI.DO.Station _updetedStation)
        {
            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            DLAPI.DO.Station stationToUpdate = listStations.Find(station => station.Code == _updetedStation.Code);
            if (stationToUpdate != null)
            {
                if (_updetedStation.Code != 0)
                    stationToUpdate.Code = _updetedStation.Code;
                if (_updetedStation.Name != "")
                    stationToUpdate.Name = _updetedStation.Name;
                if (_updetedStation.Longitude != 0)
                    stationToUpdate.Longitude = _updetedStation.Longitude;
                if (_updetedStation.Lattitude != 0)
                    stationToUpdate.Lattitude = _updetedStation.Lattitude;
                XMLTools.SaveListToXMLSerializer(listStations, StationsPath);
                return true;
            }
            return false;
        }
        public bool DeleteStation(int _code)
        {
            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            DLAPI.DO.Station stationToDelete = listStations.Find(station => station.Code == _code);
            if (stationToDelete != null)
            {
                stationToDelete.Active = false;
                XMLTools.SaveListToXMLSerializer(listStations, StationsPath);
                return true;
            }
            return false;
        }
        public IEnumerable<DLAPI.DO.Station> GetAllStations()
        {
            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            return from station in listStations
                   where station.Active
                   select station;
        }
        public IEnumerable<DLAPI.DO.Station> GetAllStations(Predicate<DLAPI.DO.Station> predicate)
        {
            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            return from station in listStations
                   where station.Active && predicate(station)
                   select station;
        }
        public IEnumerable<object> GetAllPropertyStations(string property)
        {
            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            return from station in listStations
                   where station.Active
                   orderby station.Code
                   select getPropStation(station, property);
        }
        private object getPropStation(DLAPI.DO.Station _station, string property)
        {
            PropertyInfo pinfo = typeof(DLAPI.DO.Station).GetProperty(property);
            object value = pinfo.GetValue(_station, null);
            return value;
        }
        private object getPropBus(DLAPI.DO.Bus _bus, string property)
        {
            PropertyInfo pinfo = typeof(DLAPI.DO.Bus).GetProperty(property);
            object value = pinfo.GetValue(_bus, null);
            return value;
        }

        #endregion

        #region Line
        public DLAPI.DO.Line GetLine(int _lineID)
        {
            List<Line> listLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            DLAPI.DO.Line _line = listLines.Find(line => line.LineID == _lineID
             && line.Active);
            if (_line != null)
                return _line;
            throw new DO.BadLineIDException(_lineID);

        }
        public int AddLine(int _code, DLAPI.DO.Areas _area, int _firstStation, int _lastStatoin)
        {
            List<int> list = XMLTools.LoadListFromXMLSerializer<int>(configPath);
            List<Line> listLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            DLAPI.DO.Line newLine = new DLAPI.DO.Line();
            newLine.Active = true;
            newLine.Area = _area;
            newLine.Code = _code;
            newLine.LineID = list[0]++;
            newLine.FirstStation = _firstStation;
            newLine.LastStation = _lastStatoin;
            listLines.Add(newLine);
            XMLTools.SaveListToXMLSerializer(listLines, LinesPath);
            XMLTools.SaveListToXMLSerializer<int>(list, configPath);
            return newLine.LineID;
        }
        public void UpdateLine(int _lineID, int _stationCode, int firstOrLast)
        {
            List<Line> listLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            if (firstOrLast == 0)
                listLines.Find(line => line.LineID == _lineID).FirstStation = _stationCode;
            else
                listLines.Find(line => line.LineID == _lineID).LastStation = _stationCode;
            XMLTools.SaveListToXMLSerializer(listLines, LinesPath);
        }

        public void UpdateLine(int _lineID, Action<DLAPI.DO.Line> update)// Method that knows to updt specific fields in Line
        {
            List<Line> listLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            try
            {
                DLAPI.DO.Line _currentLine = listLines.Find(_line => _line.LineID == _lineID);
                if (_currentLine != null)
                    update(_currentLine);
            }
            catch (DO.BadLineIDException ex)
            {
                throw ex;
            }
            XMLTools.SaveListToXMLSerializer(listLines, LinesPath);
        }
        public void DeleteLine(int _lineID)
        {
            List<Line> listLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            DLAPI.DO.Line _line = listLines.Find(line => line.LineID == _lineID);
            if (_line != null)
            {
                _line.Active = false;
                this.DeleteLineStation(_line.LineID, _line.FirstStation);
                this.DeleteLineStation(_line.LineID, _line.LastStation);
            }
            else
                throw new DO.BadLineIDException(_lineID);
            XMLTools.SaveListToXMLSerializer(listLines, LinesPath);
        }
        public IEnumerable<DLAPI.DO.Line> GetAllLines()
        {
            List<Line> listLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            return from line in listLines
                   where line.Active
                   select line;
        }
        public IEnumerable<DLAPI.DO.Line> GetAllLines(Predicate<DLAPI.DO.Line> predicate)
        {
            List<Line> listLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            return from line in listLines
                   where predicate(line) && line.Active
                   select line;
        }
        public IEnumerable<object> GetLineListWithSelectedFields(Func<DLAPI.DO.Line, object> generate)
        {
            List<Line> listLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            return from line in listLines
                   where line.Active
                   select generate(line);
        }
        public IEnumerable<object> GetAllPropertyLines(string property)
        {
            List<Line> listLines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            return from line in listLines
                   where line.Active
                   select getPropLine(line, property);
        }
        private object getPropLine(DLAPI.DO.Line _line, string property)
        {
            PropertyInfo pinfo = typeof(DLAPI.DO.Line).GetProperty(property);
            object value = pinfo.GetValue(_line, null);
            return value;
        }

        #endregion

        #region LineStation
        public DLAPI.DO.LineStation GetLineStation(int _station, int _lineID)
        {
            List<LineStation> listLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
            DLAPI.DO.LineStation _lineStation = listLineStations.Find(lineStation => lineStation.Station == _station
             && lineStation.LineID == _lineID && lineStation.Active);
            if (_lineStation != null)
                return _lineStation;
            throw new DO.BadLineStationException(_lineID, _station);
        }
        public void AddLineStation(int _lineID, int _stationCode, int _index)
        {
            List<LineStation> listLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
            DLAPI.DO.LineStation lineStation = new DLAPI.DO.LineStation()
            {
                Active = true,
                Station = _stationCode,
                LineStationIndex = _index,
                LineID = _lineID
            };
            listLineStations.Add(lineStation);
            XMLTools.SaveListToXMLSerializer(listLineStations, LineStationsPath);
        }
        public void AddLineStation(DLAPI.DO.LineStation _lineStation)
        {
            List<LineStation> listLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
            DLAPI.DO.LineStation newLineStation = listLineStations.Find(station => station.Station == _lineStation.Station && station.LineID == _lineStation.LineID);
            if (newLineStation != null)
            {
                if (newLineStation.Active)
                    throw new DO.BadLineStationException(newLineStation.LineID, newLineStation.Station);
                newLineStation.LineStationIndex = _lineStation.LineStationIndex;
                newLineStation.Active = true;
            }
            listLineStations.Add(_lineStation);
            XMLTools.SaveListToXMLSerializer(listLineStations, LineStationsPath);
        }
        public void UpdateLineStation(int _lineID, int _stationCode, Action<DLAPI.DO.LineStation> update)
        {
            List<LineStation> listLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
            DLAPI.DO.LineStation lineStationToUpdate = listLineStations.Find(_lineStation => _lineStation.LineID == _lineID && _lineStation.Station == _stationCode);
            if (lineStationToUpdate != null)
                update(lineStationToUpdate);
            XMLTools.SaveListToXMLSerializer(listLineStations, LineStationsPath);
        } // Method that knows to updt specific fields in LineStation.
        public void DeleteLineStation(int _lineID, int _stationCode)
        {
            List<LineStation> listLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
            listLineStations.Find(lineStation => lineStation.Station == _stationCode
            && lineStation.LineID == _lineID).Active = false;
            XMLTools.SaveListToXMLSerializer(listLineStations, LineStationsPath);

        }
        public void DeleteLineStations(int _stationCode)
        {
            List<LineStation> listLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
            foreach (var lineStation in listLineStations)
            {
                if (lineStation.Active)
                    if (lineStation.Station == _stationCode)
                        lineStation.Active = false;
            }
            XMLTools.SaveListToXMLSerializer(listLineStations, LineStationsPath);
        }

        public IEnumerable<DLAPI.DO.LineStation> GetRouteLine(int _lineID)
        {
            List<LineStation> listLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
            return from lineStation in listLineStations
                   where lineStation.LineID == _lineID && lineStation.Active
                   orderby lineStation.LineStationIndex
                   select lineStation;

        }
        public IEnumerable<DLAPI.DO.LineStation> GetRouteLine(Predicate<DLAPI.DO.LineStation> predicate)
        {
            List<LineStation> listLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
            return from lineStation in listLineStations
                   where predicate(lineStation) && lineStation.Active
                   select lineStation;
        }

        public IEnumerable<DLAPI.DO.LineStation> GetAllLineStations(Predicate<DLAPI.DO.LineStation> predicate)
        {
            List<LineStation> listLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
            return from lineStation in listLineStations
                   where predicate(lineStation) && lineStation.Active
                   select lineStation.Clone();
        }

        #endregion

        #region AdjacentStations
        public DLAPI.DO.AdjacentStations GetAdjacentStations(int _station1, int _station2)
        {
            XElement adjacentStationsRootElem = XMLTools.LoadListFromXMLElement(AdjacentStationsPath);
            IEnumerable<XElement> List = adjacentStationsRootElem.Elements();
            AdjacentStations adjacentStationsToGet = (from adjacentStations in adjacentStationsRootElem.Elements()
                                                      where int.Parse(adjacentStations.Element("Station1").Value) == _station1 && int.Parse(adjacentStations.Element("Station2").Value) == _station2
                                                      && bool.Parse(adjacentStations.Element("Active").Value)
                                                      select new AdjacentStations()
                                                      {
                                                          Active = bool.Parse(adjacentStations.Element("Active").Value),
                                                          Station1 = int.Parse(adjacentStations.Element("Station1").Value),
                                                          Station2 = int.Parse(adjacentStations.Element("Station2").Value),
                                                          Time = TimeSpan.ParseExact(adjacentStations.Element("Time").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture),
                                                          Distance = double.Parse(adjacentStations.Element("Distance").Value)

                                                      }).FirstOrDefault();

            if (adjacentStationsToGet != null && adjacentStationsToGet.Active)
                return adjacentStationsToGet;
            throw new DO.BadAdjacentStationsException(_station1, _station2);
        }
        public double GetDistanceForTwoStations(int _station1, int _station2)
        {
            return this.GetAdjacentStations(_station1, _station2).Distance;
        }
        public TimeSpan GetTimeForTwoStations(int _station1, int _station2)
        {
            return this.GetAdjacentStations(_station1, _station2).Time;
        }
        public void AddAdjacentStations(DLAPI.DO.AdjacentStations _adjacentStations)
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
                                        new XElement("Distance", _adjacentStations.Distance), new XElement("Active", _adjacentStations.Active), new XElement("Time", _adjacentStations.Time.ToString()));
            adjacentStationsRootElem.Add(newAdjacentStationsAdd);
            XMLTools.SaveListToXMLElement(adjacentStationsRootElem, AdjacentStationsPath);
        }
        public void DeleteAdjacentStations(int _stationCode)
        {
            XElement adjacentStationsRootElem = XMLTools.LoadListFromXMLElement(AdjacentStationsPath);
            for (int i = 0; i < adjacentStationsRootElem.Elements().Count(); i++)
            {
                adjacentStationsRootElem = XMLTools.LoadListFromXMLElement(LinesTripPath);
                XElement adjacentStationsToDelete = default(XElement);
                try
                {
                    adjacentStationsToDelete = (from adjacentStations in adjacentStationsRootElem.Elements()
                                                where int.Parse(adjacentStations.Element("Station1").Value) == _stationCode || int.Parse(adjacentStations.Element("Station2").Value) == _stationCode
                                                && bool.Parse(adjacentStations.Element("Active").Value)
                                                select adjacentStations).FirstOrDefault();
                }
                catch (NullReferenceException ex)
                {
                    return;

                }
                if (adjacentStationsToDelete != null)
                {
                    adjacentStationsToDelete.Remove();
                    XMLTools.SaveListToXMLElement(adjacentStationsRootElem, AdjacentStationsPath);
                }
                else
                    throw new DO.BadAdjacentStationsException(_stationCode, _stationCode, "Coudln't find Adjacent Stations.");
            }
        }
        #endregion

        #region LineTrip
        public DLAPI.DO.LineTrip GetLineTrip(int _lineID, TimeSpan _startAt)
        {
            XElement lineTripsRootElem = XMLTools.LoadListFromXMLElement(LinesTripPath);
            LineTrip lineTripToGet = (from _lineTrip in lineTripsRootElem.Elements()
                                      where int.Parse(_lineTrip.Element("LineID").Value) == _lineID && TimeSpan.ParseExact(_lineTrip.Element("StartAt").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture) == _startAt
                                      select new LineTrip()
                                      {
                                          Active = bool.Parse(_lineTrip.Element("Active").Value),
                                          LineID = int.Parse(_lineTrip.Element("LineID").Value),
                                          LineTripID = int.Parse(_lineTrip.Element("LineTripID").Value),
                                          StartAt = TimeSpan.ParseExact(_lineTrip.Element("StartAt").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)
                                      }).FirstOrDefault();
            if (lineTripToGet != null && lineTripToGet.Active)
                return lineTripToGet;
            throw new DO.BadLineTripException("Line trip doesn't exist");
        }
        public void AddLineTrip(DLAPI.DO.LineTrip _lineTrip)
        {
            List<int> list = XMLTools.LoadListFromXMLSerializer<int>(configPath);
            _lineTrip.LineTripID = list[2]++;
            XElement lineTripsRootElem = XMLTools.LoadListFromXMLElement(LinesTripPath);
            XElement lineTripToAdd = (from lineTrip in lineTripsRootElem.Elements()
                                      where int.Parse(lineTrip.Element("LineID").Value) == _lineTrip.LineID && TimeSpan.ParseExact(lineTrip.Element("StartAt").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture) == _lineTrip.StartAt
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
            XMLTools.SaveListToXMLSerializer<int>(list, configPath);

        }
        public void UpdateLineTrip(DLAPI.DO.LineTrip _lineTrip)
        {
            XElement lineTripsRootElem = XMLTools.LoadListFromXMLElement(LinesTripPath);
            XElement lineTripToUpdate = (from lineTrip in lineTripsRootElem.Elements()
                                         where int.Parse(lineTrip.Element("LineTripID").Value) == _lineTrip.LineTripID
                                         && bool.Parse(lineTrip.Element("Active").Value)
                                         select lineTrip).FirstOrDefault();
            if (lineTripToUpdate != null)
            {
                lineTripToUpdate.Element("LineTripID").Value = _lineTrip.LineTripID.ToString();
                lineTripToUpdate.Element("LineID").Value = _lineTrip.LineID.ToString();
                lineTripToUpdate.Element("StartAt").Value = _lineTrip.StartAt.ToString();
                lineTripToUpdate.Element("Active").Value = _lineTrip.Active.ToString();
                XMLTools.SaveListToXMLElement(lineTripsRootElem, LinesTripPath);
            }
            else
                throw new DO.BadLineTripException("Line trip doesn't exist");

        }
        public void DeleteLineTrip(int _lineTripID)
        {
            XElement lineTripsRootElem = XMLTools.LoadListFromXMLElement(LinesTripPath);
            XElement lineTripToDelete = (from lineTrip in lineTripsRootElem.Elements()
                                         where int.Parse(lineTrip.Element("LineTripID").Value) == _lineTripID
                                         && bool.Parse(lineTrip.Element("Active").Value)
                                         select lineTrip).FirstOrDefault();
            if (lineTripToDelete != null)
            {
                lineTripToDelete.Remove();
                XMLTools.SaveListToXMLElement(lineTripsRootElem, LinesTripPath);
            }
            else
                throw new DO.BadLineTripException(_lineTripID, "Coudln't find LineTrip.");
        }
        public void DeleteLineTripPerLine(int _lineID)
        {
            foreach (var lineTrip in this.GetAllLinesTrip())
            {
                if (lineTrip.LineID == _lineID)
                    this.DeleteLineTrip(lineTrip.LineTripID);
            }
        }
        public IEnumerable<DLAPI.DO.LineTrip> GetAllLinesTrip()
        {
            XElement lineTripsRootElem = XMLTools.LoadListFromXMLElement(LinesTripPath);
            IEnumerable<XElement> List = lineTripsRootElem.Elements();
            return from lineTrip in lineTripsRootElem.Elements()
                   let _lineTrip = new LineTrip()
                   {
                       Active = bool.Parse(lineTrip.Element("Active").Value),
                       LineID = int.Parse(lineTrip.Element("LineID").Value),
                       LineTripID = int.Parse(lineTrip.Element("LineTripID").Value),
                       StartAt = TimeSpan.ParseExact(lineTrip.Element("StartAt").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)
                   }
                   where _lineTrip.Active
                   orderby _lineTrip.StartAt
                   select _lineTrip;
        }
        public IEnumerable<DLAPI.DO.LineTrip> GetAllLinesTrip(Predicate<DLAPI.DO.LineTrip> predicate)
        {
            XElement lineTripsRootElem = XMLTools.LoadListFromXMLElement(LinesTripPath);
            return from lineTrip in lineTripsRootElem.Elements()
                   let _lineTrip = new LineTrip()
                   {
                       Active = bool.Parse(lineTrip.Element("Active").Value),
                       LineID = int.Parse(lineTrip.Element("LineID").Value),
                       LineTripID = int.Parse(lineTrip.Element("LineTripID").Value),
                       StartAt = TimeSpan.ParseExact(lineTrip.Element("StartAt").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)
                   }
                   where _lineTrip.Active && predicate(_lineTrip)
                   orderby _lineTrip.StartAt
                   select _lineTrip;
        }
        #endregion

        #region User
        public DLAPI.DO.User GetUser(string _userName)
        {
            List<User> listUsers = XMLTools.LoadListFromXMLSerializer<User>(UsersPath);
            DLAPI.DO.User currUser = listUsers.Find(p => p.UserName == _userName);
            return currUser;
        }
        public void AddUser(DLAPI.DO.User _user)
        {
            List<User> listUsers = XMLTools.LoadListFromXMLSerializer<User>(UsersPath);
            listUsers.Add(_user);
            XMLTools.SaveListToXMLSerializer(listUsers, UsersPath);
        }
        public void UpdateUser(DLAPI.DO.User _user)
        {
            List<User> listUsers = XMLTools.LoadListFromXMLSerializer<User>(UsersPath);
            DLAPI.DO.User currUser = listUsers.Find(p => p.UserName == _user.UserName);
            currUser.Password = _user.Password;
            currUser.Admin = _user.Admin;
            XMLTools.SaveListToXMLSerializer(listUsers, UsersPath);
        }
        public void DeleteUser(string _userName)
        {
            List<User> listUsers = XMLTools.LoadListFromXMLSerializer<User>(UsersPath);
            listUsers.Find(p => p.UserName == _userName).Active = false;
            XMLTools.SaveListToXMLSerializer(listUsers, UsersPath);
        }
        public IEnumerable<DLAPI.DO.User> GetAllUsers()
        {
            List<User> listUsers = XMLTools.LoadListFromXMLSerializer<User>(UsersPath);
            return from user in listUsers
                   select user;
        }
        public IEnumerable<DLAPI.DO.User> GetAllUsers(Predicate<DLAPI.DO.User> predicate)
        {
            List<User> listUsers = XMLTools.LoadListFromXMLSerializer<User>(UsersPath);
            return from user in listUsers
                   where predicate(user)
                   select user;
        }
        #endregion

    }
}

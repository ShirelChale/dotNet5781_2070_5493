using System;
using System.Collections.Generic;
using System.Linq;
using DLAPI;
using BL.BLAPI;
using System.Threading;
using System.Device.Location;

namespace BL
{

    class BLImp : IBL //internal
    {
        #region udapter
        IDL dl = DLFactory.GetDL();
        public BO.Bus busDoBoAdapter(DLAPI.DO.Bus DO_bus)
        {
            BO.Bus BO_bus = new BO.Bus();
            if (DO_bus != null)
                DO_bus.CopyPropertiesTo(BO_bus);
            return BO_bus;
        }
        public BO.User userDoBoAdapter(DLAPI.DO.User DO_user)
        {
            BO.User BO_user = new BO.User();
            if (DO_user != null)
                DO_user.CopyPropertiesTo(BO_user);
            return BO_user;
        }
        public BO.Station stationDoBoAdapter(DLAPI.DO.Station DO_station)
        {
            BO.Station BO_station = new BO.Station();
            if (DO_station != null)
            {
                BO_station.Code = DO_station.Code;
                BO_station.Longitude = DO_station.Longitude;
                BO_station.Lattitude = DO_station.Lattitude;
                BO_station.Name = DO_station.Name;
                BO_station.Lines = new List<BL.BO.Line>(this.GetAllLines(BO_station));
            }

            return BO_station;
        }
        public BO.LineStation lineStationDoBoAdapter(DLAPI.DO.LineStation DO_lineStation)
        {
            BO.LineStation BO_lineStation = new BO.LineStation();
            if (DO_lineStation != null)
            {
                BO_lineStation.LineID = DO_lineStation.LineID;
                BO_lineStation.LineStationIndex = DO_lineStation.LineStationIndex;
            }
            try
            {
                BO_lineStation.Station = dl.GetStation(DO_lineStation.Station);
            }
            catch (DO.BadStationCodeException ex)
            {
                throw new BO.BadStationException("LineStation not exist", ex);
            }
            return BO_lineStation;
        }
        public BO.Line lineDoBoAdapter(DLAPI.DO.Line DO_line)
        {
            BO.Line BO_line = new BO.Line();
            if (DO_line != null)
            {
                BO_line.LineID = DO_line.LineID;
                BO_line.Area = (BL.BO.Areas)(int)DO_line.Area;
                BO_line.Code = DO_line.Code;
            }
            try
            {
                BO_line.FirstStation = this.lineStationDoBoAdapter(dl.GetLineStation(DO_line.FirstStation, DO_line.LineID));
                BO_line.LastStation = this.lineStationDoBoAdapter(dl.GetLineStation(DO_line.LastStation, DO_line.LineID));
                BO_line.StationsLine = from lineStation in dl.GetRouteLine(lineStation => lineStation.LineID == BO_line.LineID)
                                       select this.lineStationDoBoAdapter(lineStation);
            }
            catch (BO.BadLineStationException ex)
            {
                throw new BO.BadLineStationException("LineStation not defined", ex);
            }
            return BO_line;
        }
        #endregion

        #region Line
        public void AddLine(int _code, IEnumerable<int> _rode, BO.Areas _area=BO.Areas.Jerusalem)
        {
            int lineID = dl.AddLine(_code, (DLAPI.DO.Areas)(int)_area, _rode.First(), _rode.Last());
            int index = 0;
            int prevStation = _rode.First();
            DLAPI.DO.AdjacentStations adjacentStation;
            foreach (int stationCode in _rode)
            {
                if (index != 0)
                {
                    double _distance;
                    TimeSpan _time;
                    try
                    {
                        adjacentStation = dl.GetAdjacentStations(prevStation, stationCode);
                    }
                    catch (DO.BadAdjacentStationsException ex)
                    {
                        double longitude1 = dl.GetStation(prevStation).Longitude;
                        double longitude2 = dl.GetStation(stationCode).Longitude;
                        double latitude1 = dl.GetStation(prevStation).Lattitude;
                        double latitude2 = dl.GetStation(stationCode).Lattitude;
                        _distance = this.distanceCalc(latitude1, longitude1, latitude2, longitude2);
                        _time = new TimeSpan(0, 0, (int)_distance * 1000 / 20 / 60);
                        adjacentStation = new DLAPI.DO.AdjacentStations()
                        {
                            Active = true,
                            Station1 = prevStation,
                            Station2 = stationCode,
                            Distance = _distance,
                            Time = _time
                        };
                        dl.AddAdjacentStations(adjacentStation);
                    }
                    this.AddLineStation(lineID, dl.GetStation(stationCode), index);
                }
                else
                    this.AddLineStation(lineID, dl.GetStation(stationCode), index);

                index++;
                prevStation = stationCode;
            }

        }
        public void DeleteLine(int _lineID)
        {
            try
            {
                foreach (var lineStation in this.GetLine(_lineID).StationsLine)
                    dl.DeleteLineStation(_lineID, lineStation.Station.Code);

                dl.DeleteLine(_lineID);
            }
            catch (DO.BadLineIDException ex)
            {
                throw new BO.BadLineIDException("Couln't find line", ex);
            }
        }
        public BO.Line GetLine(int _lineID)
        {
            return this.lineDoBoAdapter(dl.GetLine(_lineID));
        }
        public void UpdateLine(int _lineID) { }
        public IEnumerable<BO.Line> GetAllLines(BO.Station _station)
        {
            try
            {
                return from lineStation in dl.GetAllLineStations(lineStation => lineStation.Station == _station.Code)
                       select this.lineDoBoAdapter(dl.GetLine(lineStation.LineID));
            }
            catch (BO.BadLineStationException ex)
            {
                throw new BO.BadLineStationException("LineStation not defined", ex);
            }

        }
        public IEnumerable<BO.Line> GetAllLines(Predicate<BO.Line> predicate)
        {
            return from line in this.GetAllLines()
                   where predicate(line)
                   select line;
        }
        public IEnumerable<BO.LinesPerStation> GetAllLinesCodeLastStation(BO.Station _station)
        {
            List<DLAPI.DO.LineStation> lineStationslist = dl.GetRouteLine(lineStation => lineStation.Station == _station.Code).ToList();
            List<BO.LinesPerStation> resultList = new List<BO.LinesPerStation>();
            foreach (var lineStation in lineStationslist)
            {
                resultList.Add(new BO.LinesPerStation() { LastStation = dl.GetLine(lineStation.LineID).LastStation, Code = dl.GetLine(lineStation.LineID).Code });
            }
            return resultList;
        }
        public IEnumerable<BO.Line> GetAllLines()
        {
            return from line in dl.GetAllLines()
                   select this.lineDoBoAdapter(line);

        }
        public IEnumerable<object> GetAllPropertyLine(string _property)
        {
            return dl.GetAllPropertyLines(_property);
        }
        public IEnumerable<BO.Line> GetAllLines(int stationCode, int firstOrLastStation)
        {
            if (firstOrLastStation == 0)
                return this.GetAllLines(line => line.FirstStation.Station.Code == stationCode);
            return this.GetAllLines(line => line.LastStation.Station.Code == stationCode);
        }
        #endregion

        #region User
        public void AddUser(string _userName, string _password)
        {
            DLAPI.DO.User newUser = new DLAPI.DO.User();
            newUser.UserName = _userName;
            newUser.Password = _password;
            dl.AddUser(newUser);
        }
        public void DeleteUser(string _userName)
        {

        }
        public BO.User GetUser(string _userName)
        {
            return this.userDoBoAdapter(dl.GetUser(_userName));
        }
        public void UpdateUser(string _userName)
        {

        }
        public IEnumerable<BO.User> GetAllUsers(BO.Line _station)
        {
            return new List<BO.User>();
        }
        public IEnumerable<BO.User> GetAllUsers(Predicate<BO.User> predicate)
        {
            return new List<BO.User>();
        }


        #endregion

        #region LineStation
        public void AddLineStation(int _lineID, DLAPI.DO.Station _currentStation, int _index)
        {
            dl.AddLineStation(_lineID, _currentStation.Code, _index);
        }
        public void DeleteLineStation(int _lineID, BO.Station _currentStation, BO.Station _prevStation, BO.Station _nextStation) { }
        public void GetLineStation(int _lineID, BO.Station _currentStation, BO.Station _prevStation, BO.Station _nextStation) { }
        public void UpdateLineStation(int _lineID, BO.Station _currentStation, BO.Station _prevStation, BO.Station _nextStation) { }
        public IEnumerable<BO.LineStation> GetRouteLine(BO.Line _line)
        {
            var rode = from lineStation in dl.GetRouteLine(_line.LineID)
                       select this.lineStationDoBoAdapter(lineStation);
            BO.LineStation prevLineStation = new BO.LineStation();
            List<BO.LineStation> resultRode = new List<BO.LineStation>();
            try
            {
                foreach (var lineStation in rode)
                {

                    if (lineStation.LineStationIndex != 0)
                    {
                        lineStation.DistanceFrom = dl.GetDistanceForTwoStations(prevLineStation.Station.Code, lineStation.Station.Code);
                        lineStation.TimeFrom = dl.GetTimeForTwoStations(prevLineStation.Station.Code, lineStation.Station.Code);
                    }
                    prevLineStation = lineStation;
                    resultRode.Add(lineStation);
                }
            }
            catch (BO.BadLineStationException ex)
            {
                throw ex;
            }
            return resultRode;

        }

        public IEnumerable<BO.LineStation> GetRouteLine(Predicate<BO.LineStation> predicate)
        {
            return new List<BO.LineStation>();

        }
        #endregion

        #region Station
        public void AddStation(int _code, string _name) { }
        public bool AddStation(BO.Station _station)
        {
            var results = from station in dl.GetAllStations()
                          where station.Code == _station.Code
                          select station;
            foreach (var result in results)
            {
                if (!result.Active)
                {
                    dl.UpdateStation(result.Code, result);
                    return true;
                }
                return false;
            }
            DLAPI.DO.Station newStation = new DLAPI.DO.Station()
            {
                Active = true,
                Code = _station.Code,
                Lattitude = _station.Lattitude,
                Longitude = _station.Longitude,
                Name = _station.Name
            };
            dl.AddStation(newStation);
            return true;
        }
        public void DeleteStation(int _code, string _name) { }
        public bool DeleteStation(BO.Station _station)
        {
            if (dl.DeleteStation(_station.Code))
                return true;
            // TO DO adjacentStation
            return false;
        }
        public BL.BO.Station GetStation(int _code)
        {
            return this.stationDoBoAdapter(dl.GetStation(_code));
        }

        public void UpdateStation(int _code, string _name) { }
        public IEnumerable<BO.Station> GetAllStations()
        {
            return from _station in dl.GetAllStations()
                   orderby _station.Code
                   select this.stationDoBoAdapter(_station);
        }
        public IEnumerable<object> GetAllPropertyStations(string property)
        {
            return dl.GetAllPropertyStations(property);
        }
        public bool UpdateStation(BL.BO.Station _thisStation, BL.BO.Station _updetedStation)
        {
            DLAPI.DO.Station updetedStation = new DLAPI.DO.Station()
            {
                Active = true,
                Code = _updetedStation.Code,
                Lattitude = _updetedStation.Lattitude,
                Longitude = _updetedStation.Longitude,
                Name = _updetedStation.Name
            };
            if (dl.UpdateStation(_thisStation.Code, updetedStation))
                return true;
            return false;
        }

        public IEnumerable<BO.Station> GetAllStations(Predicate<BO.Station> predicate)
        {
            return from station in this.GetAllStations()
                   where predicate(station)
                   select station;
        }
        #endregion

        #region AdjacentStations
        public void AddAdjacentStations(int _station1, int _station2) { }
        public void DeleteAdjacentStations(BO.Station _station1, BO.Station _station2) { }
        public void GetAdjacentStations(BO.Station _station1, BO.Station _station2) { }
        public void UpdateAdjacentStations(BO.Station _station1, BO.Station _station2) { }
        public IEnumerable<BO.AdjacentStations> GetAllAdjacentStations(BO.Line _line)
        {
            return new List<BO.AdjacentStations>();
        }
        public IEnumerable<BO.AdjacentStations> GetAllAdjacentStations(Predicate<BO.AdjacentStations> predicate)
        {
            return new List<BO.AdjacentStations>();
        }
        #endregion

        #region Bus
        public void AddBus(int _licenceNum) { }
        public void DeleteBus(int _licenceNum) { }
        public void GetBus(int _licenceNum) { }
        public void UpdateBus(int _licenceNum) { }
        public IEnumerable<BO.Bus> GetAllBuses()
        {
            return from bus in dl.GetAllBuses()
                   select this.busDoBoAdapter(bus);
        }
        public IEnumerable<BO.Bus> GetAllBuses(Predicate<BO.Bus> predicate)
        {
            return new List<BO.Bus>();
        }
        #endregion

        private double distanceCalc(double _latitude1, double _longitude1, double _latitude2, double _longitude2)
        {
            var sCoord = new GeoCoordinate(_latitude1, _longitude1);
            var eCoord = new GeoCoordinate(_latitude2, _longitude2);
            return sCoord.GetDistanceTo(eCoord);

        }
    }
}

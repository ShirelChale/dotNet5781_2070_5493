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
        IDL dl = DLFactory.GetDL();

        #region singelton
        static readonly BLImp instance = new BLImp();
        static BLImp() { }// static ctor to ensure instance init is done just before first usage
        //BLImp() { } // default => private
        public static BLImp Instance { get => instance; }// The public Instance property to use
        #endregion 

        #region Adapter
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
                BO_lineStation.Station = dl.GetStation(DO_lineStation.Station).Code;
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

        public BO.LineTrip lineTripAdapter(DLAPI.DO.LineTrip DO_lineTrip)
        {
            BO.LineTrip BO_lineTrip = new BO.LineTrip();
            if (DO_lineTrip != null)
                DO_lineTrip.CopyPropertiesTo(BO_lineTrip);
            DLAPI.DO.Line line = dl.GetLine(DO_lineTrip.LineID);
            BO_lineTrip.LineCode = line.Code;
            BO_lineTrip.LastStationName = dl.GetStation(line.LastStation).Name;
            BO_lineTrip.Area = (BO.Areas)(int)line.Area;
            return BO_lineTrip;
        }
        #endregion

        #region Line
        public void AddLine(int _code, IEnumerable<int> _rode, BO.Areas _area = BO.Areas.Jerusalem)
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
                        _distance = this.distanceCalc(latitude1, longitude1, latitude2, longitude2) / 1000;
                        _time = new TimeSpan(0, (int)(_distance * 1.5), 0);
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
                    dl.DeleteLineStation(_lineID, lineStation.Station);
                dl.DeleteLineTripPerLine(_lineID);
                dl.DeleteLine(_lineID);
            }
            catch (DO.BadLineIDException ex)
            {
                throw new BO.BadLineIDException("Couln't find line", ex);
            }
        }
        public void DeleteStationFromLines(int _stationCode)
        {
            foreach (var line in this.GetAllLines())
            {
                if (line.FirstStation.Station == _stationCode || line.LastStation.Station == _stationCode)
                {
                    if (line.StationsLine.Count() == 2)
                        dl.DeleteLine(line.LineID);
                    else
                    {
                        if (line.FirstStation.Station == _stationCode)
                        {
                            dl.UpdateLine(line.LineID, line.StationsLine.ElementAt(1).Station, 0);
                            foreach (var lineStation in line.StationsLine)
                            {
                                dl.UpdateLineStation(line.LineID, lineStation.Station, _lineStation => _lineStation.LineStationIndex--);
                            }
                        }
                        if (line.LastStation.Station == _stationCode)
                            dl.UpdateLine(line.LineID, line.StationsLine.ElementAt(line.StationsLine.Count() - 2).Station, 1);
                    }
                }
                else
                {
                    BO.LineStation _currentLineStation = line.StationsLine.ToList().Find(_station => _station.Station == _stationCode);
                    if (_currentLineStation != null)
                    {
                        this.AddAdjacentStations(line.StationsLine.ElementAt(_currentLineStation.LineStationIndex - 1).Station, line.StationsLine.ElementAt(_currentLineStation.LineStationIndex + 1).Station);
                        dl.UpdateLineStation(line.LineID, line.StationsLine.ElementAt(_currentLineStation.LineStationIndex + 1).Station, _lineStation => _lineStation.LineStationIndex--);
                    }

                }

            }
        }
        public BO.Line GetLine(int _lineID)
        {
            return this.lineDoBoAdapter(dl.GetLine(_lineID));
        }
        public void UpdateLine(int _lineID, BO.Areas area = BO.Areas.Jerusalem, int lineCode = -1)
        {
            if (lineCode == -1)
            {
                DLAPI.DO.Areas updateArea = (DLAPI.DO.Areas)(int)area;
                dl.UpdateLine(_lineID, line => line.Area = updateArea);
                return;
            }
            dl.GetLine(_lineID).Code = lineCode;
        }
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
        public IEnumerable<BO.Line> GetAllLines(Predicate<BO.Line> predicate, IEnumerable<BO.Line> _lines)
        {
            return from line in _lines
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
        public IEnumerable<BO.Line> GetAllLines(int stationCode, int firstOrLastStation, IEnumerable<BO.Line> _lines)
        {
            if (firstOrLastStation == 0)
                return from line in _lines
                       where line.FirstStation.Station == stationCode
                       select line;
            return from line in _lines
                   where line.LastStation.Station == stationCode
                   select line;
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
        public void UpdateUser(string _userName, bool _admin)
        {
            DLAPI.DO.User updateUser = dl.GetUser(_userName);
            updateUser.Admin = _admin;
            dl.UpdateUser(updateUser);
        }
        public IEnumerable<BO.User> GetAllUsers()
        {
            return from user in dl.GetAllUsers()
                   orderby user.UserName
                   select this.userDoBoAdapter(user);
        }
        public IEnumerable<BO.User> GetAllUsers(Predicate<BO.User> predicate)
        {
            return from user in this.GetAllUsers()
                   where predicate(user)
                   orderby user.UserName
                   select user;
        }
        public IEnumerable<string> getAllpropertyPerUser()
        {
            return from user in dl.GetAllUsers()
                   orderby user.UserName
                   select user.UserName;
        }



        #endregion

        #region LineStation
        private void AddLineStation(int _lineID, DLAPI.DO.Station _currentStation, int _index)
        {
            dl.AddLineStation(_lineID, _currentStation.Code, _index);
        }
        public void AddLineStation(int _lineID, int _index, int _currentStationCode)
        {
            List<BO.LineStation> list = this.GetRouteLine(this.GetLine(_lineID)).ToList();
            if (list.Find(lineStatio => lineStatio.Station == _currentStationCode) != null)
                throw new BO.BadLineStationException("Line station already exist");
            BO.LineStation prevStation = new BO.LineStation();
            BO.LineStation nextStation = new BO.LineStation();
            double distance;
            DLAPI.DO.AdjacentStations adjacentStation = new DLAPI.DO.AdjacentStations();
            if (_index != list.First().LineStationIndex)
            {
                if (_index == list.Last().LineStationIndex + 1)
                {
                    dl.UpdateLine(_lineID, line => line.LastStation = _currentStationCode);
                }
                prevStation = list[_index - 1];
                try
                {
                    dl.GetAdjacentStations(prevStation.Station, _currentStationCode);
                    adjacentStation = dl.GetAdjacentStations(prevStation.Station, _currentStationCode);

                }
                catch (DO.BadAdjacentStationsException ex)
                {
                    DLAPI.DO.Station _station1 = dl.GetStation(prevStation.Station);
                    DLAPI.DO.Station _station2 = dl.GetStation(_currentStationCode);
                    distance = this.distanceCalc(_station1.Lattitude, _station1.Longitude,
                        _station2.Lattitude, _station2.Longitude) / 1000;
                    adjacentStation = new DLAPI.DO.AdjacentStations()
                    {
                        Active = true,
                        Station1 = _station1.Code,
                        Station2 = _station2.Code,
                        Distance = distance,
                        Time = new TimeSpan(0, (int)(distance * 1.5), 0),
                    };
                    dl.AddAdjacentStations(adjacentStation);

                }
                DLAPI.DO.LineStation newLineStation = new DLAPI.DO.LineStation()
                {
                    LineID = _lineID,
                    Station = dl.GetStation(_currentStationCode).Code,
                    LineStationIndex = _index,
                    Active = true
                };
                try
                {
                    dl.AddLineStation(newLineStation);
                }
                catch (DO.BadLineStationException ex)
                {
                    throw new BO.BadLineStationException("Line station already exist in this line", ex);
                }

            }
            if (_index != list.Last().LineStationIndex + 1)
            {
                if (_index == 0)
                {
                    DLAPI.DO.LineStation newLineStation = new DLAPI.DO.LineStation()
                    {
                        LineID = _lineID,
                        Station = dl.GetStation(_currentStationCode).Code,
                        LineStationIndex = _index,
                        Active = true
                    };
                    try
                    {
                        dl.AddLineStation(newLineStation);
                    }
                    catch (DO.BadLineStationException ex)
                    {
                        throw new BO.BadLineStationException("Line station already exist in this line", ex);
                    }
                    dl.UpdateLine(_lineID, line => line.FirstStation = _currentStationCode);
                }
                nextStation = list[_index];
                try
                {
                    dl.GetAdjacentStations(_currentStationCode, nextStation.Station);
                    adjacentStation = dl.GetAdjacentStations(_currentStationCode, nextStation.Station);
                }
                catch (DO.BadAdjacentStationsException ex)
                {
                    DLAPI.DO.Station _station1 = dl.GetStation(_currentStationCode);
                    DLAPI.DO.Station _station2 = dl.GetStation(nextStation.Station);
                    distance = this.distanceCalc(_station1.Lattitude, _station1.Longitude,
                        _station2.Lattitude, _station2.Longitude) / 1000;
                    adjacentStation = new DLAPI.DO.AdjacentStations()
                    {
                        Active = true,
                        Station1 = _station1.Code,
                        Station2 = _station2.Code,
                        Distance = distance,
                        Time = new TimeSpan(0, (int)(distance * 1.5), 0),
                    };
                    dl.AddAdjacentStations(adjacentStation);


                }

            }
            for (int i = _index; i < list.Count; i++)
            {
                DLAPI.DO.LineStation lineStationToUpdate = dl.GetLineStation(list[i].Station, list[i].LineID);
                dl.UpdateLineStation(lineStationToUpdate.LineID, lineStationToUpdate.Station, lineStation => lineStation.LineStationIndex++);
            }
        }
        public void DeleteLineStation(int _lineID, int _currentStationCode)
        {
            int currentIndex = dl.GetLineStation(_currentStationCode, _lineID).LineStationIndex;
            int prevStationIndex = 0;
            var list = dl.GetRouteLine(_lineID).ToList();
            if (currentIndex != 0 && currentIndex != list.Last().LineStationIndex)
            {
                prevStationIndex = dl.GetLineStation(_currentStationCode, _lineID).LineStationIndex - 1;
                try
                {
                    dl.GetAdjacentStations(list[prevStationIndex].Station, list[prevStationIndex + 2].Station);
                }
                catch (DO.BadAdjacentStationsException ex)
                {
                    double distance = this.distanceCalc(dl.GetStation(list[prevStationIndex].Station).Lattitude, dl.GetStation(list[prevStationIndex].Station).Longitude,
                        dl.GetStation(list[prevStationIndex + 2].Station).Lattitude, dl.GetStation(list[prevStationIndex + 2].Station).Longitude) / 1000;
                    DLAPI.DO.AdjacentStations adjacentStation = new DLAPI.DO.AdjacentStations()
                    {
                        Active = true,
                        Station1 = list[prevStationIndex].Station,
                        Station2 = list[prevStationIndex + 2].Station,
                        Distance = distance,
                        Time = new TimeSpan(0, (int)(distance * 1.5), 0)
                    };
                    dl.AddAdjacentStations(adjacentStation);
                }
            }
            dl.DeleteLineStation(_lineID, _currentStationCode);
            list = dl.GetRouteLine(_lineID).ToList();
            if (currentIndex != list.Last().LineStationIndex + 1)
            {
                for (int i = list.Count - 1; i > prevStationIndex; i--)
                {
                    dl.UpdateLineStation(_lineID, list[i].Station, lineStation => lineStation.LineStationIndex--);
      
                }
            }
            if (currentIndex == 0)
            {
                dl.UpdateLine(_lineID, lineStation => lineStation.FirstStation = list[0].Station);
                dl.GetLineStation(list[0].Station, _lineID).LineStationIndex = 0;
            }
            if (currentIndex == list.Last().LineStationIndex + 1)
                dl.UpdateLine(_lineID, lineStation => lineStation.LastStation = list.Last().Station);
        }
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
                        lineStation.DistanceFrom = dl.GetDistanceForTwoStations(prevLineStation.Station, lineStation.Station);
                        lineStation.TimeFrom = dl.GetTimeForTwoStations(prevLineStation.Station, lineStation.Station);
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
                    dl.UpdateStation(result);
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
            this.DeleteStationFromLines(_station.Code);
            dl.DeleteLineStations(_station.Code);
            try
            {
                dl.DeleteAdjacentStations(_station.Code);
            }
            catch (BO.BadAdjacentStationsException ex)
            {

                
            }
            if (dl.DeleteStation(_station.Code))
                return true;
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
        public bool UpdateStation(BL.BO.Station _updetedStation)
        {
            DLAPI.DO.Station updetedStation = new DLAPI.DO.Station()
            {
                Active = true,
                Code = _updetedStation.Code,
                Lattitude = _updetedStation.Lattitude,
                Longitude = _updetedStation.Longitude,
                Name = _updetedStation.Name
            };
            if (dl.UpdateStation(updetedStation))
                return true;
            return false;
        }

        public IEnumerable<BO.Station> GetAllStations(Predicate<BO.Station> predicate, IEnumerable<BO.Station> _stations)
        {
            return from station in _stations
                   where predicate(station)
                   select station;
        }
        #endregion

        #region AdjacentStations
        public void AddAdjacentStations(int _station1, int _station2)
        {
            try
            {
                dl.GetAdjacentStations(_station1, _station2);
            }
            catch (DO.BadAdjacentStationsException ex)
            {
                double _distance = this.distanceCalc(dl.GetStation(_station1).Lattitude, dl.GetStation(_station1).Longitude,
                dl.GetStation(_station2).Lattitude, dl.GetStation(_station2).Longitude);
                dl.AddAdjacentStations(new DLAPI.DO.AdjacentStations
                {
                    Active = true,
                    Station1 = _station1,
                    Station2 = _station2,
                    Distance = _distance,
                    Time = new TimeSpan(0, (int)(_distance * 1.5), 0)
                });
            }


        }
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
        public void AddBus(BO.Bus _bus)
        {
            DLAPI.DO.Bus newBus = new DLAPI.DO.Bus()
            {
                Active = true,
                LicenceNum = _bus.LicenceNum,
                TotalTrip = _bus.TotalTrip,
                FromDate = _bus.FromDate,
                FuelRemain = _bus.FuelRemain,
                Status = (DLAPI.DO.BusStatus)(int)_bus.Status,
            };
            try
            {
                dl.AddBus(newBus);

            }
            catch (DO.BadBusException ex)
            {

                throw new BO.BadBusException(ex.LicenseNum, ex.Message);
            }

        }
        public void DeleteBus(int _licenseNum)
        {
            try
            {
                dl.DeleteBus(_licenseNum);
            }
            catch (DO.BadBusException ex)
            {

                throw new BO.BadBusException(ex.LicenseNum, ex.Message);
            }
        }
        public void GetBus(int _licenseNum) { }
        public void UpdateBus(BO.Bus _bus)
        {
            DLAPI.DO.Bus busToUpdate = new DLAPI.DO.Bus()
            {
                Active = true,
                LicenceNum = _bus.LicenceNum,
                FromDate = _bus.FromDate,
                FuelRemain = _bus.FuelRemain,
                Status = (DLAPI.DO.BusStatus)(int)_bus.Status,
                TotalTrip = _bus.TotalTrip
            };
            try
            {
                dl.UpdateBus(busToUpdate);

            }
            catch (DO.BadBusException ex)
            {

                throw new BO.BadBusException(ex.LicenseNum, ex.Message);
            }
        }
        public IEnumerable<BO.Bus> GetAllBuses()
        {
            return from bus in dl.GetAllBuses()
                   select this.busDoBoAdapter(bus);
        }
        public IEnumerable<object> GetAllpropertiesToBuses(string property)
        {
            return dl.GetAllproperties(property);
        }
        public IEnumerable<BO.Bus> GetAllBuses(Predicate<BO.Bus> predicate, IEnumerable<BO.Bus> _buses)
        {
            return from bus in _buses
                   where predicate(bus)
                   select bus;
        }
        #endregion

        #region LineTrip
        public BO.LineTrip GetLineTrip(int _lineID, TimeSpan _startAt)
        {
            try
            {
                return this.lineTripAdapter(dl.GetLineTrip(_lineID, _startAt));
            }
            catch (DO.BadLineTripException ex)
            {
                throw new BO.BadLineTripException(ex.Message);
            }
        }
        public void AddLineTrip(BO.LineTrip _lineTrip)
        {
            dl.AddLineTrip(new DLAPI.DO.LineTrip()
            {
                Active = true,
                LineID = _lineTrip.LineID,
                StartAt = _lineTrip.StartAt
            });
        }
        public void UpdateLineTrip(BO.LineTrip _lineTrip)
        {
            try
            {
                DLAPI.DO.LineTrip lineTripToUpdate = new DLAPI.DO.LineTrip()
                {
                    Active = true,
                    LineID = _lineTrip.LineID,
                    LineTripID = _lineTrip.LineTripID,
                    StartAt = _lineTrip.StartAt
                };
                dl.UpdateLineTrip(lineTripToUpdate);

            }
            catch (DO.BadLineTripException ex)
            {

                throw new BO.BadLineTripException(ex.Message);
            }
        }
        public void UpdateLineTrip(int _lineID, Action<BO.LineTrip> update)
        {
        }
        public bool DeleteLineTrip(int _lineTripID)
        {
            try
            {
                dl.DeleteLineTrip(_lineTripID);
                return true;
            }
            catch (DO.BadLineTripException ex)
            {
                return false;
            }
        }
        public IEnumerable<BO.LineTrip> GetAllLinesTrip()
        {
            return from lineTrip in dl.GetAllLinesTrip()
                   select this.lineTripAdapter(lineTrip);
        }
        public IEnumerable<BO.LineTrip> GetAllLinesTrip(Predicate<BO.LineTrip> predicate)
        {
            return from lineTrip in this.GetAllLinesTrip()
                   where predicate(lineTrip)
                   select lineTrip;
        }
        public IEnumerable<BO.LineTrip> GetAllLinesTrip(Predicate<BO.LineTrip> predicate, IEnumerable<BO.LineTrip> _lineTrips)
        {
            return from lineTrip in _lineTrips
                   where predicate(lineTrip)
                   select lineTrip;
        }
        #endregion

        #region LineTiming
        public IEnumerable<BO.LineTiming> GetAllLineTimingPerStation(int _stationCode, TimeSpan _currentTime)
        {
            List<BO.LineTiming> LineTiminglist = new List<BO.LineTiming>();
            foreach (var line in this.GetAllLines(this.GetStation(_stationCode)))
            {
                var listLineTrip = from lineTrip in this.GetAllLinesTrip(_lineTrip =>
                  _lineTrip.LineID == line.LineID && _lineTrip.StartAt < _currentTime)
                                 select lineTrip;
                foreach (var lineTrip in listLineTrip)
                {
                    if ((lineTrip.StartAt + this.calcTotalTimeTrip(lineTrip.LineID, _stationCode)) > _currentTime)
                        LineTiminglist.Add(this.lineTripToLineTiming(lineTrip, _currentTime, _stationCode));

                }


            }

            return LineTiminglist;
        }

        private BO.LineTiming lineTripToLineTiming(BO.LineTrip _lineTrip, TimeSpan _currentTime, int _stationCode)
        {
            BO.LineTiming newLineTiming = new BO.LineTiming()
            {
                LineId = _lineTrip.LineID,
                LineCode = _lineTrip.LineCode,
                LastStation = _lineTrip.LastStationName,
                TripStart = _lineTrip.StartAt,
                ExpectedTimeTillArrive = calcArrival(_currentTime, _lineTrip, _stationCode)
            };
            return newLineTiming;
        }
        private TimeSpan calcArrival(TimeSpan _currentTime, BO.LineTrip _lineTrip, int _stationCode)
        {
            TimeSpan totalTimeTrip = calcTotalTimeTrip(_lineTrip.LineID, _stationCode);
            TimeSpan expectedTime =totalTimeTrip - (_currentTime - _lineTrip.StartAt);
            return expectedTime;
        }
        private TimeSpan calcTotalTimeTrip(int _lineID, int _stationCode)
        {
            TimeSpan totalTime = new TimeSpan(0,0,0);
            foreach (var lineStation in this.GetRouteLine(this.GetLine(_lineID)))
            {
                totalTime=totalTime.Add(lineStation.TimeFrom);
                if (lineStation.Station == _stationCode)
                    break;
            }
            return totalTime;
        }

        private double distanceCalc(double _latitude1, double _longitude1, double _latitude2, double _longitude2)
        {
            var sCoord = new GeoCoordinate(_latitude1, _longitude1);
            var eCoord = new GeoCoordinate(_latitude2, _longitude2);
            return sCoord.GetDistanceTo(eCoord);

        }

    }
    #endregion


}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using DLAPI;
//using DLAPI.DLAPI.DO;
using DS;

namespace DL
{
    sealed class DLObject : IDL   //internal
    {

        #region singelton
        static readonly DLObject instance = new DLObject();
        static DLObject() { }// static ctor to ensure instance init is done just before first usage
        DLObject() { } // default => private
        public static DLObject Instance { get => instance; }// The public Instance property to use
        #endregion

        #region Bus
        public DLAPI.DO.Bus GetBus(int _licenseNum)
        {
            return new DLAPI.DO.Bus();
        }
        public void AddBus(DLAPI.DO.Bus _bus) { }
        public void UpdateBus(DLAPI.DO.Bus _bus) { }
        public void UpdateBus(int _licenseNum, Action<DLAPI.DO.Bus> update) { } // Method that knows to updt specific fields in Bus.
        public void DeleteBus(int _licenseNum) { }
        public IEnumerable<DLAPI.DO.Bus> GetAllBuses()
        {
            return from bus in DataSource.ListBuses
                   select bus.Clone();
        }
        public IEnumerable<DLAPI.DO.Bus> GetAllBuses(Predicate<DLAPI.DO.Bus> predicate)
        {
            return new List<DLAPI.DO.Bus>();
        }
        #endregion

        #region Station
        public DLAPI.DO.Station GetStation(int _code)
        {
            DLAPI.DO.Station _station = DataSource.ListStations.Find(station => station.Code == _code
             && station.Active);
            if (_station != null)
                return _station;
            throw new DO.BadStationCodeException(_code);
        }
        public IEnumerable<DLAPI.DO.Station> GetStationPerName(string _name)
        {
            return from station in DataSource.ListStations
                   where station.Name == _name && station.Active
                   select station.Clone();
        }

        public void AddStation(DLAPI.DO.Station _station)
        {
            DataSource.ListStations.Add(_station);
        }
        public bool UpdateStation(int _thisStationCode, DLAPI.DO.Station _updetedStation)
        {
            DLAPI.DO.Station stationToUpdate = DataSource.ListStations.Find(station => station.Code == _thisStationCode);
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
                // stationToUpdate.Active = true;
                return true;
            }
            return false;
        }
        public void UpdateStation(int _code, Action<DLAPI.DO.Station> update) { } // Method that knows to updt specific fields in Station.
        public bool DeleteStation(int _code)
        {
            DLAPI.DO.Station stationToDelete = DataSource.ListStations.Find(station => station.Code == _code);
            if (stationToDelete != null)
            {
                stationToDelete.Active = false;
                return true;
            }
            return false;
        }
        public IEnumerable<DLAPI.DO.Station> GetAllStations()
        {
            return from station in DataSource.ListStations
                   where station.Active
                   select station.Clone();
        }
        public IEnumerable<DLAPI.DO.Station> GetAllStations(Predicate<DLAPI.DO.Station> predicate)
        {
            return from station in DataSource.ListStations
                   where station.Active && predicate(station)
                   select station.Clone();
        }
        public IEnumerable<object> GetAllPropertyStations(string property)
        {
            return from station in DataSource.ListStations
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

        #endregion

        #region Line
        public DLAPI.DO.Line GetLine(int _lineID)
        {
            DLAPI.DO.Line _line= DataSource.ListLines.Find(line => line.LineID == _lineID
            && line.Active);
            if (_line != null)
                return _line;
            throw new DO.BadLineIDException(_lineID);

        }
        public int AddLine(int _code, DLAPI.DO.Areas _area, int _firstStation, int _lastStatoin)
        {
            DLAPI.DO.Line newLine = new DLAPI.DO.Line();
            newLine.Active = true;
            newLine.Area = _area;
            newLine.Code = _code;
            newLine.LineID = DataSource.ListLines.Last().LineID+1;
            newLine.FirstStation = _firstStation;
            newLine.LastStation = _lastStatoin;
            DataSource.ListLines.Add(newLine);
            return newLine.LineID;
        }
        public void UpdateLine(DLAPI.DO.Line _line)
        {
        }
        public void UpdateLine(int _lineID, Action<DLAPI.DO.Line> update) { } // Method that knows to updt specific fields in Line.
        public void DeleteLine(int _lineID)
        {
            DLAPI.DO.Line _line = DataSource.ListLines.Find(line => line.LineID == _lineID);
            if (_line != null)
                _line.Active = false;
            else
                throw new DO.BadLineIDException(_lineID);
        }
        public IEnumerable<DLAPI.DO.Line> GetAllLines()
        {
            return from line in DataSource.ListLines
                   where line.Active
                   select line;
        }
        public IEnumerable<DLAPI.DO.Line> GetAllLines(Predicate<DLAPI.DO.Line> predicate)
        {
            return from line in DataSource.ListLines
                   where predicate(line) && line.Active
                   select line;
        }
        public IEnumerable<object> GetLineListWithSelectedFields(Func<DLAPI.DO.Line, object> generate)
        {
            return from line in DataSource.ListLines
                   where line.Active
                   select generate(line);
        }
        public IEnumerable<object> GetAllPropertyLines(string property)
        {
            return from line in DataSource.ListLines
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
            DLAPI.DO.LineStation _lineStation = DataSource.ListLineStations.Find(lineStation => lineStation.Station == _station
             && lineStation.LineID == _lineID && lineStation.Active);
            if (_lineStation != null)
                return _lineStation;
            throw new DO.BadLineStationException(_lineID,_station);

        }
        public void AddLineStation(int _lineID, int _stationCode, int _index) 
        {
            DLAPI.DO.LineStation lineStation= new DLAPI.DO.LineStation()
            {
                Active = true,
                Station = _stationCode,
                LineStationIndex = _index,
                LineID = _lineID
            };
            DataSource.ListLineStations.Add(lineStation);
        }
        public void UpdateLineStation(DLAPI.DO.LineStation _station) { }
        public void UpdateLineStation(int _lineID, int _stationCode, Action<DLAPI.DO.LineStation> update) { } // Method that knows to updt specific fields in LineStation.
        public void DeleteLineStation(int _lineID, int _stationCode)
        {
            DataSource.ListLineStations.Find(lineStation => lineStation.Station == _stationCode
            && lineStation.LineID == _lineID).Active = false;
        }
        public IEnumerable<DLAPI.DO.LineStation> GetRouteLine(int _lineID)
        {
            return from lineStation in DataSource.ListLineStations
                   where lineStation.LineID == _lineID && lineStation.Active
                   orderby lineStation.LineStationIndex
                   select lineStation.Clone();

        }
        public IEnumerable<DLAPI.DO.LineStation> GetRouteLine(Predicate<DLAPI.DO.LineStation> predicate)
        {
            return from lineStation in DataSource.ListLineStations
                   where predicate(lineStation) && lineStation.Active
                   select lineStation.Clone();
        }

        public IEnumerable<DLAPI.DO.LineStation> GetAllLineStations(Predicate<DLAPI.DO.LineStation> predicate)
        {
            return from lineStation in DataSource.ListLineStations
                   where predicate(lineStation) && lineStation.Active
                   select lineStation;
        }

        #endregion

        #region AdjacentStations
        public DLAPI.DO.AdjacentStations GetAdjacentStations(int _station1, int _station2)
        {
            DLAPI.DO.AdjacentStations _station = DataSource.ListAdjacentStations.Find(station => station.Station1 == _station1
             && station.Station2 == _station2 && station.Active);
            if (_station != null)
                return _station;
            throw new DO.BadAdjacentStationsException(_station1,_station2);


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
            DataSource.ListAdjacentStations.Add(_adjacentStations);
        }
        public void UpdateAdjacentStations(DLAPI.DO.AdjacentStations _adjacentStations) { }
        public void UpdateAdjacentStations(int _station1, int _station2, Action<DLAPI.DO.AdjacentStations> update) { } // Method that knows to updt specific fields in AdjacentStations.
        public void DeleteAdjacentStations(int _station1, int _station2) { }
        public IEnumerable<DLAPI.DO.AdjacentStations> GetAllAdjacentStations()
        {
            return new List<DLAPI.DO.AdjacentStations>();
        }
        public IEnumerable<DLAPI.DO.AdjacentStations> GetAllAdjacentStations(Predicate<DLAPI.DO.AdjacentStations> predicate)
        {
            return new List<DLAPI.DO.AdjacentStations>();
        }
        #endregion

        #region BusOnTrip
        public DLAPI.DO.BusOnTrip GetBusOnTrip(int _licenseNum)
        {
            return new DLAPI.DO.BusOnTrip();
        }
        public void AddBusOnTrip(DLAPI.DO.BusOnTrip _busOnTrip) { }
        public void UpdateBusOnTrip(DLAPI.DO.BusOnTrip _busOnTrip) { }
        public void UpdateBusOnTrip(int _licenseNum, Action<DLAPI.DO.BusOnTrip> update) { } // Method that knows to updt specific fields in BusOnTrip.
        public void DeleteBusOnTrip(int _licenseNum) { }
        public IEnumerable<DLAPI.DO.BusOnTrip> GetAllBusesOnTrip()
        {
            return new List<DLAPI.DO.BusOnTrip>();
        }
        public IEnumerable<DLAPI.DO.BusOnTrip> GetAllBusesOnTrip(Predicate<DLAPI.DO.BusOnTrip> predicate)
        {
            return new List<DLAPI.DO.BusOnTrip>();
        }
        #endregion

        #region LineTrip
        public DLAPI.DO.LineTrip GetLineTrip(int _lineID)
        {
            return new DLAPI.DO.LineTrip();
        }
        public void AddLineTrip(DLAPI.DO.LineTrip _lineTrip) { }
        public void UpdateLineTrip(DLAPI.DO.LineTrip _lineTrip) { }
        public void UpdateLineTrip(int _lineID, Action<DLAPI.DO.LineTrip> update) { } // Method that knows to updt specific fields in LineTrip.
        public void DeleteLineTrip(int _lineID) { }
        public IEnumerable<DLAPI.DO.LineTrip> GetAllLinesTrip()
        {
            return new List<DLAPI.DO.LineTrip>();
        }
        public IEnumerable<DLAPI.DO.LineTrip> GetAllLinesTrip(Predicate<DLAPI.DO.LineTrip> predicate)
        {
            return new List<DLAPI.DO.LineTrip>();
        }
        #endregion

        #region Trip
        public DLAPI.DO.Trip GetTrip(string _userName, int _inStationCode, int _outStationCode)
        {
            return new DLAPI.DO.Trip();
        }
        public void AddTrip(DLAPI.DO.Trip _trip) { }
        public void UpdateTrip(DLAPI.DO.Trip _trip) { }
        public void UpdateTrip(string _userName, int _inStationCode, int _outStationCode, Action<DLAPI.DO.Trip> update) { }// Method that knows to updt specific fields in Trip.
        public void DeleteTrip(string _userName, int _inStationCode, int _outStationCode) { }
        public IEnumerable<DLAPI.DO.Trip> GetAllTrips()
        {
            return new List<DLAPI.DO.Trip>();
        }
        public IEnumerable<DLAPI.DO.Trip> GetAllTrips(Predicate<DLAPI.DO.Trip> predicate)
        {
            return new List<DLAPI.DO.Trip>();
        }
        #endregion

        #region User
        public DLAPI.DO.User GetUser(string _userName)
        {
            DLAPI.DO.User currUser = DataSource.ListUsers.Find(p => p.UserName == _userName);
            return currUser;

        }
        public void AddUser(DLAPI.DO.User _user)
        {
            DataSource.ListUsers.Add(_user);
        }
        public void UpdateUser(DLAPI.DO.User _user)
        {
            DLAPI.DO.User currUser = DataSource.ListUsers.Find(p => p.UserName == _user.UserName);
            currUser.Password = _user.Password;
            currUser.Admin = _user.Admin;
        }
        public void UpdateUser(string _userName, Action<DLAPI.DO.User> update)
        {

        } // Method that knows to updt specific fields in User.
        public void DeleteUser(string _userName)
        {
            DataSource.ListUsers.Find(p => p.UserName == _userName).Active = false;

        }
        public IEnumerable<DLAPI.DO.User> GetAllUsers()
        {
            return from user in DataSource.ListUsers
                   select user.Clone();
        }
        public IEnumerable<DLAPI.DO.User> GetAllUsers(Predicate<DLAPI.DO.User> predicate)
        {
            return from user in DataSource.ListUsers
                   where predicate(user)
                   select user.Clone();
        }
        #endregion
    }
}
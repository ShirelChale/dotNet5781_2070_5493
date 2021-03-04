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
        public void AddBus(DLAPI.DO.Bus _bus)
        {
            DLAPI.DO.Bus busToAdd = DataSource.ListBuses.Find(bus => bus.LicenceNum == _bus.LicenceNum);
            if (busToAdd == null)
                DataSource.ListBuses.Add(_bus.Clone());
            else if (busToAdd.Active)
                throw new DO.BadBusException(_bus.LicenceNum, "Bus dousn't exist");

        }
        public void UpdateBus(DLAPI.DO.Bus _bus)
        {


            DLAPI.DO.Bus busToUpdate = DataSource.ListBuses.Find(bus => bus.LicenceNum == _bus.LicenceNum && bus.Active);
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
        }
        public void DeleteBus(int _licenseNum)
        {
            try
            {
                DataSource.ListBuses.Find(bus => bus.LicenceNum == _licenseNum && bus.Active).Active = false;
            }
            catch (NullReferenceException ex)
            {

                throw new DO.BadBusException(_licenseNum, "Bus dosen't exist");
            }
        }
        public IEnumerable<DLAPI.DO.Bus> GetAllBuses()
        {
            return from bus in DataSource.ListBuses
                   where bus.Active
                   select bus.Clone();
        }

        public IEnumerable<object> GetAllproperties(string property)
        {
            return from bus in DataSource.ListBuses
                   where bus.Active
                   orderby bus.LicenceNum
                   select this.getPropBus(bus, property);
        }

        
        #endregion

        #region Station
        public DLAPI.DO.Station GetStation(int _code)
        {
            DLAPI.DO.Station _station = DataSource.ListStations.Find(station => station.Code == _code
             && station.Active);
            if (_station != null)
                return _station.Clone();
            throw new DO.BadStationCodeException(_code);
        }
        public void AddStation(DLAPI.DO.Station _station)
        {
            DataSource.ListStations.Add(_station.Clone());
        }
        public bool UpdateStation(DLAPI.DO.Station _updetedStation)
        {
            DLAPI.DO.Station stationToUpdate = DataSource.ListStations.Find(station => station.Code == _updetedStation.Code);
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
            DLAPI.DO.Line _line = DataSource.ListLines.Find(line => line.LineID == _lineID
             && line.Active);
            if (_line != null)
                return _line.Clone();
            throw new DO.BadLineIDException(_lineID);

        }
        public int AddLine(int _code, DLAPI.DO.Areas _area, int _firstStation, int _lastStatoin)
        {
            DLAPI.DO.Line newLine = new DLAPI.DO.Line();
            newLine.Active = true;
            newLine.Area = _area;
            newLine.Code = _code;
            newLine.LineID = DataSource.ListLines.Last().LineID + 1;
            newLine.FirstStation = _firstStation;
            newLine.LastStation = _lastStatoin;
            DataSource.ListLines.Add(newLine.Clone());
            return newLine.LineID;
        }
        public void UpdateLine(int _lineID, int _stationCode, int firstOrLast)
        {
            if (firstOrLast == 0)
                DataSource.ListLines.Find(line => line.LineID == _lineID).FirstStation = _stationCode;
            else
                DataSource.ListLines.Find(line => line.LineID == _lineID).LastStation = _stationCode;

        }

        public void UpdateLine(int _lineID, Action<DLAPI.DO.Line> update)// Method that knows to updt specific fields in Line
        {
            try
            {
                DLAPI.DO.Line _currentLine = DataSource.ListLines.Find(_line => _line.LineID == _lineID);
                if (_currentLine != null)
                    update(_currentLine);
            }
            catch (DO.BadLineIDException ex)
            {
                throw ex;
            }
        }
        public void DeleteLine(int _lineID)
        {
            DLAPI.DO.Line _line = DataSource.ListLines.Find(line => line.LineID == _lineID);
            if (_line != null)
            {
                _line.Active = false;
                this.DeleteLineStation(_line.LineID, _line.FirstStation);
                this.DeleteLineStation(_line.LineID, _line.LastStation);
            }
            else
                throw new DO.BadLineIDException(_lineID);
        }
        public IEnumerable<DLAPI.DO.Line> GetAllLines()
        {
            return from line in DataSource.ListLines
                   where line.Active
                   select line.Clone();
        }
        public IEnumerable<DLAPI.DO.Line> GetAllLines(Predicate<DLAPI.DO.Line> predicate)
        {
            return from line in DataSource.ListLines
                   where predicate(line) && line.Active
                   select line.Clone();
        }
        public IEnumerable<object> GetLineListWithSelectedFields(Func<DLAPI.DO.Line, object> generate)
        {
            return from line in DataSource.ListLines
                   where line.Active
                   select generate(line.Clone());
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
                return _lineStation.Clone();
            throw new DO.BadLineStationException(_lineID, _station);

        }
        public void AddLineStation(int _lineID, int _stationCode, int _index)
        {
            DLAPI.DO.LineStation lineStation = new DLAPI.DO.LineStation()
            {
                Active = true,
                Station = _stationCode,
                LineStationIndex = _index,
                LineID = _lineID
            };
            DataSource.ListLineStations.Add(lineStation.Clone());
        }
        public void AddLineStation(DLAPI.DO.LineStation _lineStation)
        {
            DLAPI.DO.LineStation newLineStation = DataSource.ListLineStations.Find(station => station.Station == _lineStation.Station && station.LineID == _lineStation.LineID);
            if (newLineStation != null)
            {
                if (newLineStation.Active)
                    throw new DO.BadLineStationException(newLineStation.LineID, newLineStation.Station);
                newLineStation.LineStationIndex = _lineStation.LineStationIndex;
                newLineStation.Active = true;
            }
            DataSource.ListLineStations.Add(_lineStation.Clone());
        }
        public void UpdateLineStation(int _lineID, int _stationCode, Action<DLAPI.DO.LineStation> update)
        {
            DLAPI.DO.LineStation lineStationToUpdate = DataSource.ListLineStations.Find(_lineStation => _lineStation.LineID == _lineID && _lineStation.Station == _stationCode);
            if (lineStationToUpdate != null)
                update(lineStationToUpdate);
        } // Method that knows to updt specific fields in LineStation.
        public void DeleteLineStation(int _lineID, int _stationCode)
        {
            DataSource.ListLineStations.Find(lineStation => lineStation.Station == _stationCode
            && lineStation.LineID == _lineID).Active = false;
        }
        public void DeleteLineStations(int _stationCode)
        {
            foreach (var lineStation in DataSource.ListLineStations)
            {
                if (lineStation.Active)
                    if (lineStation.Station == _stationCode)
                        lineStation.Active = false;
            }
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
                   select lineStation.Clone();
        }

        #endregion

        #region AdjacentStations
        public DLAPI.DO.AdjacentStations GetAdjacentStations(int _station1, int _station2)
        {
            DLAPI.DO.AdjacentStations _station = DataSource.ListAdjacentStations.Find(station => station.Station1 == _station1
             && station.Station2 == _station2 && station.Active);
            if (_station != null)
                return _station.Clone();
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
            DataSource.ListAdjacentStations.Add(_adjacentStations.Clone());
        }
        public void DeleteAdjacentStations(int _stationCode)
        {
            foreach (var adjacentStation in DataSource.ListAdjacentStations)
            {
                if (adjacentStation.Active)
                    if (adjacentStation.Station1 == _stationCode || adjacentStation.Station2 == _stationCode)
                        adjacentStation.Active = false;
            }
        }

        #endregion

        #region LineTrip
        public DLAPI.DO.LineTrip GetLineTrip(int _lineID, TimeSpan _startAt)
        {
            DLAPI.DO.LineTrip lineTrip = DataSource.ListLinesTrip.Find(_lineTrip => _lineTrip.LineID == _lineID
             && _lineTrip.StartAt == _startAt);
            if (lineTrip != null && lineTrip.Active)
                return lineTrip.Clone();
            throw new DO.BadLineTripException("Line trip doesn't exist");
        }
        public void AddLineTrip(DLAPI.DO.LineTrip _lineTrip)
        {
            if (DataSource.ListLinesTrip.Find(lineTrip => lineTrip.LineID == _lineTrip.LineID && lineTrip.StartAt == _lineTrip.StartAt) == null)
            {
                _lineTrip.LineTripID = Config.LineTripID++;
                DataSource.ListLinesTrip.Add(_lineTrip.Clone());
            }
            else
                DataSource.ListLinesTrip.Find(lineTrip => lineTrip.LineID == _lineTrip.LineID && lineTrip.StartAt == _lineTrip.StartAt).Active = true;
        }
        public void UpdateLineTrip(DLAPI.DO.LineTrip _lineTrip)
        {
            try
            {
                DataSource.ListLinesTrip.Find(lineTrip => lineTrip.LineTripID == _lineTrip.LineTripID && _lineTrip.Active).StartAt = _lineTrip.StartAt;
            }
            catch (NullReferenceException ex)
            {

                throw new DO.BadLineTripException("Line trip doesn't exist");
            }
        }
        public void DeleteLineTrip(int _lineTripID)
        {

            try
            {
                DataSource.ListLinesTrip.Find(_lineTrip => _lineTrip.LineTripID == _lineTripID).Active = false;
            }
            catch (NullReferenceException ex)
            {
                throw new DO.BadLineTripException(_lineTripID, "Coudln't find LineTrip.");
            }

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
            return from lineTrip in DataSource.ListLinesTrip
                   where lineTrip.Active
                   orderby lineTrip.StartAt
                   select lineTrip.Clone();
        }
        public IEnumerable<DLAPI.DO.LineTrip> GetAllLinesTrip(Predicate<DLAPI.DO.LineTrip> predicate)
        {
            return from lineTrip in DataSource.ListLinesTrip
                   where lineTrip.Active && predicate(lineTrip)
                   orderby lineTrip.StartAt
                   select lineTrip.Clone();
        }
        #endregion

        #region User
        public DLAPI.DO.User GetUser(string _userName)
        {
            DLAPI.DO.User currUser = DataSource.ListUsers.Find(p => p.UserName == _userName);
            return currUser.Clone();

        }
        public void AddUser(DLAPI.DO.User _user)
        {
            DataSource.ListUsers.Add(_user.Clone());
        }
        public void UpdateUser(DLAPI.DO.User _user)
        {
            DLAPI.DO.User currUser = DataSource.ListUsers.Find(p => p.UserName == _user.UserName);
            currUser.Password = _user.Password;
            currUser.Admin = _user.Admin;
        }
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
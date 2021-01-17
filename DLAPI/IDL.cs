using System;
using System.Collections.Generic;

//using DO;

namespace DLAPI
{
    //CRUD Logic:
    // Create - add new instance
    // Request - ask for an instance or for a collection
    // Update - update properties of an instance
    // Delete - delete an instance
    public interface IDL
    {
        #region Bus
        DO.Bus GetBus(int _licenseNum);
        void AddBus(DO.Bus _bus);
        void UpdateBus(DO.Bus _bus);
        void UpdateBus(int _licenseNum, Action<DO.Bus> update); // Method that knows to updt specific fields in Bus.
        void DeleteBus(int _licenseNum);
        IEnumerable<DO.Bus> GetAllBuses();
        IEnumerable<DO.Bus> GetAllBuses(Predicate<DO.Bus> predicate);
        #endregion

        #region Station
        DO.Station GetStation(int _code);
        void AddStation(DO.Station _station);
        bool UpdateStation(int _thisStationCode, DLAPI.DO.Station _updetedStation);
        void UpdateStation(int _code, Action<DO.Station> update); // Method that knows to updt specific fields in Station.
        bool DeleteStation(int _code);
        IEnumerable<DO.Station> GetAllStations();
        IEnumerable<DO.Station> GetAllStations(Predicate<DO.Station> predicate);
        IEnumerable<object> GetAllPropertyStations(string property);

        #endregion

        #region Line
        DO.Line GetLine(int _lineID);
        int AddLine(int _code, DO.Areas _area, int _firstStation, int _lastStatoin);
        void UpdateLine(DO.Line _line);
        void UpdateLine(int _lineID, Action<DO.Line> update); // Method that knows to updt specific fields in Line.
        void DeleteLine(int _lineID);
        IEnumerable<DO.Line> GetAllLines();
        IEnumerable<DO.Line> GetAllLines(Predicate<DO.Line> predicate);
        IEnumerable<object> GetLineListWithSelectedFields(Func<DO.Line, object> generate);
        IEnumerable<object> GetAllPropertyLines(string property);

        #endregion

        #region LineStation
        DO.LineStation GetLineStation(int _station, int _lineID);
        void AddLineStation(int _lineID, int _stationCode, int _index);
        void UpdateLineStation(DO.LineStation _station);
        void UpdateLineStation(int _lineID, int _stationCode, Action<DO.LineStation> update); // Method that knows to updt specific fields in LineStation.
        void DeleteLineStation(int _lineID, int _stationCode);
        IEnumerable<DO.LineStation> GetRouteLine(int _lineID);
        IEnumerable<DO.LineStation> GetRouteLine(Predicate<DO.LineStation> predicate);
        IEnumerable<DO.LineStation> GetAllLineStations(Predicate<DO.LineStation> predicate);
        #endregion

        #region AdjacentStations
        DO.AdjacentStations GetAdjacentStations(int _station1, int _station2);
        double GetDistanceForTwoStations(int _station1, int _station2);
        TimeSpan GetTimeForTwoStations(int _station1, int _station2);
        void AddAdjacentStations(DO.AdjacentStations _adjacentStations);
        void UpdateAdjacentStations(DO.AdjacentStations _adjacentStations);
        void UpdateAdjacentStations(int _station1, int _station2, Action<DO.AdjacentStations> update); // Method that knows to updt specific fields in AdjacentStations.
        void DeleteAdjacentStations(int _station1, int _station2);
        IEnumerable<DO.AdjacentStations> GetAllAdjacentStations();
        IEnumerable<DO.AdjacentStations> GetAllAdjacentStations(Predicate<DO.AdjacentStations> predicate);
        #endregion

        #region BusOnTrip
        DO.BusOnTrip GetBusOnTrip(int _licenseNum);
        void AddBusOnTrip(DO.BusOnTrip _busOnTrip);
        void UpdateBusOnTrip(DO.BusOnTrip _busOnTrip);
        void UpdateBusOnTrip(int _licenseNum, Action<DO.BusOnTrip> update); // Method that knows to updt specific fields in BusOnTrip.
        void DeleteBusOnTrip(int _licenseNum);
        IEnumerable<DO.BusOnTrip> GetAllBusesOnTrip();
        IEnumerable<DO.BusOnTrip> GetAllBusesOnTrip(Predicate<DO.BusOnTrip> predicate);
        #endregion

        #region LineTrip
        DO.LineTrip GetLineTrip(int _lineID);
        void AddLineTrip(DO.LineTrip _lineTrip);
        void UpdateLineTrip(DO.LineTrip _lineTrip);
        void UpdateLineTrip(int _lineID, Action<DO.LineTrip> update); // Method that knows to updt specific fields in LineTrip.
        void DeleteLineTrip(int _lineID);
        IEnumerable<DO.LineTrip> GetAllLinesTrip();
        IEnumerable<DO.LineTrip> GetAllLinesTrip(Predicate<DO.LineTrip> predicate);
        #endregion

        #region Trip
        DO.Trip GetTrip(string _userName, int _inStationCode, int _outStationCode);
        void AddTrip(DO.Trip _trip);
        void UpdateTrip(DO.Trip _trip);
        void UpdateTrip(string _userName, int _inStationCode, int _outStationCode, Action<DO.Trip> update); // Method that knows to updt specific fields in Trip.
        void DeleteTrip(string _userName, int _inStationCode, int _outStationCode);
        IEnumerable<DO.Trip> GetAllTrips();
        IEnumerable<DO.Trip> GetAllTrips(Predicate<DO.Trip> predicate);
        #endregion

        #region User
        DO.User GetUser(string _userName);
        void AddUser(DO.User _user);
        void UpdateUser(DO.User _user);
        void UpdateUser(string _userName, Action<DO.User> update); // Method that knows to updt specific fields in User.
        void DeleteUser(string _userName);
        IEnumerable<DO.User> GetAllUsers();
        IEnumerable<DO.User> GetAllUsers(Predicate<DO.User> predicate);
        #endregion
    }
}

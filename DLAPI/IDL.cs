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
       /// <summary>
       /// Add DO.bus to the data
       /// </summary>
       /// <param name="_bus"></param>
        void AddBus(DO.Bus _bus);
        /// <summary>
        /// Update DO.bus and save updated bus in data
        /// </summary>
        /// <param name="_bus"></param>
        void UpdateBus(DO.Bus _bus);
        /// <summary>
        /// Delete DO.bus from the data
        /// </summary>
        /// <param name="_licenseNum"></param>
        void DeleteBus(int _licenseNum);
        /// <summary>
        /// Return all the buses in data
        /// </summary>
        /// <returns> IEnumerable<DO.Bus></returns>
        IEnumerable<DO.Bus> GetAllBuses();
        /// <summary>
        /// Get IEnumerable of Specific prop
        /// </summary>
        /// <param name="property"></param>
        /// <returns>IEnumerable<object></returns>
        IEnumerable<object> GetAllproperties(string property);
        #endregion

        #region Station
        DO.Station GetStation(int _code);
        void AddStation(DO.Station _station);
        bool UpdateStation(DLAPI.DO.Station _updetedStation);
        bool DeleteStation(int _code);
        IEnumerable<DO.Station> GetAllStations();
        IEnumerable<DO.Station> GetAllStations(Predicate<DO.Station> predicate);
        IEnumerable<object> GetAllPropertyStations(string property);

        #endregion

        #region Line
        DO.Line GetLine(int _lineID);
        int AddLine(int _code, DO.Areas _area, int _firstStation, int _lastStatoin);
        void UpdateLine(int _lineID, Action<DO.Line> update); // Method that knows to updt specific fields in Line.
        void UpdateLine(int _lineID, int _stationCode, int firstOrLast); // Method that knows to updt specific fields in Line.
        void DeleteLine(int _lineID);
        IEnumerable<DO.Line> GetAllLines();
        IEnumerable<DO.Line> GetAllLines(Predicate<DO.Line> predicate);
        IEnumerable<object> GetLineListWithSelectedFields(Func<DO.Line, object> generate);
        IEnumerable<object> GetAllPropertyLines(string property);

        #endregion

        #region LineStation
        DO.LineStation GetLineStation(int _station, int _lineID);
        void AddLineStation(int _lineID, int _stationCode, int _index);
        void AddLineStation(DO.LineStation _lineStation);
        void UpdateLineStation(int _lineID, int _stationCode, Action<DO.LineStation> update); // Method that knows to updt specific fields in LineStation.
        void DeleteLineStation(int _lineID, int _stationCode);
        void DeleteLineStations(int _stationCode);
        IEnumerable<DO.LineStation> GetRouteLine(int _lineID);
        IEnumerable<DO.LineStation> GetRouteLine(Predicate<DO.LineStation> predicate);
        IEnumerable<DO.LineStation> GetAllLineStations(Predicate<DO.LineStation> predicate);
        #endregion

        #region AdjacentStations
        DO.AdjacentStations GetAdjacentStations(int _station1, int _station2);
        double GetDistanceForTwoStations(int _station1, int _station2);
        TimeSpan GetTimeForTwoStations(int _station1, int _station2);
        void AddAdjacentStations(DO.AdjacentStations _adjacentStations);
        void DeleteAdjacentStations(int _stationCode);
        #endregion

        #region LineTrip
        DO.LineTrip GetLineTrip(int _lineID, TimeSpan _startAt);
        void AddLineTrip(DO.LineTrip _lineTrip);
        void UpdateLineTrip(DO.LineTrip _lineTrip);
        void DeleteLineTrip(int _lineTripID);
        void DeleteLineTripPerLine(int _lineID);

        IEnumerable<DO.LineTrip> GetAllLinesTrip();
        IEnumerable<DO.LineTrip> GetAllLinesTrip(Predicate<DO.LineTrip> predicate);
        #endregion

        #region User
        DO.User GetUser(string _userName);
        void AddUser(DO.User _user);
        void UpdateUser(DO.User _user);
        void DeleteUser(string _userName);
        IEnumerable<DO.User> GetAllUsers();
        IEnumerable<DO.User> GetAllUsers(Predicate<DO.User> predicate);
        #endregion
    }
}

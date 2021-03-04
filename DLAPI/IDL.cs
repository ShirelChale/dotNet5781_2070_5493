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
        /// Get all the buses in data
        /// </summary>
        /// <returns> IEnumerable<DO.Bus></returns>
        IEnumerable<DO.Bus> GetAllBuses();
        /// <summary>
        /// Get IEnumerable of Specific proprty in bus.
        /// </summary>
        /// <param name="property"></param>
        /// <returns>IEnumerable<object></returns>
        IEnumerable<object> GetAllproperties(string property);
        #endregion

        #region Station
        /// <summary>
        /// Get station
        /// </summary>
        /// <param name="_code"></param>
        /// <returns>DO.station</returns>
        DO.Station GetStation(int _code);
        /// <summary>
        /// Add station to data
        /// </summary>
        /// <param name="_station"></param>
        void AddStation(DO.Station _station);
        /// <summary>
        /// Update station in data
        /// </summary>
        /// <param name="_updetedStation"></param>
        /// <returns>True if the station updated successfully, otherwise return false</returns>
        bool UpdateStation(DLAPI.DO.Station _updetedStation);
        /// <summary>
        /// Delete station from data
        /// </summary>
        /// <param name="_code"></param>
        /// <returns>True if the station deleted successfully, otherwise return false</returns>
        bool DeleteStation(int _code);
        /// <summary>
        /// Get all the stations in data
        /// </summary>
        /// <returns>IEnumerable<DO.Station></returns>
        IEnumerable<DO.Station> GetAllStations();
        /// <summary>
        /// Get all stations that meet a certain condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>IEnumerable<DO.Station></returns>
        IEnumerable<DO.Station> GetAllStations(Predicate<DO.Station> predicate);
        /// <summary>
        /// Get IEnumerable of Specific proprty in station
        /// </summary>
        /// <param name="property"></param>
        /// <returns> IEnumerable<object></returns>
        IEnumerable<object> GetAllPropertyStations(string property);

        #endregion

        #region Line
        /// <summary>
        /// Get line
        /// </summary>
        /// <param name="_lineID"></param>
        /// <returns>DO.Line</returns>
        DO.Line GetLine(int _lineID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_code"></param>
        /// <param name="_area"></param>
        /// <param name="_firstStation"></param>
        /// <param name="_lastStatoin"></param>
        /// <returns>Return the lineID of the new line</returns>
        int AddLine(int _code, DO.Areas _area, int _firstStation, int _lastStatoin);
        /// <summary>
        /// Update line in data depending on the action received.
        /// </summary>
        /// <param name="_lineID"></param>
        /// <param name="update"></param>
        void UpdateLine(int _lineID, Action<DO.Line> update); // Method that knows to updt specific fields in Line.
        /// <summary>
        /// Update firstStation or lastStation in linede pending to parameter 'firstOrLast' received.
        /// </summary>
        /// <param name="_lineID"></param>
        /// <param name="_stationCode"></param>
        /// <param name="firstOrLast"></param>
        void UpdateLine(int _lineID, int _stationCode, int firstOrLast); // Method that knows to updt specific fields in Line.
        /// <summary>
        /// Delete line from data
        /// </summary>
        /// <param name="_lineID"></param>
        void DeleteLine(int _lineID);
        /// <summary>
        /// Get all lines exist in data
        /// </summary>
        /// <returns>IEnumerable<DO.Line></returns>
        IEnumerable<DO.Line> GetAllLines();
        /// <summary>
        ///Get all lines that meet a certain condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>IEnumerable<DO.Line></returns>
        IEnumerable<DO.Line> GetAllLines(Predicate<DO.Line> predicate);
        /// <summary>
        /// Get all lines that meet the func that recived.
        /// </summary>
        /// <param name="generate"></param>
        /// <returns></returns>
        IEnumerable<object> GetLineListWithSelectedFields(Func<DO.Line, object> generate);
        /// <summary>
        /// Get IEnumerable of Specific proprty in line
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IEnumerable<object> GetAllPropertyLines(string property);

        #endregion

        #region LineStation
        /// <summary>
        /// Get lineStation
        /// </summary>
        /// <param name="_station"></param>
        /// <param name="_lineID"></param>
        /// <returns> DO.LineStation </returns>
        DO.LineStation GetLineStation(int _station, int _lineID);
        /// <summary>
        /// Add lineStation to data
        /// </summary>
        /// <param name="_lineID"></param>
        /// <param name="_stationCode"></param>
        /// <param name="_index"></param>
        void AddLineStation(int _lineID, int _stationCode, int _index);
        /// <summary>
        /// Add lineStation to data
        /// </summary>
        /// <param name="_lineStation"></param>
        void AddLineStation(DO.LineStation _lineStation);
        /// <summary>
        /// Update lineStation 
        /// </summary>
        /// <param name="_lineID"></param>
        /// <param name="_stationCode"></param>
        /// <param name="update"></param>
        void UpdateLineStation(int _lineID, int _stationCode, Action<DO.LineStation> update); // Method that knows to updt specific fields in LineStation.
        /// <summary>
        /// Delete lineStation from data
        /// </summary>
        /// <param name="_lineID"></param>
        /// <param name="_stationCode"></param>
        void DeleteLineStation(int _lineID, int _stationCode);
        /// <summary>
        /// Delete lineStation from data
        /// </summary>
        /// <param name="_stationCode"></param>
        void DeleteLineStations(int _stationCode);
        /// <summary>
        /// Get all lineStations
        /// </summary>
        /// <param name="_lineID"></param>
        /// <returns>IEnumerable<DO.LineStation></returns>
        IEnumerable<DO.LineStation> GetRouteLine(int _lineID);
        /// <summary>
        ///Get Rout of line that meet a condition from filtered database
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<DO.LineStation>GetRouteLine(Predicate<DO.LineStation> predicate);
        /// <summary>
        ///Get all lineStations that meet a condition from filtered database
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns> IEnumerable<DO.LineStation></returns>
        IEnumerable<DO.LineStation> GetAllLineStations(Predicate<DO.LineStation> predicate);
        #endregion

        #region AdjacentStations
        /// <summary>
        /// Get adjacentStation object.
        /// </summary>
        /// <param name="_station1"></param>
        /// <param name="_station2"></param>
        /// <returns> DO.AdjacentStations</returns>
        DO.AdjacentStations GetAdjacentStations(int _station1, int _station2);
        /// <summary>
        /// Get distance for two stations
        /// </summary>
        /// <param name="_station1"></param>
        /// <param name="_station2"></param>
        /// <returns>double distance</returns>
        double GetDistanceForTwoStations(int _station1, int _station2);
        /// <summary>
        /// Get time for two stations
        /// </summary>
        /// <param name="_station1"></param>
        /// <param name="_station2"></param>
        /// <returns>TimeSpan time</returns>
        TimeSpan GetTimeForTwoStations(int _station1, int _station2);
        /// <summary>
        /// Add adjacentStation to data
        /// </summary>
        /// <param name="_adjacentStations"></param>
        void AddAdjacentStations(DO.AdjacentStations _adjacentStations);
        /// <summary>
        /// Delete adjacentStation from data
        /// </summary>
        /// <param name="_stationCode"></param>
        void DeleteAdjacentStations(int _stationCode);
        #endregion

        #region LineTrip
        /// <summary>
        /// Get lineTrip
        /// </summary>
        /// <param name="_lineID"></param>
        /// <param name="_startAt"></param>
        /// <returns>DO.LineTrip</returns>
        DO.LineTrip GetLineTrip(int _lineID, TimeSpan _startAt);
        /// <summary>
        /// Add lineTrip to data
        /// </summary>
        /// <param name="_lineTrip"></param>
        void AddLineTrip(DO.LineTrip _lineTrip);
        /// <summary>
        /// Update lineTrip in data
        /// </summary>
        /// <param name="_lineTrip"></param>
        void UpdateLineTrip(DO.LineTrip _lineTrip);
        /// <summary>
        /// Delete lineTrip from data
        /// </summary>
        /// <param name="_lineTripID"></param>
        void DeleteLineTrip(int _lineTripID);
        /// <summary>
        /// Delete all lineTrip per particular line
        /// </summary>
        /// <param name="_lineID"></param>
        void DeleteLineTripPerLine(int _lineID);
        /// <summary>
        /// Get all lineTrips exist in data
        /// </summary>
        /// <returns>IEnumerable<DO.LineTrip></returns>
        IEnumerable<DO.LineTrip> GetAllLinesTrip();
        /// <summary>
        /// Get all lineTrips that meet a condition from filtered database
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>IEnumerable<DO.LineTrip></returns>
        IEnumerable<DO.LineTrip> GetAllLinesTrip(Predicate<DO.LineTrip> predicate);
        #endregion

        #region User
        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="_userName"></param>
        /// <returns>DO.User </returns>
        DO.User GetUser(string _userName);
        /// <summary>
        /// Add user to data
        /// </summary>
        /// <param name="_user"></param>
        void AddUser(DO.User _user);
        /// <summary>
        /// Update user exist in data
        /// </summary>
        /// <param name="_user"></param>
        void UpdateUser(DO.User _user);
        /// <summary>
        /// Delete user form data
        /// </summary>
        /// <param name="_userName"></param>
        void DeleteUser(string _userName);
        /// <summary>
        /// Get all users that exist in data
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.User> GetAllUsers();
        /// <summary>
        ///Get all users that meet a condition from filtered database
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<DO.User> GetAllUsers(Predicate<DO.User> predicate);
        #endregion
    }
}

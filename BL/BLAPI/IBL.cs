using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using BO;


namespace BL.BLAPI
{
    public interface IBL
    {
        #region Line
        /// <summary>
        /// Add new Line to database.
        /// </summary>
        /// <param name="_code"></param>
        /// <param name="_rode"></param>
        /// <param name="_area"></param>
        void AddLine(int _code, IEnumerable<int> _rode, BO.Areas _area = BO.Areas.Jerusalem);

        /// <summary>
        /// Delete a Line from datebase.
        /// </summary>
        /// <param name="_lineID"></param>
        void DeleteLine(int _lineID);

        /// <summary>
        /// Delete Station from all lines in database.
        /// </summary>
        /// <param name="_stationCode"></param>
        void DeleteStationFromLines(int _stationCode);

        /// <summary>
        /// Get a Line object from database.
        /// </summary>
        /// <param name="_lineID"></param>
        /// <returns></returns>
        BO.Line GetLine(int _lineID);

        /// <summary>
        /// Update a Line.
        /// </summary>
        /// <param name="_lineID"></param>
        /// <param name="area"></param>
        /// <param name="lineCode"></param>
        void UpdateLine(int _lineID, BO.Areas area = BO.Areas.Jerusalem, int lineCode = -1);

        /// <summary>
        /// Get all Line objects from database.
        /// </summary>
        /// <returns></returns>
        IEnumerable<BO.Line> GetAllLines();

        /// <summary>
        /// Get all Line objects passing through a particular Station from database.
        /// </summary>
        /// <param name="_station"></param>
        /// <returns></returns>
        IEnumerable<BO.Line> GetAllLines(BO.Station _station);

        /// <summary>
        /// Get all Line objects with a particular last station code from database.
        /// </summary>
        /// <param name="_station"></param>
        /// <returns></returns>
        IEnumerable<BO.LinesPerStation> GetAllLinesCodeLastStation(BO.Station _station);

        /// <summary>
        /// Get all Line objects that meet a condition from database.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<BO.Line> GetAllLines(Predicate<BO.Line> predicate);

        /// <summary>
        /// Get all Line objects that meet a condition from filtered database.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="_lines"></param>
        /// <returns></returns>
        IEnumerable<BO.Line> GetAllLines(Predicate<BO.Line> predicate, IEnumerable<BO.Line> _lines);

        /// <summary>
        /// Get a particular property of all lines from database.
        /// </summary>
        /// <param name="_property"></param>
        /// <returns></returns>
        IEnumerable<object> GetAllPropertyLine(string _property);

        /// <summary>
        /// Get all Line objects with a particular last/first station code from filtered database.
        /// </summary>
        /// <param name="stationCode"></param>
        /// <param name="firstOrLastStation"></param>
        /// <param name="_lines"></param>
        /// <returns></returns>
        IEnumerable<BO.Line> GetAllLines(int stationCode, int firstOrLastStation, IEnumerable<BO.Line> _lines);
        #endregion

        #region LineStation
        /// <summary>
        /// Add new LineStation to database.
        /// </summary>
        /// <param name="_lineID"></param>
        /// <param name="_index"></param>
        /// <param name="_currentStationCode"></param>
        void AddLineStation(int _lineID, int _index, int _currentStationCode);

        /// <summary>
        /// Delete a LineStation from database.
        /// </summary>
        /// <param name="_lineID"></param>
        /// <param name="_currentStationCode"></param>
        void DeleteLineStation(int _lineID, int _currentStationCode);

        /// <summary>
        /// Get a LineStation object from database.
        /// </summary>
        /// <param name="_lineID"></param>
        /// <param name="_currentStation"></param>
        /// <param name="_prevStation"></param>
        /// <param name="_nextStation"></param>
        void GetLineStation(int _lineID, BO.Station _currentStation, BO.Station _prevStation, BO.Station _nextStation);

        /// <summary>
        /// Update a LineStation.
        /// </summary>
        /// <param name="_lineID"></param>
        /// <param name="_currentStation"></param>
        /// <param name="_prevStation"></param>
        /// <param name="_nextStation"></param>
        void UpdateLineStation(int _lineID, BO.Station _currentStation, BO.Station _prevStation, BO.Station _nextStation);

        /// <summary>
        /// Get all LineStation for a Line from database.
        /// </summary>
        /// <param name="_line"></param>
        /// <returns></returns>
        IEnumerable<BO.LineStation> GetRouteLine(BO.Line _line);

        /// <summary>
        /// Get all LineStation objects for a Line that meet a condition from database.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<BO.LineStation> GetRouteLine(Predicate<BO.LineStation> predicate);
        #endregion

        #region Station
        /// <summary>
        /// Add new Station to database.
        /// </summary>
        /// <param name="_code"></param>
        /// <param name="_name"></param>
        void AddStation(int _code, string _name);

        /// <summary>
        /// Add new Station object to database.
        /// </summary>
        /// <param name="_station"></param>
        /// <returns></returns>
        bool AddStation(BO.Station _station);

        /// <summary>
        /// Delete a Station from database.
        /// </summary>
        /// <param name="_code"></param>
        /// <param name="_name"></param>
        void DeleteStation(int _code, string _name);

        /// <summary>
        /// Delete a Station object from database.
        /// </summary>
        /// <param name="_station"></param>
        /// <returns></returns>
        bool DeleteStation(BO.Station _station);

        /// <summary>
        /// Get a Station object from database.
        /// </summary>
        /// <param name="_code"></param>
        /// <returns></returns>
        BL.BO.Station GetStation(int _code);

        /// <summary>
        /// Update a Station.
        /// </summary>
        /// <param name="_code"></param>
        /// <param name="_name"></param>
        void UpdateStation(int _code, string _name);

        /// <summary>
        /// Update a Station object.
        /// </summary>
        /// <param name="_updetedStation"></param>
        /// <returns></returns>
        bool UpdateStation(BL.BO.Station _updetedStation);

        /// <summary>
        /// Get all Station objects from database.
        /// </summary>
        /// <returns></returns>
        IEnumerable<BO.Station> GetAllStations();

        /// <summary>
        /// Get all Station objects that meet a condition from filtered database.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="_stations"></param>
        /// <returns></returns>
        IEnumerable<BO.Station> GetAllStations(Predicate<BO.Station> predicate, IEnumerable<BO.Station> _stations);

        /// <summary>
        /// Get a particular property of all stations from database.
        /// </summary>
        /// <param name="_property"></param>
        /// <returns></returns>
        IEnumerable<object> GetAllPropertyStations(string _property);
        #endregion

        #region User
        /// <summary>
        /// Add new User to database.
        /// </summary>
        /// <param name="_userName"></param>
        /// <param name="_password"></param>
        void AddUser(string _userName, string _password);

        /// <summary>
        /// Delete a User from database.
        /// </summary>
        /// <param name="_userName"></param>
        void DeleteUser(string _userName);

        /// <summary>
        /// Get a User object from database.
        /// </summary>
        /// <param name="_userName"></param>
        /// <returns></returns>
        BO.User GetUser(string _userName);

        /// <summary>
        /// Update a User.
        /// </summary>
        /// <param name="_userName"></param>
        /// <param name="_admin"></param>
        void UpdateUser(string _userName, bool _admin);

        /// <summary>
        /// Get all User objects from database.
        /// </summary>
        /// <returns></returns>
        IEnumerable<BO.User> GetAllUsers();

        /// <summary>
        /// Get all properties per User object from database.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> getAllpropertyPerUser();

        /// <summary>
        /// Get all User objects that meet a condition from database.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<BO.User> GetAllUsers(Predicate<BO.User> predicate);
        #endregion

        #region AdjacentStations
        /// <summary>
        /// Add new AdjacentStations to database.
        /// </summary>
        /// <param name="_station1"></param>
        /// <param name="_station2"></param>
        void AddAdjacentStations(int _station1, int _station2);

        /// <summary>
        /// Delete a AdjacentStations from database.
        /// </summary>
        /// <param name="_station1"></param>
        /// <param name="_station2"></param>
        void DeleteAdjacentStations(BO.Station _station1, BO.Station _station2);

        /// <summary>
        /// Get a AdjacentStations object from database.
        /// </summary>
        /// <param name="_station1"></param>
        /// <param name="_station2"></param>
        void GetAdjacentStations(BO.Station _station1, BO.Station _station2);

        /// <summary>
        /// Update a AdjacentStations.
        /// </summary>
        /// <param name="_station1"></param>
        /// <param name="_station2"></param>
        void UpdateAdjacentStations(BO.Station _station1, BO.Station _station2);

        /// <summary>
        /// Get all AdjacentStations for a Line from database.
        /// </summary>
        /// <param name="_line"></param>
        /// <returns></returns>
        IEnumerable<BO.AdjacentStations> GetAllAdjacentStations(BO.Line _line);

        /// <summary>
        /// Get all AdjacentStations objects that meet a condition from database.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<BO.AdjacentStations> GetAllAdjacentStations(Predicate<BO.AdjacentStations> predicate);
        #endregion

        #region Bus
        /// <summary>
        /// Add new Bus object to database.
        /// </summary>
        /// <param name="_bus"></param>
        void AddBus(BO.Bus _bus);

        /// <summary>
        /// Delete a Bus from database.
        /// </summary>
        /// <param name="_LicenseNum"></param>
        void DeleteBus(int _LicenseNum);

        /// <summary>
        /// Get a Bus object from database.
        /// </summary>
        /// <param name="_LicenseNum"></param>
        void GetBus(int _LicenseNum);

        /// <summary>
        /// Update a Bus.
        /// </summary>
        /// <param name="_bus"></param>
        void UpdateBus(BO.Bus _bus);

        /// <summary>
        /// Get all Bus objects from database.
        /// </summary>
        /// <returns></returns>
        IEnumerable<BO.Bus> GetAllBuses();

        /// <summary>
        /// Get a particular property of all buses from database.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IEnumerable<object> GetAllpropertiesToBuses(string property);

        /// <summary>
        /// Get all Bus objects that meet a condition from filtered database.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="_buses"></param>
        /// <returns></returns>
        IEnumerable<BO.Bus> GetAllBuses(Predicate<BO.Bus> predicate, IEnumerable<BO.Bus> _buses);

        #endregion

        #region LineTrip
        /// <summary>
        /// Get a LineTrip object from database.
        /// </summary>
        /// <param name="_lineID"></param>
        /// <param name="_startAt"></param>
        /// <returns></returns>
        BO.LineTrip GetLineTrip(int _lineID, TimeSpan _startAt);

        /// <summary>
        /// Add new LineTrip object to database.
        /// </summary>
        /// <param name="_lineTrip"></param>
        void AddLineTrip(BO.LineTrip _lineTrip);

        /// <summary>
        /// Update a LineTrip.
        /// </summary>
        /// <param name="_lineTrip"></param>
        void UpdateLineTrip(BO.LineTrip _lineTrip);

        /// <summary>
        /// Update a LineTrip with a particular update action.
        /// </summary>
        /// <param name="_lineID"></param>
        /// <param name="update"></param>
        void UpdateLineTrip(int _lineID, Action<BO.LineTrip> update); // Method that knows to updt specific fields in LineTrip.

        /// <summary>
        /// Delete a LineTrip from database.
        /// </summary>
        /// <param name="_lineTripID"></param>
        /// <returns></returns>
        bool DeleteLineTrip(int _lineTripID);

        /// <summary>
        /// Get all LineTrip objects from database.
        /// </summary>
        /// <returns></returns>
        IEnumerable<BO.LineTrip> GetAllLinesTrip();

        /// <summary>
        /// Get all LineTrip objects that meet a condition from database.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<BO.LineTrip> GetAllLinesTrip(Predicate<BO.LineTrip> predicate);

        /// <summary>
        /// Get all LineTrip objects that meet a condition from filtered database.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="_lineTrips"></param>
        /// <returns></returns>
        IEnumerable<BO.LineTrip> GetAllLinesTrip(Predicate<BO.LineTrip> predicate, IEnumerable<BO.LineTrip> _lineTrips);
        #endregion

        #region LineTiming
        /// <summary>
        /// Get all LineTiming for a station from database.
        /// </summary>
        /// <param name="_stationCode"></param>
        /// <param name="_currentTime"></param>
        /// <returns></returns>
        IEnumerable<BO.LineTiming> GetAllLineTimingPerStation(int _stationCode, TimeSpan _currentTime);
        #endregion
    }
}

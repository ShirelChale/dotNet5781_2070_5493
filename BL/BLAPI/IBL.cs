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
        void AddLine(int _code, IEnumerable<int> _rode, BO.Areas _area = BO.Areas.Jerusalem);
        void DeleteLine(int _lineID);
        void DeleteStationFromLines(int _stationCode);
        BO.Line GetLine(int _lineID);
        void UpdateLine(int _lineID, BO.Areas area = BO.Areas.Jerusalem, int lineCode = -1);
        IEnumerable<BO.Line> GetAllLines();
        IEnumerable<BO.Line> GetAllLines(BO.Station _station);
        IEnumerable<BO.LinesPerStation> GetAllLinesCodeLastStation(BO.Station _station);
        IEnumerable<BO.Line> GetAllLines(Predicate<BO.Line> predicate);
        IEnumerable<BO.Line> GetAllLines(Predicate<BO.Line> predicate, IEnumerable<BO.Line> _lines);
        IEnumerable<object> GetAllPropertyLine(string _property);
        IEnumerable<BO.Line> GetAllLines(int stationCode, int firstOrLastStation, IEnumerable<BO.Line> _lines);


        #endregion

        #region LineStation
        // void AddLineStation(int _lineID, DLAPI.DO.Station _currentStation, int _index);
        void AddLineStation(int _lineID, int _index, int _currentStationCode);
        void DeleteLineStation(int _lineID, int _currentStationCode);
        void GetLineStation(int _lineID, BO.Station _currentStation, BO.Station _prevStation, BO.Station _nextStation);
        void UpdateLineStation(int _lineID, BO.Station _currentStation, BO.Station _prevStation, BO.Station _nextStation);
        IEnumerable<BO.LineStation> GetRouteLine(BO.Line _line);
        IEnumerable<BO.LineStation> GetRouteLine(Predicate<BO.LineStation> predicate);


        #endregion

        #region Station
        void AddStation(int _code, string _name);
        bool AddStation(BO.Station _station);
        void DeleteStation(int _code, string _name);
        bool DeleteStation(BO.Station _station);
        BL.BO.Station GetStation(int _code);
        void UpdateStation(int _code, string _name);
        bool UpdateStation( BL.BO.Station _updetedStation);
        IEnumerable<BO.Station> GetAllStations();
        IEnumerable<BO.Station> GetAllStations(Predicate<BO.Station> predicate, IEnumerable<BO.Station> _stations);
        IEnumerable<object> GetAllPropertyStations(string _property);

        #endregion

        #region User
        void AddUser(string _userName, string _password);
        void DeleteUser(string _userName);
        BO.User GetUser(string _userName);
        void UpdateUser(string _userName, bool _admin);
        IEnumerable<BO.User> GetAllUsers();
        IEnumerable<string> getAllpropertyPerUser();
        IEnumerable<BO.User> GetAllUsers(Predicate<BO.User> predicate);

        #endregion

        #region AdjacentStations
        void AddAdjacentStations(int _station1, int _station2);
        void DeleteAdjacentStations(BO.Station _station1, BO.Station _station2);
        void GetAdjacentStations(BO.Station _station1, BO.Station _station2);
        void UpdateAdjacentStations(BO.Station _station1, BO.Station _station2);
        IEnumerable<BO.AdjacentStations> GetAllAdjacentStations(BO.Line _line);
        IEnumerable<BO.AdjacentStations> GetAllAdjacentStations(Predicate<BO.AdjacentStations> predicate);

        #endregion

        #region Bus
        void AddBus(BO.Bus _bus);
        void DeleteBus(int _LicenseNum);
        void GetBus(int _LicenseNum);
        void UpdateBus(BO.Bus _bus);
        IEnumerable<BO.Bus> GetAllBuses();
        IEnumerable<object> GetAllpropertiesToBuses(string property);
        IEnumerable<BO.Bus> GetAllBuses(Predicate<BO.Bus> predicate, IEnumerable<BO.Bus> _buses);

        #endregion

        #region LineTrip
        BO.LineTrip GetLineTrip(int _lineID, TimeSpan _startAt);
        void AddLineTrip(BO.LineTrip _lineTrip);
        void UpdateLineTrip(BO.LineTrip _lineTrip);
        void UpdateLineTrip(int _lineID, Action<BO.LineTrip> update); // Method that knows to updt specific fields in LineTrip.
        bool DeleteLineTrip(int _lineTripID);
        IEnumerable<BO.LineTrip> GetAllLinesTrip();
        IEnumerable<BO.LineTrip> GetAllLinesTrip(Predicate<BO.LineTrip> predicate);
        IEnumerable<BO.LineTrip> GetAllLinesTrip(Predicate<BO.LineTrip> predicate, IEnumerable<BO.LineTrip> _lineTrips);
        #endregion

        #region LineTiming
        IEnumerable<BO.LineTiming> GetAllLineTimingPerStation(int _stationCode, TimeSpan _currentTime);
        #endregion


    }
}

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
        BO.Line GetLine(int _lineID);
        void UpdateLine(int _lineID);
        IEnumerable<BO.Line> GetAllLines();
        IEnumerable<BO.Line> GetAllLines(BO.Station _station);
        IEnumerable<BO.LinesPerStation> GetAllLinesCodeLastStation(BO.Station _station);
        IEnumerable<BO.Line> GetAllLines(Predicate<BO.Line> predicate);
        IEnumerable<object> GetAllPropertyLine(string _property);
        IEnumerable<BO.Line> GetAllLines(int stationCode, int firstOrLastStation);


        #endregion

        #region LineStation
        void AddLineStation(int _lineID, DLAPI.DO.Station _currentStation, int _index);
        void DeleteLineStation(int _lineID, BO.Station _currentStation, BO.Station _prevStation, BO.Station _nextStation);
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
        bool UpdateStation(BL.BO.Station _thisStation, BL.BO.Station _updetedStation);
        IEnumerable<BO.Station> GetAllStations();
        IEnumerable<BO.Station> GetAllStations(Predicate<BO.Station> predicate);
        IEnumerable<object> GetAllPropertyStations(string _property);

        #endregion

        #region User
        void AddUser(string _userName, string _password);
        void DeleteUser(string _userName);
        BO.User GetUser(string _userName);
        void UpdateUser(string _userName);
        IEnumerable<BO.User> GetAllUsers(BO.Line _line);
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
        void AddBus(int _licenceNum);
        void DeleteBus(int _licenceNum);
        void GetBus(int _licenceNum);
        void UpdateBus(int _licenceNum);
        IEnumerable<BO.Bus> GetAllBuses();
        IEnumerable<BO.Bus> GetAllBuses(Predicate<BO.Bus> predicate);

        #endregion
    }
}

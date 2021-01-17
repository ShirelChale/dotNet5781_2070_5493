using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    [Serializable]
    public class BadStationCodeException : Exception
    {
        public int Code;
        public BadStationCodeException(int code) : base() => Code = code;
        public BadStationCodeException(int code, string message) :
            base(message) => Code = code;
        public BadStationCodeException(int code, string message, Exception innerException) : 
            base(message, innerException) => Code = code;
      
        public override string ToString() => base.ToString() + $", bad station id: {Code}";
    }

    public class BadLineIDException : Exception
    {
        public int lineID;
        public BadLineIDException(int _lineID) : base() { lineID = _lineID; }  
        public BadLineIDException(int _lineID, string message) :
            base(message) { lineID = _lineID; }
        public BadLineIDException(int _lineID,string message, Exception innerException) :
            base(message, innerException) { lineID = _lineID; }

        public override string ToString() => base.ToString() + $", bad line id: {lineID}";
    }

    public class BadLineStationException : Exception
    {
        public int lineID;
        public int stationCode;

        public BadLineStationException(int _lineID, int _stationCode) : base() { lineID = _lineID; stationCode = _stationCode; }
        public BadLineStationException(int _lineID, int _stationCode, string message) :
            base(message)
        { lineID = _lineID; stationCode = _stationCode; }
        public BadLineStationException(int _lineID, int _stationCode,string message, Exception innerException) :
            base(message, innerException)
        { lineID = _lineID; stationCode = _stationCode; }

        public override string ToString() => base.ToString() + $", bad lineStation Line ID: {lineID} and bad station code: {stationCode}";
    }
    public class BadAdjacentStationsException : Exception
    {
        public int station1;
        public int station2;

        public BadAdjacentStationsException(int _station1, int _station2) : base() { station1 = _station1; station2 = _station2; }
        public BadAdjacentStationsException(int _station1, int _station2, string message) :
            base(message)
        { station1 = _station1; station2 = _station2; }
        public BadAdjacentStationsException(int _station1, int _station2, string message, Exception innerException) :
            base(message, innerException)
        { station1 = _station1; station2 = _station2; }

        public override string ToString() => base.ToString() + $", bad AdjacentStation codes for stations: {station1} and  {station2}";
    }



    //public class XMLFileLoadCreateException : Exception
    //{
    //    public string xmlFilePath;
    //    public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
    //    public XMLFileLoadCreateException(string xmlPath, string message) :
    //        base(message)
    //    { xmlFilePath = xmlPath; }
    //    public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) :
    //        base(message, innerException)
    //    { xmlFilePath = xmlPath;}

    //    public override string ToString() => base.ToString() + $", fail to load or create xml file: {xmlFilePath}";
}



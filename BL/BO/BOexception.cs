﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DO;

namespace BL.BO
{
    [Serializable]
    public class BadStationException : Exception
    {
        public int Code;
        public BadStationException(string message, Exception innerException) :
            base(message, innerException) => Code = ((DO.BadStationCodeException)innerException).Code;
        public override string ToString() => base.ToString() + $", bad station code: {Code}";
    }

    [Serializable]
    public class BadLineIDException : Exception
    {
        public int lineID;
        public BadLineIDException(string message, Exception innerException) :
            base(message, innerException) => lineID = ((DO.BadLineIDException)innerException).lineID;
        public override string ToString() => base.ToString() + $", bad line id: {lineID}";
    }

    [Serializable]
    public class BadLineStationException : Exception
    {
        public int lineID;
        public int stationCode;
        public BadLineStationException(string message, Exception innerException) :
            base(message, innerException)
        {
            lineID = ((DO.BadLineStationException)innerException).lineID;
            stationCode = ((DO.BadLineStationException)innerException).stationCode;
        }
        public override string ToString() => base.ToString() + $", bad line station Line ID: {lineID} and station code: {stationCode}";
    }
    public class BadAdjacentStationsException : Exception
    {
        public int station1;
        public int station2;
        public BadAdjacentStationsException(string message, Exception innerException) :
            base(message, innerException)
        {
            station1 = ((DO.BadAdjacentStationsException)innerException).station1;
            station2 = ((DO.BadAdjacentStationsException)innerException).station2;
        }
        public override string ToString() => base.ToString() + $", bad AdjacentStation codes for stations: {station1} and  {station2}";
    }

}

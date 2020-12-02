using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_2070_5493
{
    /// <summary>
    /// Initializing bus line collection.
    /// </summary>
    public class BusesLineCollection: IEnumerable<BusesLine>
    {
        private Collection<BusesLine> linesCollection;
        // Indexer:
        public BusesLine this[int index]
        {
            get
            {

                return linesCollection[index];
            }
            set
            {
                linesCollection[index] = value;
            }
        }
        public BusesLineCollection(Collection<BusesLine> _linesCollection)
        { // Constructor.
            this.LinesCollection = _linesCollection;
        }
        public BusesLineCollection()
        { // Defualt constructor.
            this.LinesCollection = new Collection<BusesLine>();
        }

        // Property:
        public Collection<BusesLine> LinesCollection
        {
            get => linesCollection;
            set => linesCollection = value;
        }

        /// <summary>
        /// Adds a new bus line to collection.
        /// </summary>
        /// <param name="bus"></param>
        public void addLine(BusesLine bus)
        {
            if (linesCollection.Contains(bus))
                // Check if the line exist in the collection.
                return;
            linesCollection.Add(bus);
        }

        /// <summary>
        /// Remove a bus line from a collection.
        /// </summary>
        /// <param name="bus"></param>
        public void removeLine(BusesLine bus)
        {
            if (linesCollection.Contains(bus))
                // Check if the line exist in the collection.
                linesCollection.Remove(bus);
            return;
        }

        /// <summary>
        /// Checks for all buses line for specific satation
        /// </summary>
        /// <param name="stationID"></param>
        /// <returns>List of buses line</returns>
        public List<BusesLine> allBusesForStation(double stationID)
        {
            List<BusesLine> busListForStation = new List<BusesLine>();
            LineStation station = new LineStation();
            station.BusStationKey = stationID;
            foreach (BusesLine line in linesCollection)
            {
                if (line.search(stationID))
                    busListForStation.Add(line);
            }
            return busListForStation;
        }

        /// <summary>
        /// Sorts all buses by arriving time
        /// </summary>
        /// <returns>List of buses sorted by arriving time</returns>
        public List<BusesLine> sortAllBuses()
        {
            List<BusesLine> sortedList = new List<BusesLine>(linesCollection.ToList<BusesLine>());
            sortedList.Sort();
            return sortedList;
        }

        /// <summary>
        /// Get enumerator for bus line collection
        /// </summary>
        /// <returns>an enumerator for bus line collection</returns>
        public IEnumerator<BusesLine> GetEnumerator()
        {
            for (int i = 0; i < linesCollection.Count(); i++)
            {
                yield return linesCollection[i];
            } 
        }

        /// <summary>
        /// Implementation for IEnumerable.GetEnumerator
        /// </summary>
        /// <returns>an enumerator for bus line collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)linesCollection).GetEnumerator();
        }
    }
}

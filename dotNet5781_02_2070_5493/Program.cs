using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_2070_5493
{
    class Program
    {
        enum Area { Jerusalem, Har_Nof, Givat_Shaul };
        enum choices
        {
            exit, addBus, addStation, removeBus, RemoveStation, searchByStation,
            busTime, allBuses, allStation
        };

        /// <summary>
        /// Main
        /// </summary>
        static void Main(string[] args)
        {
            List<LineStation> stationsData = setStationsList();
            BusesLineCollection linesData = setBusLinecollection(stationsData);

            double lineNumber;
            double StationKey;
            int areaNumber;
            double firstStationKey;
            double secondStationKey;
            bool flag = false;
            choices choice = choices.addBus; // Reset.

            while (choice != 0)
            {
                Console.WriteLine("What is your request?\n" +
                    "(0) Exit\n" +
                    "(1) Add a bus line\n" +
                    "(2) Add a line station\n" +
                    "(3) Remove a bus line\n" +
                    "(4) Remove a line station\n" +
                    "(5) Search all lines passing through a station\n" +
                    "(6) Find shortest rode between 2 stations\n" +
                    "(7) Display all bus lines\n" +
                    "(8) Display all stations and their bus lines\n");
                choices.TryParse(Console.ReadLine(), out choice);
                try
                {
                    switch (choice)
                    {
                        case choices.addBus: // Add a bus line.
                            Console.WriteLine("Please enter new bus line.");
                            double.TryParse(Console.ReadLine(), out lineNumber);
                            Console.WriteLine("Please enter line area.\n" +
                                "(0) Jerusalem\n" + "(1) Har_Nof\n" + "(2) Givat_Shaul\n");
                            int.TryParse(Console.ReadLine(), out areaNumber);
                            Console.WriteLine("Please enter first station.");
                            double.TryParse(Console.ReadLine(), out firstStationKey);
                            Console.WriteLine("Please enter last station.");
                            double.TryParse(Console.ReadLine(), out secondStationKey);
                            bool flag1 = false;
                            bool flag2 = false;
                            LineStation firstStation = new LineStation();
                            LineStation secondStation = new LineStation();
                            foreach (LineStation station in stationsData)
                            {
                                if (station.BusStationKey == firstStationKey)
                                { // The first station found in data.
                                    firstStation = station;
                                    flag1 = true;
                                }
                                if (station.BusStationKey == secondStationKey)
                                { // The second station found in data.
                                    flag2 = true;
                                    secondStation = station;
                                }
                            }
                            if (flag1 && flag2)
                            { // Checks if both stations found.
                                BusesLine newLine = new BusesLine(lineNumber, firstStation, secondStation, areaNumber);
                                linesData.addLine(newLine); // Adds line to data.
                            }
                            else // If one station or more not found
                                throw new myException("One or more of the stations not exist!");
                            break;

                        case choices.addStation: // Add a line station.
                            Console.WriteLine("Please enter bus line which you'd like to add a new station.");
                            double.TryParse(Console.ReadLine(), out lineNumber);
                            flag = false;
                            foreach (BusesLine line in linesData)
                            {
                                if (line.BusLine == lineNumber)
                                { // Wanted line found.
                                    Console.WriteLine("Please enter wanted station.");
                                    double.TryParse(Console.ReadLine(), out StationKey);
                                    foreach (LineStation station in stationsData)
                                    {
                                        if (station.BusStationKey == StationKey)
                                        { // Check if station exists in data.
                                            line.addStation(station);
                                            flag = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (!flag) // Line not exists in data.
                                throw new myException("Line not exist!");
                            break;

                        case choices.removeBus: // Remove a bus line.
                            Console.WriteLine("Please enter bus line which you'd like to remove.");
                            double.TryParse(Console.ReadLine(), out lineNumber);
                            foreach (BusesLine line in linesData)
                            { // Checks if line exists in data.
                                if (line.BusLine == lineNumber) // Wanted line found.
                                    linesData.removeLine(line);
                            }
                            break;

                        case choices.RemoveStation: // Remove a line station.
                            Console.WriteLine("Please enter bus line which you'd like to remove a existed station.");
                            double.TryParse(Console.ReadLine(), out lineNumber);
                            flag = false;
                            foreach (BusesLine line in linesData)
                            {
                                if (line.BusLine == lineNumber)
                                { // Checks line exists in data.
                                    Console.WriteLine("Please enter wanted station.");
                                    double.TryParse(Console.ReadLine(), out StationKey);
                                    foreach (LineStation station in stationsData)
                                    {
                                        if (station.BusStationKey == StationKey)
                                        { // Wanted station found.
                                            line.removeStation(station);
                                            flag = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (!flag) // Station not exist in data.
                                throw new myException("Station not exist!");
                            break;

                        case choices.searchByStation: // Search all lines passing through a station.
                            Console.WriteLine("Please enter wanted station.");
                            double.TryParse(Console.ReadLine(), out StationKey);
                            flag = false;
                            foreach (LineStation station in stationsData)
                            { 
                                if (station.BusStationKey == StationKey)
                                { // Checks if station exist in data.
                                    flag = true;
                                    break;
                                }
                            }
                            if (!flag) // if station not exist in data.
                                throw new myException("Station not exist!");
                            flag = false;
                            Console.WriteLine("Lines passing through station " + StationKey + ":\n");
                            List<BusesLine> stationLines = new List<BusesLine>(linesData.allBusesForStation(StationKey));
                            foreach (BusesLine line in stationLines)
                            { // Prints lines.
                                Console.WriteLine("Line number: " + line.BusLine.ToString());
                            }
                            if (!stationLines.Any()) // If no line passing through the station.
                                throw new myException("No lines found.");
                            break;

                        case choices.busTime: // Find shortest rode between 2 stations.
                            Console.WriteLine("Please enter departure station:\n");
                            double.TryParse(Console.ReadLine(), out firstStationKey);
                            Console.WriteLine("Please enter destination station:\n");
                            double.TryParse(Console.ReadLine(), out secondStationKey);
                            flag1 = false;
                            flag2 = false;
                            firstStation = new LineStation();
                            secondStation = new LineStation();
                            foreach (LineStation station in stationsData)
                            {
                                if (station.BusStationKey == firstStationKey)
                                { // First station found in data.
                                    firstStation = station;
                                    flag1 = true;
                                }
                                if (station.BusStationKey == secondStationKey)
                                { // Last station found in data.
                                    secondStation = station;
                                    flag2 = true;
                                }

                                if (flag1 && flag2) // If both stations found in data.
                                    break;
                            }
                            if (!flag1 || !flag2) // If one of the stations or more not exists in data.
                                throw new myException("One or more of the stations not exist!");

                            if (firstStation.BusStationKey == secondStation.BusStationKey)
                                // If first and last stations are the same.
                                throw new myException("Stations are the same. There's no rode exist!");
                            BusesLineCollection subLineCollection = new BusesLineCollection();
                            // Create a collection of all lines which fassing through both stations:
                            foreach (BusesLine line in linesData)
                            {
                                if (line.search(firstStationKey) && line.search(secondStationKey))
                                    // Checks if both stations are on the same rode.
                                    subLineCollection.addLine(line.subBusLine(firstStation, secondStation));
                            }
                            BusesLineCollection collectionToPrint = new BusesLineCollection(new ObservableCollection<BusesLine>
                                 (subLineCollection.sortAllBuses().Distinct()));
                            // Prints results:
                            print(collectionToPrint, false);
                            break;

                        case choices.allBuses: // Display all bus lines.
                            Console.WriteLine("All buses exist list:\n");
                            print(linesData, true);
                            break;

                        case choices.allStation: // Display all stations and their bus lines.
                            Console.WriteLine("All stations and their buses list:\n");
                            foreach (LineStation station in stationsData)
                            {
                                Console.WriteLine(station.ToString());
                                foreach (BusesLine line in linesData)
                                {
                                    if (line.search(station.BusStationKey))
                                        // Checks if line is passing through station.
                                        Console.WriteLine("Line: " + line.BusLine + "\n");
                                }
                            }
                            break;
                        case choices.exit: // Exit.
                            Console.WriteLine("Bye!\n");
                            break;
                        default:
                            Console.WriteLine("Illegal input! Try again.\n");
                            break;

                    }
                }
                catch (myException newException)
                {
                    Console.WriteLine(newException.ToString());
                }
            }
        }

        /// <summary>
        /// Sets stations list
        /// </summary>
        /// <returns>List of stations</returns>
        private static List<LineStation> setStationsList()
        {
            List<LineStation> stationsList = new List<LineStation>();//Data
            double _busStationKey = 100000;
            Random randomLandmark = new Random();
            double _latitude;
            double _longitude;

            for (int i = 0; i < 40; i++)
            {
                _latitude = randomLandmark.Next(31, 33) + randomLandmark.NextDouble();
                _longitude = randomLandmark.Next(34, 35) + randomLandmark.NextDouble();
                LineStation newStation = new LineStation(_busStationKey + i, _latitude, _longitude, "");
                stationsList.Add(newStation);
            }
            return stationsList;
        }

        /// <summary>
        /// Sets buses line list
        /// </summary>
        /// <param name="stationsList"></param>
        /// <returns>Collction of buses line</returns>
        private static BusesLineCollection setBusLinecollection(List<LineStation> stationsList)
        {
            BusesLineCollection busCollection = new BusesLineCollection();
            double _busLine = 1;
            int _area = 0;
            Random randomArea = new Random(0);
            List<LineStation> rode = new List<LineStation>();
            for (int i = 0; i < 10; i++)
            {
                _area = randomArea.Next(0, 3);
                BusesLine bus = new BusesLine(_busLine + i, stationsList[i], _area);
                busCollection.addLine(bus);
            }
            int j = 9;
            for (int i = 0; i < 10; i++)
            {
                busCollection[j--].addStation(stationsList[i]);
            }
            Random randomLine = new Random(0);
            foreach (LineStation station in stationsList)
            {
                int lineIndex = randomLine.Next(0, 9);
                busCollection[lineIndex].addStation(station);
            }
            return busCollection;
        }

        /// <summary>
        /// Prints buses line list and its station (Option)
        /// </summary>
        /// <param name="collectionToPrint"></param>
        /// <param name="printStation"></param>
        private static void print(BusesLineCollection collectionToPrint, bool printStation)
        {
            foreach (BusesLine line in collectionToPrint)
            {
                Console.WriteLine("Line: " + line.BusLine.ToString() + "\n");
                if (printStation)
                    foreach (LineStation station in line.Stations)
                    {
                        Console.WriteLine(station.ToString());
                    }
            }
        }
    }
}

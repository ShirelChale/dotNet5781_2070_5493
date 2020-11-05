using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_2070_5493
{

    class MainProgram
    {
        enum choices { exit, add, select, operations, display };
        static void Main(string[] args)
        {
            List<Bus> busList = new List<Bus>();
            choices choice = choices.add; // Reset.
            string licenseNumber;
            DateTime date;

            while (choice != 0)
            {
                Console.WriteLine("What is your request?\n" +
                    "(0) Exit\n" +
                    "(1) Add a bus\n" +
                    "(2) Select a ride\n" +
                    "(3) Operations: refuel or make a treatment\n" +
                    "(4) Display bus license number and its kilometer data for all buses");
                choices.TryParse(Console.ReadLine(), out choice);
                switch (choice)
                {
                    case choices.add: // Add a new bus to data.
                        Console.WriteLine("Please enter new bus license number.");
                        licenseNumber = Console.ReadLine();
                        if (licenseNumber.Length != 7 && licenseNumber.Length != 8)
                        { // Check if license Number format legal.
                            Console.WriteLine("License Number format illegal");
                            break;
                        }
                        Console.WriteLine("Please enter the new bus start date in format dd/mm/yy.");
                        date = Convert.ToDateTime(Console.ReadLine());
                        Bus newBus = new Bus(licenseNumber, date);
                        busList.Add(newBus);
                        break;
                    case choices.select: // Select ride.
                        Console.WriteLine("Please enter wished bus license number.");
                        licenseNumber = Console.ReadLine();
                        foreach (Bus myBus in busList)
                        {
                            if (myBus.getLicenseNumber() == licenseNumber)
                            { // Check if the bus exists.
                                Random r = new Random(DateTime.Now.Millisecond);
                                double ride = r.Next(0, 1200);
                                int result = myBus.integrityCheck(ride);

                                if (result == 1) // There's not enough fuel for the ride.
                                    Console.WriteLine("There's not enough fuel for the ride. " +
                                        "Please refuel.");
                                if (result == 2) // The bus needs treatment.
                                    Console.WriteLine("It's been a year and the bus needs treatment. " +
                                        "Please send for treatment.");
                            }
                            else
                                Console.WriteLine("The viacle's not found. Try again.");

                        }
                        break;
                    case choices.operations: // Refuel or make a treatment.
                        Console.WriteLine("Please enter wished bus license number.");
                        licenseNumber = Console.ReadLine();
                        foreach (Bus myBus in busList)
                        {
                            if (myBus.getLicenseNumber() == licenseNumber)
                            { // Check if the bus exists.
                                Console.WriteLine("Please enter the wished operation. (1) for refuel. (2) for treatment.");
                                int op = int.Parse(Console.ReadLine());
                                if (op == 1)// Refuel.
                                    myBus.afterRefueling();
                                else if (op == 2) // Make a treatment.
                                    myBus.afterTreatment();
                            }
                            else
                                Console.WriteLine("The viacle's not found. Try again.");
                        }
                        break;
                    case choices.display: // Display bus license number and its 
                        // kilometer data for all buses.
                        foreach (Bus myBus in busList)
                        {
                            Console.WriteLine("Viacle:");
                            printlicenseNumberForm(myBus.getLicenseNumber());
                            Console.WriteLine("Kilometer:\n" + myBus.getKilometers());
                        }
                        break;
                    case choices.exit:
                        Console.WriteLine("Bye!");
                        break;
                    default:
                        break;

                }
            }
        }
        static void printlicenseNumberForm(string licenseNumber)
        {
            char[] licenseForm = licenseNumber.ToCharArray();
            if (licenseNumber.Length == 7) // New license number format.
            {
                Console.WriteLine("{0}{1}-{2}{3}{4}-{5}{6}",
                   licenseForm[0], licenseForm[1], licenseForm[2], licenseForm[3],
                   licenseForm[4], licenseForm[5], licenseForm[6]);
                return;
            }
            // Old license number format.
            Console.WriteLine("{0}{1}{2}-{3}{4}-{5}{6}{7}",
                    licenseForm[0], licenseForm[1], licenseForm[2], licenseForm[3],
                    licenseForm[4], licenseForm[5], licenseForm[6], licenseForm[7]);
        }
    }
}


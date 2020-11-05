using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_2070_5493
{
    class Bus
    {
        private string licenseNumber;
        private DateTime startingDate;
        private double kilometers;
        private double fuel;


        public Bus(string _licenseNumber, DateTime _startingDate,
            double _kilometers = 0, double _fuel = 0)
        { // A constructor.
            this.licenseNumber = _licenseNumber;
            this.startingDate = _startingDate;
            this.kilometers = _kilometers;
            this.fuel = _fuel;
        }
        public string getLicenseNumber()
        { // Getter.
            return licenseNumber;
        }


        public double getKilometers()
        { // Getter.
            return kilometers;
        }

        public int integrityCheck(double ride)
        {
            if (this.fuel - ride < 0)
                return 1; // There's not enough fuel for the ride.
            System.TimeSpan resultDate = DateTime.Now.Subtract(this.startingDate);
            if (resultDate.TotalDays >= 365 || (this.kilometers != 0 && this.kilometers % 20000 == 0))
                return 2; // It's been a year and the bus needs treatment.
            this.kilometers += ride; // Add ride to kilometer.
            this.fuel -= ride; // Substract fuel of ride.
            return 0;
        }
        public void afterTreatment()
        { // Updates the treatment date.
            this.startingDate = DateTime.Now;
        }
        public void afterRefueling()
        { // Refuel.
            this.fuel = 1200;
        }

    }
}

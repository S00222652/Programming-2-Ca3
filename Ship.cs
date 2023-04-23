using Ca3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ca3
{
    // The Ship class represents a ship carrying passengers.
    public class Ship
    {
        // Properties for the ship's information
        public string ManifestIdentificationNumber { get; set; }
        public string PassengerPortOfEmbarkationCode { get; set; }
        public DateTime PassengerArrivalDate { get; set; }

        // A list of passengers on the ship
        public List<Passenger> Passengers { get; set; }

        // Constructor for the Ship class
        public Ship(string manifestIdentificationNumber, string passengerPortOfEmbarkationCode, DateTime passengerArrivalDate)
        {
            // Set the properties for the ship
            ManifestIdentificationNumber = manifestIdentificationNumber;
            PassengerPortOfEmbarkationCode = passengerPortOfEmbarkationCode;
            PassengerArrivalDate = passengerArrivalDate;

            // Initialize the list of passengers
            Passengers = new List<Passenger>();
        }

        // Method to add a passenger to the ship's list of passengers
        public void AddPassenger(Passenger passenger)
        {
            Passengers.Add(passenger);
        }

        // Method to return the total number of passengers on the ship
        public int PassengerCount()
        {
            return Passengers.Count;
        }
    }
}

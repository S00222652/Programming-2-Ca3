using System; // Import namespaces for required libraries
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class Passenger // Define a Passenger class to hold passenger data
{
    // Define properties for passenger attributes
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public int Age { get; set; }
    public string SexCode { get; set; }
    public string OccupationCode { get; set; }
    public string NativeCountryCode { get; set; }
    public string Destination { get; set; }
    public string PassengerPortOfEmbarkationCode { get; set; }
    public string ManifestIdentificationNumber { get; set; }
    public DateTime PassengerArrivalDate { get; set; }
}

class Program // Define the main program class
{
    static void Main(string[] args) // Main method that is executed when the program starts
    {
        // Initialize string variable to store the path of the CSV file
        string filePath = @"../../../faminefile.csv";

        // Create a new instance of the CsvReader class to read data from the CSV file
        CsvReader reader = new CsvReader();

        List<Passenger> passengers; // Create an empty list to hold Passenger objects

        try // Use try-catch blocks to handle exceptions that may occur when reading the file
        {
            // Attempt to read the data from the CSV file and store it in the passengers list
            passengers = reader.ReadDataFromCSV(filePath);
        }
        catch (FileNotFoundException)
        {
            // Display an error message if the file is not found
            Console.WriteLine("Error: File not found. Please ensure the file path is correct.");
            return;
        }
        catch (IOException)
        {
            // Display an error message if there is an error reading the file
            Console.WriteLine("Error: Unable to read the file. Please ensure the file is not in use by another application.");
            return;
        }

        int choice; // Declare a variable to hold the user's menu choice

        // Use a do-while loop to repeatedly display the menu and execute the chosen option
        do
        {
            // Display the main menu options
            Console.WriteLine("\nMain Menu");
            Console.WriteLine("1. Ship Reports");
            Console.WriteLine("2. Occupation Report");
            Console.WriteLine("3. Age Report");
            Console.WriteLine("4. Exit");
            Console.Write("Enter Choice: ");

            // Read the user's choice from the console
            choice = int.Parse(Console.ReadLine());

            // Execute the corresponding method based on the user's choice
            switch (choice)
            {
                case 1:
                    ShowShipReports(passengers);
                    break;
                case 2:
                    ShowOccupationReport(passengers);
                    break;
                case 3:
                    DisplayAgeReport(passengers);
                    break;
                case 4:
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

        } while (choice != 4); // Repeat the loop until the user chooses to exit
    }

    // Method to display a list of ships and allow the user to select one to view more information
    static void ShowShipReports(List<Passenger> passengers)
    {
        // Use LINQ to group the passengers by ManifestIdentificationNumber (the ship they arrived on)
        var ships = passengers.GroupBy(p => p.ManifestIdentificationNumber).Select(g => new
        // Select a new anonymous object with the ship ID and a list of passengers on that ship
        {
            ShipId = g.Key,
            Passengers = g.ToList()
        });

        Console.WriteLine("\nList of Ships:");
        int index = 1;
        foreach (var ship in ships)
        {
            Console.WriteLine($"{index}. {ship.ShipId}"); // Display the ship ID for each ship
            index++;
        }

        Console.Write("\nEnter the number of the ship you want to view: ");
        int choice = int.Parse(Console.ReadLine()) - 1; // Read the user's choice and subtract 1 (to match the index)

        if (choice >= 0 && choice < ships.Count()) // Check if the user's choice is valid
        {
            var selectedShip = ships.ElementAt(choice); // Get the selected ship using LINQ
            Console.WriteLine($"\n{selectedShip.ShipId} : leaving from {selectedShip.Passengers.First().PassengerPortOfEmbarkationCode} Arrived : {selectedShip.Passengers.First().PassengerArrivalDate.ToString("dd/MM/yyyy")} with {selectedShip.Passengers.Count} passengers");

            // Display a formatted table of the passengers on the selected ship
            Console.WriteLine("\nPassenger List:");
            Console.WriteLine("┌─────────────────┬─────────────────┐");
            Console.WriteLine("│ First Name      │ Last Name       │");
            Console.WriteLine("├─────────────────┼─────────────────┤");
            foreach (var passenger in selectedShip.Passengers)
            {
                Console.WriteLine($"│ {passenger.FirstName,-15} │ {passenger.LastName,-15} │");
            }
            Console.WriteLine("└─────────────────┴─────────────────┘");
        }
        else
        {
            Console.WriteLine("Invalid ship number. Please try again.");
        }
    }

    // Method to display a report of passenger counts by occupation code
    static void ShowOccupationReport(List<Passenger> passengers)
    {
        // Use LINQ to group the passengers by OccupationCode
        var occupations = passengers.GroupBy(p => p.OccupationCode).Select(g => new
        {
            OccupationCode = g.Key,
            Count = g.Count()
        });

        // Display a formatted table of the occupation report
        Console.WriteLine("\nOccupation Report:");
        Console.WriteLine("┌──────────────────────────────────────────────────┬───────┐");
        Console.WriteLine("│ Occupation                                       │ Count │");
        Console.WriteLine("├──────────────────────────────────────────────────┼───────┤");
        foreach (var occupation in occupations)
        {
            Console.WriteLine($"│ {occupation.OccupationCode,-48} │ {occupation.Count,5} │");
        }
        Console.WriteLine("└──────────────────────────────────────────────────┴───────┘");
    }

    // Method to display a report of passenger counts by age group
    static void DisplayAgeReport(List<Passenger> passengers)
    {
        // Define an array of age category objects with a name, min age, and max age
        var ageCategories = new[]
        {
        new { Name = "Infants", Min = -1, Max = 1 },
        new { Name = "Children", Min = 02, Max = 12 },
        new { Name = "Teenagers", Min = 13, Max = 19 },
        new { Name = "Young Adults", Min = 20, Max = 29 },
        new { Name = "Adults", Min = 30, Max = 49 },
        new { Name = "Older Adults", Min = 50, Max = int.MaxValue },
        new { Name = "Unknown", Min = -2, Max = -1 }
        };
        // Use LINQ to count the number of passengers in each age category
        var ageCounts = ageCategories.Select(ac => new
        {
            ac.Name,
            Count = passengers.Count(p => p.Age >= ac.Min && p.Age <= ac.Max)
        }).ToList();

        // Display a formatted table of the age report
        Console.WriteLine("\nAge Report:");
        Console.WriteLine("┌───────────────┬──────────┐");
        Console.WriteLine("│ Age Group     │ Count    │");
        Console.WriteLine("├───────────────┼──────────┤");

        foreach (var ageCount in ageCounts)
        {
            Console.WriteLine($"│ {ageCount.Name,-13} │ {ageCount.Count,8} │");
        }
        Console.WriteLine("└───────────────┴──────────┘");
    }
}

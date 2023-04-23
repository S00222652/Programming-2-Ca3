using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

class CsvReader
{
    public List<Passenger> ReadDataFromCSV(string filePath)
    {
        List<Passenger> passengers = new List<Passenger>();

        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                reader.ReadLine(); // Skipping header

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    if (values.Length != 10)
                    {
                        Console.WriteLine("Invalid line format: " + line);
                        continue;
                    }

                    DateTime arrivalDate;
                    if (!DateTime.TryParseExact(values[9], "MM/dd/yyyy", CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.None, out arrivalDate))
                    {
                        Console.WriteLine("Invalid date format: " + line);
                        continue;
                    }

                    int age;
                    if (values[2] == "Unknown")
                    {
                        age = -1;
                    }
                    else
                    {
                        string ageString = "";
                        for (int i = 0; i < values[2].Length; i++)
                        {
                            if (Char.IsDigit(values[2][i]))
                            {
                                ageString += values[2][i];
                            }
                        }

                        if (!int.TryParse(ageString, out age))
                        {
                            Console.WriteLine("Invalid age format: " + line);
                            age = -1;
                        }
                    }

                    Passenger passenger = new Passenger
                    {
                        LastName = values[0],
                        FirstName = values[1],
                        Age = age,
                        SexCode = values[3],
                        OccupationCode = values[4],
                        NativeCountryCode = values[5],
                        Destination = values[6],
                        PassengerPortOfEmbarkationCode = values[7],
                        ManifestIdentificationNumber = values[8],
                        PassengerArrivalDate = arrivalDate
                    };

                    passengers.Add(passenger);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading file: " + ex.Message);
            Environment.Exit(1);
        }

        return passengers;
    }
}

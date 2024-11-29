using System;
using System.Globalization;

namespace HotelReservation
{
    class Program
    {
        static void Main(string[] args)
        {
            var processor = BookProcessor.Instance;

            if (args.Length < 4)
            {
                Console.WriteLine("Usage: myapp --hotels hotels.json --bookings bookings.json");
                return;
            }
            string hotelsFile = args[1];
            string bookingsFile = args[3];

            processor.LoadData(hotelsFile, bookingsFile);

            while (true)
            {
                Console.WriteLine("Enter availability queries (blank line to exit):");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    break;
                }

                try
                {
                    var parts = input.Split(new[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 4)
                    {
                        Console.WriteLine("Invalid command format. For exammple: Availability(H1, 20240901-20240904, SGL)");
                        continue;
                    }

                    string hotelId = parts[1].Trim();
                    string dateRange = parts[2].Trim();
                    string roomType = parts[3].Trim();

                    DateTime startDate, endDate;

                    if (dateRange.Contains('-'))
                    {
                        var dates = dateRange.Split('-');
                        startDate = DateTime.ParseExact(dates[0], "yyyyMMdd", CultureInfo.InvariantCulture);
                        endDate = DateTime.ParseExact(dates[1], "yyyyMMdd", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        startDate = DateTime.ParseExact(dateRange, "yyyyMMdd", CultureInfo.InvariantCulture);
                        endDate = startDate;
                    }

                    int availability = processor.GetAvailability(hotelId, startDate, endDate, roomType);
                    Console.WriteLine($"Availability: {availability}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}

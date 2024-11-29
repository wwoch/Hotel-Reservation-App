using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using HotelReservation;

namespace HotelReservation
{
    public class BookProcessor
    {
        // Singleton
        private static readonly Lazy<BookProcessor> instance = new Lazy<BookProcessor>(() => new BookProcessor());
        public static BookProcessor Instance => instance.Value;
        private BookProcessor() { }
        public List<Hotel> Hotels { get; private set; } = new List<Hotel>();
        public List<Booking> Bookings { get; private set; } = new List<Booking>();

        public void LoadData(string hotelsFile, string bookingsFile)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(hotelsFile) || !File.Exists(hotelsFile))
                    throw new FileNotFoundException($"File not found: {hotelsFile}");

                if (string.IsNullOrWhiteSpace(bookingsFile) || !File.Exists(bookingsFile))
                    throw new FileNotFoundException($"File not found: {bookingsFile}");

                string hotelsJson = File.ReadAllText(hotelsFile);
                Hotels = JsonConvert.DeserializeObject<List<Hotel>>(hotelsJson) ?? new List<Hotel>();

                string bookingsJson = File.ReadAllText(bookingsFile);
                Bookings = JsonConvert.DeserializeObject<List<Booking>>(bookingsJson) ?? new List<Booking>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                Environment.Exit(1);
            }
        }

        public int GetAvailability(string hotelId, DateTime startDate, DateTime endDate, string roomType)
        {
            if (endDate < startDate)
            {
                Console.WriteLine("The end date of the booking cannot be earlier than the start date");
                return 0;
            }

            var hotel = Hotels?.FirstOrDefault(h => h.Id == hotelId);
            if (hotel == null)
            {
                Console.WriteLine("Hotel not found");
                return 0;
            }

            var totalRooms = hotel.Rooms.Count(r => r.RoomType == roomType);
            if (totalRooms == 0)
            {
                Console.WriteLine($"No rooms of type {roomType} found in hotel {hotelId}.");
                return 0;
            }

            var overlappingBookings = new List<Booking>();

            foreach (var booking in Bookings)
            {
                if (booking.HotelId == hotelId && booking.RoomType == roomType)
                {
                    var arrivalDate = DateTime.ParseExact(booking.Arrival, "yyyyMMdd", null);
                    var departureDate = DateTime.ParseExact(booking.Departure, "yyyyMMdd", null);

                    if (!(endDate < arrivalDate || startDate > departureDate))
                    {
                        overlappingBookings.Add(booking);
                    }
                }
            }

            int bookedRooms = overlappingBookings.Count;
            int availableRooms = totalRooms - bookedRooms;

            return availableRooms;
        }

    }
}
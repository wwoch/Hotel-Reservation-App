# Hotel Reservation App

This is a simple C# application for checking hotel room availability based on bookings. The program loads data from JSON files containing information about hotels and bookings, and then allows the user to query the availability of rooms.

## Files

- **Models.cs**: Contains data models for `Hotel`, `RoomType`, `Room`, and `Booking`.
- **BookingProcessor.cs**: Processes hotel data, bookings, and checks room availability.
- **Program.cs**: Main entry point that handles user input and displays results.

## Setup

1. Clone the repository:

   ```bash
   git clone https://github.com/wwoch/Hotel-Reservation-App.git
   cd .\Hotel-Reservation-App\src\
   ```

2. Install dependencies:

   ```bash
   dotnet add package Newtonsoft.Json
   ```

3. Run the application:

   ```bash
   dotnet run --hotels data/hotels.json --bookings data/bookings.json
   ```

4. Usage:
   
   To check room availability, enter a query in the format:

   ```bash
   Availability(H1, 20240901-20240904, SGL)
   ```
   
   Where:
- **H1**: Hotel ID
- **0240901-20240904**: Date range (arrival - departure)
- **SGL**: Room type (single) or DBL (double)

  Example:

   ```bash
   Enter availability queries (blank line to exit):
   Availability(H1, 20240901-20240904, SGL)
   Availability: 5
   ```
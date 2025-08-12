using System;
using System.Threading.Tasks;
using FlightSystemUsingAPI.Data;
using FlightSystemUsingAPI.FileHandling;
using Microsoft.EntityFrameworkCore;

namespace FlightSystemUsingAPI.Service
{
    public class DataBackupService
    {
        private readonly FlightContext _context;
        private readonly FileSaveLoad _fileHandler;

        public DataBackupService(FlightContext context, FileSaveLoad fileHandler)
        {
            _context = context;
            _fileHandler = fileHandler;
        }

        // ===================== EXPORT =====================
        public async Task ExportDatabaseToFilesAsync()
        {
            Console.WriteLine("Exporting database to TXT files...");

            await _fileHandler.SaveAirportsAsync(await _context.Airports.AsNoTracking().ToListAsync());
            await _fileHandler.SaveAircraftsAsync(await _context.Aircrafts.AsNoTracking().ToListAsync());
            await _fileHandler.SaveCrewMembersAsync(await _context.CrewMembers.AsNoTracking().ToListAsync());
            await _fileHandler.SaveRoutesAsync(await _context.Routes.AsNoTracking().ToListAsync());
            await _fileHandler.SaveFlightsAsync(await _context.Flights.AsNoTracking().ToListAsync());
            await _fileHandler.SavePassengersAsync(await _context.Passengers.AsNoTracking().ToListAsync());
            await _fileHandler.SaveBookingsAsync(await _context.Bookings.AsNoTracking().ToListAsync());
            await _fileHandler.SaveTicketsAsync(await _context.Tickets.AsNoTracking().ToListAsync());
            await _fileHandler.SaveFlightCrewsAsync(await _context.FlightCrews.AsNoTracking().ToListAsync());
            await _fileHandler.SaveBaggageAsync(await _context.Baggages.AsNoTracking().ToListAsync());
            await _fileHandler.SaveMaintenancesAsync(await _context.AircraftMaintenances.AsNoTracking().ToListAsync());

            Console.WriteLine(" Export complete!");
        }

        // ===================== IMPORT =====================
        public async Task ImportFilesToDatabaseAsync()
        {
            Console.WriteLine("Importing TXT files into database...");

            _context.Airports.AddRange(await _fileHandler.LoadAirportsAsync());
            _context.Aircrafts.AddRange(await _fileHandler.LoadAircraftsAsync());
            _context.CrewMembers.AddRange(await _fileHandler.LoadCrewMembersAsync());
            _context.Routes.AddRange(await _fileHandler.LoadRoutesAsync());
            _context.Flights.AddRange(await _fileHandler.LoadFlightsAsync());
            _context.Passengers.AddRange(await _fileHandler.LoadPassengersAsync());
            _context.Bookings.AddRange(await _fileHandler.LoadBookingsAsync());
            _context.Tickets.AddRange(await _fileHandler.LoadTicketsAsync());
            _context.FlightCrews.AddRange(await _fileHandler.LoadFlightCrewsAsync());
            _context.Baggages.AddRange(await _fileHandler.LoadBaggageAsync());
            _context.AircraftMaintenances.AddRange(await _fileHandler.LoadMaintenancesAsync());

            await _context.SaveChangesAsync();

            Console.WriteLine(" Import complete!");
        }
    }
}

using FlightSystemUsingAPI;
using FlightSystemUsingAPI.Data;
using FlightSystemUsingAPI.FileHandling;
using FlightSystemUsingAPI.Repositories;
using FlightSystemUsingAPI.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// DbContext
services.AddDbContext<FlightContext>(opts =>
    opts.UseSqlServer(
        @"Server=localhost;Database=FlightDB;Trusted_Connection=True;TrustServerCertificate=True"));

// File I/O + backup
services.AddSingleton<FileSaveLoad>();
services.AddScoped<DataBackupService>();

// Repositories (register ALL on the same 'services' collection)
services.AddScoped<IAirportRepository, AirportRepository>();
services.AddScoped<IAircraftRepository, AircraftRepository>();
services.AddScoped<ICrewRepository, CrewRepository>();
services.AddScoped<IRouteRepository, RouteRepository>();
services.AddScoped<IFlightRepository, FlightRepository>();
services.AddScoped<IPassengerRepository, PassengerRepository>();
services.AddScoped<IBookingRepository, BookingRepository>();
services.AddScoped<ITicketRepository, TicketRepository>();
services.AddScoped<IBaggageRepository, BaggageRepository>();
services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();

// Services
services.AddScoped<IFlightService, FlightService>();

var sp = services.BuildServiceProvider();

using (var scope = sp.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<FlightContext>();

    // Ensure DB exists/migrated (optional but recommended)
    await ctx.Database.MigrateAsync();

    // Seed (pick the method that matches your SeedData)
     await SeedData.InitializeAsync(ctx);
    //SeedData.Initialize(ctx);

    // Backup/export (optional)
    var backup = scope.ServiceProvider.GetRequiredService<DataBackupService>();
    await backup.ExportDatabaseToFilesAsync();
    await backup.ImportFilesToDatabaseAsync();

    var flightService = scope.ServiceProvider.GetRequiredService<IFlightService>();

    Console.WriteLine("===== Flight Management Company =====\n");

    var today = DateTime.UtcNow.Date;

    var manifest = await flightService.GetDailyFlightManifestAsync(today);
    Console.WriteLine($"[1] Daily Flight Manifest: {manifest.Count} flights");
    foreach (var f in manifest.Take(3))
        Console.WriteLine($"   {f.FlightNumber} {f.Origin}->{f.Destination} Dep {f.DepUtc:HH:mm}");

    var topRoutes = await flightService.GetTopRoutesByRevenueAsync(today.AddDays(-30), today);
    Console.WriteLine($"[2] Top Routes by Revenue: {topRoutes.Count}");

    var conflicts = await flightService.GetCrewSchedulingConflictsAsync(today.AddDays(-7), today);
    Console.WriteLine($"[3] Crew Scheduling Conflicts: {conflicts.Count}");

    var availableSeats = await flightService.GetAvailableSeatsForFlightAsync(1);
    Console.WriteLine($"[4] Available Seats for Flight 1: {availableSeats.Count}");

    var frequentFliers = await flightService.GetFrequentFliersAsync(5);
    Console.WriteLine($"[5] Frequent Fliers: {frequentFliers.Count}");

    var baggageAlerts = await flightService.GetBaggageOverweightAlertsAsync(30m);
    Console.WriteLine($"[6] Baggage Overweight Alerts: {baggageAlerts.Count}");

    var otp = await flightService.GetOnTimePerformanceAsync(today.AddDays(-7), today, 15);
    Console.WriteLine($"[7] On-Time Performance: {otp:F2}%");

    var occupancy = await flightService.GetSeatOccupancyHeatmapAsync();
    Console.WriteLine($"[8] High Occupancy Flights: {occupancy.Count}");

    var connections = await flightService.GetPassengersWithConnectionsAsync(6);
    Console.WriteLine($"[9] Passengers with Connections: {connections.Count}");

    var maintenanceAlerts = await flightService.GetMaintenanceAlertsAsync(500, 180);
    Console.WriteLine($"[10] Maintenance Alerts: {maintenanceAlerts.Count}");

    var setOps = await flightService.GetComplexSetExamplesAsync();
    Console.WriteLine($"[11] Set Ops -> Union: {setOps.Union.Count}, Intersect: {setOps.Intersect.Count}, Except: {setOps.Except.Count}");

    var conversions = await flightService.GetConversionOperatorsDemoAsync();
    Console.WriteLine($"[12] Conversions -> Dict: {conversions.FlightsDict.Count}, TopRoutes: {conversions.TopRoutes.Length}");

    var runningRevenue = await flightService.GetRunningRevenueAsync();
    Console.WriteLine($"[13] Running Revenue Days: {runningRevenue.Count}");

    var forecast = await flightService.ForecastBookingsAsync();
    Console.WriteLine($"[14] Forecast Bookings (Next 7 days): {forecast}");
}

Console.WriteLine("\n===== End of Test =====");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
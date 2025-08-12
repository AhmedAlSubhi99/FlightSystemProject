using FlightSystemUsingAPI;
using FlightSystemUsingAPI.Data;
using FlightSystemUsingAPI.Repositories;
using FlightSystemUsingAPI.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

// 1. Setup DI container
var services = new ServiceCollection();

// --- Database connection ---
services.AddDbContext<FlightContext>(options =>
    options.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=FlightDB;Trusted_Connection=True;TrustServerCertificate=True"));

// --- Repository registrations ---
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

// --- Service layer ---
services.AddScoped<IFlightService, FlightService>();

// 2. Build service provider
var serviceProvider = services.BuildServiceProvider();

// 3. Seed database & run queries
using (var scope = serviceProvider.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FlightContext>();
    SeedData.Initialize(context);

    var flightService = scope.ServiceProvider.GetRequiredService<IFlightService>();

    Console.WriteLine("===== Flight Management Company: LINQ Queries Test =====\n");

    // 1. Daily Flight Manifest
    var manifest = await flightService.GetDailyFlightManifestAsync(DateTime.UtcNow.Date);
    Console.WriteLine($"[1] Daily Flight Manifest: {manifest.Count} flights");
    manifest.Take(3).ToList().ForEach(f => Console.WriteLine($"   {f.FlightNumber} -> {f.DepUtc}"));

    // 2. Top Routes by Revenue
    var topRoutes = await flightService.GetTopRoutesByRevenueAsync(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow);
    Console.WriteLine($"[2] Top Routes by Revenue: {topRoutes.Count} routes");
    topRoutes.Take(3).ToList().ForEach(r => Console.WriteLine($"   Route {r.RouteId} - Revenue: {r.Revenue:C}"));

    // 3. Crew Scheduling Conflicts
    var conflicts = await flightService.GetCrewSchedulingConflictsAsync(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
    Console.WriteLine($"[3] Crew Scheduling Conflicts: {conflicts.Count} conflicts");

    // 4. Available Seats
    var availableSeats = await flightService.GetAvailableSeatsForFlightAsync(1);
    Console.WriteLine($"[4] Available Seats for Flight 1: {availableSeats.Count} seats");

    // 5. Frequent Fliers
    var frequentFliers = await flightService.GetFrequentFliersAsync(5);
    Console.WriteLine($"[5] Frequent Fliers: {frequentFliers.Count} passengers");

    // 6. Baggage Overweight Alerts
    var baggageAlerts = await flightService.GetBaggageOverweightAlertsAsync(30m);
    Console.WriteLine($"[6] Baggage Overweight Alerts: {baggageAlerts.Count} alerts");

    // 7. On-Time Performance
    var otp = await flightService.GetOnTimePerformanceAsync(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow, 15);
    Console.WriteLine($"[7] On-Time Performance: {otp:F2}%");

    // 8. Seat Occupancy Heatmap
    var occupancy = await flightService.GetSeatOccupancyHeatmapAsync();
    Console.WriteLine($"[8] High Occupancy Flights: {occupancy.Count}");

    // 9. Passengers with Connections
    var connections = await flightService.GetPassengersWithConnectionsAsync(6);
    Console.WriteLine($"[9] Passengers with Connections: {connections.Count} passengers");

    // 10. Maintenance Alerts
    var maintenanceAlerts = await flightService.GetMaintenanceAlertsAsync(500, 180);
    Console.WriteLine($"[10] Maintenance Alerts: {maintenanceAlerts.Count} aircraft");

    // 11. Complex Set Examples
    var setOps = await flightService.GetComplexSetExamplesAsync();
    Console.WriteLine($"[11] Set Ops -> Union: {setOps.Union.Count}, Intersect: {setOps.Intersect.Count}, Except: {setOps.Except.Count}");

    // 12. Conversion Operators Demo
    var conversions = await flightService.GetConversionOperatorsDemoAsync();
    Console.WriteLine($"[12] Conversion Operators -> Dict: {conversions.FlightsDict.Count}, TopRoutes: {conversions.TopRoutes.Length}");

    // 13. Running Revenue
    var runningRevenue = await flightService.GetRunningRevenueAsync();
    Console.WriteLine($"[13] Running Revenue Days: {runningRevenue.Count}");

    // 14. Forecast Bookings
    var forecast = await flightService.ForecastBookingsAsync();
    Console.WriteLine($"[14] Forecast Bookings (Next 7 days): {forecast}");
}

Console.WriteLine("\n===== End of Test =====");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

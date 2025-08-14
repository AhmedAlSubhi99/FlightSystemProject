using System;
using System.Linq;
using FlightSystemUsingAPI.DTOs;
using FlightSystemUsingAPI.DTOs.Report;
using FlightSystemUsingAPI.Service;

namespace FlightSystemUsingAPI.Menu
{
    public static class AppMenu
    {
        public static void Run(IFlightService svc)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Flight Management Console ===");
                Console.WriteLine("1) Daily Flight Manifest");
                Console.WriteLine("2) Top Routes by Revenue");
                Console.WriteLine("3) Seat Occupancy (>= rate)");
                Console.WriteLine("4) Available Seats for Flight");
                Console.WriteLine("5) Crew Scheduling Conflicts");
                Console.WriteLine("6) Frequent Fliers");
                Console.WriteLine("7) Baggage Overweight Alerts");
                Console.WriteLine("8) Maintenance Alerts");
                Console.WriteLine("9) Running Revenue (daily)");
                Console.WriteLine("10) 7-Day Booking Forecast");
                Console.WriteLine("11) On-Time Performance");
                Console.WriteLine("12) Passengers with Connections");
                Console.WriteLine("13) Single Passenger Itinerary");
                Console.WriteLine("0) Exit");
                Console.Write("Select: ");
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": Manifest(svc); break;
                        case "2": TopRoutes(svc); break;
                        case "3": Occupancy(svc); break;
                        case "4": AvailableSeats(svc); break;
                        case "5": CrewConflicts(svc); break;
                        case "6": FrequentFliers(svc); break;
                        case "7": BaggageAlerts(svc); break;
                        case "8": MaintenanceAlerts(svc); break;
                        case "9": RunningRevenue(svc); break;
                        case "10": Forecast(svc); break;
                        case "11": OnTime(svc); break;
                        case "12": Connections(svc); break;
                        case "13": Itinerary(svc); break;
                        case "0": return;
                        default: continue;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error: " + ex.Message);
                    Pause();
                }
            }
        }

        // ---------- Handlers ----------
        private static void Manifest(IFlightService svc)
        {
            var date = ReadDate("Date (UTC)", DateTime.UtcNow.Date);
            var data = svc.GetDailyFlightManifest(date);
            Console.WriteLine($"\nManifest for {date:yyyy-MM-dd} — {data.Count} flights");
            foreach (var m in data.OrderBy(x => x.DepUtc))
                Console.WriteLine($"{m.FlightNumber,-6} {m.Origin}->{m.Destination} dep {m.DepUtc:HH:mm} arr {m.ArrUtc:HH:mm} pax {m.PassengerCount,3} bags {m.TotalBaggageKg,5:0.0} kg tail {m.AircraftTail}");
            Pause();
        }

        private static void TopRoutes(IFlightService svc)
        {
            var (from, to) = ReadRange();
            var take = ReadInt("Top N", 10);
            var data = svc.GetTopRoutesByRevenue(from, to, take);
            Console.WriteLine($"\nTop Routes by Revenue ({from:yyyy-MM-dd}..{to:yyyy-MM-dd})");
            Console.WriteLine("Origin Dest  Revenue    Seats  AvgFare");
            foreach (var r in data)
                Console.WriteLine($"{r.Origin,-6} {r.Destination,-6} {r.Revenue,9:0.00} {r.SeatsSold,6} {r.AvgFare,8:0.00}");
            Pause();
        }

        private static void Occupancy(IFlightService svc)
        {
            var (from, to) = ReadRange();
            var minRate = ReadDouble("Min occupancy rate (0..1)", 0.8);
            var topN = ReadInt("Top N (0=all)", 0);
            var data = svc.GetSeatOccupancy(from, to, minRate, topN);
            Console.WriteLine($"\nSeat Occupancy >= {minRate:0.00} ({from:yyyy-MM-dd}..{to:yyyy-MM-dd})");
            Console.WriteLine("Flight  Sold/Cap  Rate");
            foreach (var x in data)
                Console.WriteLine($"{x.FlightNumber,-6} {x.Sold,3}/{x.Capacity,-3} {x.OccupancyRate:0.000}");
            Pause();
        }

        private static void AvailableSeats(IFlightService svc)
        {
            var id = ReadInt("FlightId", 1);
            var data = svc.GetAvailableSeatsForFlight(id);
            Console.WriteLine($"\nAvailable seats for FlightId={id} ({data.Seats.Count})");
            Console.WriteLine(string.Join(", ", data.Seats.Take(200)));
            if (data.Seats.Count > 200) Console.WriteLine($"... +{data.Seats.Count - 200} more");
            Pause();
        }

        private static void CrewConflicts(IFlightService svc)
        {
            var (from, to) = ReadRange();
            var data = svc.GetCrewSchedulingConflicts(from, to);
            Console.WriteLine($"\nCrew Conflicts ({from:yyyy-MM-dd}..{to:yyyy-MM-dd}) — {data.Count}");
            Console.WriteLine("Crew                 FlightA    FlightB       A.Dep           B.Dep");
            foreach (var c in data)
                Console.WriteLine($"{c.CrewName,-20} {c.FlightAId,7}    {c.FlightBId,7}    {c.FlightADep:MM-dd HH:mm}   {c.FlightBDep:MM-dd HH:mm}");
            Pause();
        }

        private static void FrequentFliers(IFlightService svc)
        {
            var topN = ReadInt("Top N", 20);
            var data = svc.GetFrequentFliers(topN);
            Console.WriteLine($"\nFrequent Fliers (Top {topN})");
            Console.WriteLine("Passenger                     Flights  Distance(km)");
            foreach (var f in data)
                Console.WriteLine($"{f.PassengerName,-28} {f.FlightsCount,7}  {f.TotalDistanceKm,10}");
            Pause();
        }

        private static void BaggageAlerts(IFlightService svc)
        {
            var thr = ReadDecimal("Per-passenger threshold kg", 30m);
            var data = svc.GetBaggageOverweightAlerts(thr);
            Console.WriteLine($"\nBaggage Overweight Alerts (> {thr:0.0} kg)");
            Console.WriteLine("TicketId  BookingRef  TotalKg");
            foreach (var b in data)
                Console.WriteLine($"{b.TicketId,7}   {b.BookingRef,-10}  {b.TotalWeightKg,7:0.0}");
            Pause();
        }

        private static void MaintenanceAlerts(IFlightService svc)
        {
            var stale = ReadDate("Stale before (UTC)", DateTime.UtcNow.Date.AddDays(-60));
            var minFlights = ReadInt("Min flights", 20);
            var data = svc.GetMaintenanceAlerts(stale, minFlights);
            Console.WriteLine($"\nMaintenance Alerts (last <= {stale:yyyy-MM-dd} OR flights >= {minFlights})");
            Console.WriteLine("Tail      LastMaint      Flights");
            foreach (var m in data)
                Console.WriteLine($"{m.TailNumber,-8} {m.LastMaintenance:yyyy-MM-dd}  {m.FlightsCount,7}");
            Pause();
        }

        private static void RunningRevenue(IFlightService svc)
        {
            var (from, to) = ReadRange();
            var data = svc.GetRunningRevenue(from, to);
            Console.WriteLine($"\nRunning Revenue ({from:yyyy-MM-dd}..{to:yyyy-MM-dd})");
            Console.WriteLine("Day         Revenue   Running");
            foreach (var d in data)
                Console.WriteLine($"{d.Day:yyyy-MM-dd}  {d.Revenue,8:0.00}  {d.RunningRevenue,8:0.00}");
            Pause();
        }

        private static void Forecast(IFlightService svc)
        {
            var data = svc.ForecastNextWeekBookings();
            Console.WriteLine("\n7-Day Booking Forecast");
            Console.WriteLine("Day         Forecast");
            foreach (var d in data)
                Console.WriteLine($"{d.Day:yyyy-MM-dd}  {d.ForecastBookings,8}");
            Pause();
        }

        private static void OnTime(IFlightService svc)
        {
            var (from, to) = ReadRange();
            var data = svc.GetOnTimePerformance(from, to);
            Console.WriteLine($"\nOn-Time Performance ({from:yyyy-MM-dd}..{to:yyyy-MM-dd})");
            Console.WriteLine("Day         Scheduled  Landed  Delayed  OnTimeRate");
            foreach (var d in data)
                Console.WriteLine($"{d.Day:yyyy-MM-dd}  {d.Scheduled,9}  {d.Landed,6}  {d.Delayed,7}  {d.OnTimeRate:0.000}");
            Pause();
        }

        private static void Connections(IFlightService svc)
        {
            var (from, to) = ReadRange();
            var data = svc.GetPassengersWithConnections(from, to);
            Console.WriteLine($"\nPassengers with Connections ({from:yyyy-MM-dd}..{to:yyyy-MM-dd}) — {data.Count}");
            foreach (var p in data.Take(30))
            {
                Console.WriteLine($"{p.PassengerName} ({p.TotalSegments} segs): {p.Origin} -> {p.Destination}  total {p.TotalDuration}");
                foreach (var s in p.Segments)
                    Console.WriteLine($"   {s.Sequence}. {s.FlightNumber} {s.Origin}->{s.Destination} {s.DepUtc:MM-dd HH:mm} → {s.ArrUtc:HH:mm}");
            }
            if (data.Count > 30) Console.WriteLine($"... +{data.Count - 30} more");
            Pause();
        }

        private static void Itinerary(IFlightService svc)
        {
            var pid = ReadInt("PassengerId", 1);
            var (from, to) = ReadRange();
            var p = svc.GetPassengerItinerary(pid, from, to);
            if (p == null) { Console.WriteLine("No itinerary in range."); Pause(); return; }

            Console.WriteLine($"\nItinerary for {p.PassengerName} ({p.PassengerId}) — {p.TotalSegments} segments");
            foreach (var s in p.Segments)
                Console.WriteLine($"{s.Sequence}. {s.FlightNumber} {s.Origin}->{s.Destination} {s.DepUtc:yyyy-MM-dd HH:mm} → {s.ArrUtc:HH:mm}");
            Console.WriteLine($"Origin: {p.Origin}  Destination: {p.Destination}  Connections: {p.Connections}  Total: {p.TotalDuration}");
            Pause();
        }

        // ---------- Helpers ----------
        private static (DateTime from, DateTime to) ReadRange()
        {
            var from = ReadDate("From (UTC)", DateTime.UtcNow.Date.AddDays(-14));
            var to = ReadDate("To   (UTC)", DateTime.UtcNow.Date.AddDays(14));
            return (from, to);
        }

        private static DateTime ReadDate(string label, DateTime def)
        {
            Console.Write($"{label} [default {def:yyyy-MM-dd}]: ");
            var s = Console.ReadLine();
            return DateTime.TryParse(s, out var d) ? d.Date : def;
        }

        private static int ReadInt(string label, int def)
        {
            Console.Write($"{label} [default {def}]: ");
            var s = Console.ReadLine();
            return int.TryParse(s, out var v) ? v : def;
        }

        private static decimal ReadDecimal(string label, decimal def)
        {
            Console.Write($"{label} [default {def}]: ");
            var s = Console.ReadLine();
            return decimal.TryParse(s, out var v) ? v : def;
        }

        private static double ReadDouble(string label, double def)
        {
            Console.Write($"{label} [default {def}]: ");
            var s = Console.ReadLine();
            return double.TryParse(s, out var v) ? v : def;
        }

        private static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }
}

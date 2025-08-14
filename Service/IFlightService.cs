using System;
using System.Collections.Generic;
using FlightSystemUsingAPI.DTOs;
using FlightSystemUsingAPI.DTOs.Report;

namespace FlightSystemUsingAPI.Service
{
    public interface IFlightService
    {
        // Core reports
        List<FlightManifestDto> GetDailyFlightManifest(DateTime dateUtc);
        List<RouteRevenueDto> GetTopRoutesByRevenue(DateTime fromUtc, DateTime toUtc, int take = 10);
        List<FlightOccupancyDto> GetSeatOccupancy(DateTime fromUtc, DateTime toUtc, double minRate = 0.8, int topN = 0);
        AvailableSeatDto GetAvailableSeatsForFlight(int flightId);

        // Crew & passengers
        List<CrewConflictDto> GetCrewSchedulingConflicts(DateTime fromUtc, DateTime toUtc);
        List<FrequentFlierDto> GetFrequentFliers(int topN = 20);
        List<PassengerItineraryDto> GetPassengersWithConnections(DateTime fromUtc, DateTime toUtc);
        PassengerItineraryDto? GetPassengerItinerary(int passengerId, DateTime fromUtc, DateTime toUtc);

        // Ops / safety / revenue
        List<BaggageAlertDto> GetBaggageOverweightAlerts(decimal perPassengerThresholdKg = 30m);
        List<MaintenanceAlertDto> GetMaintenanceAlerts(DateTime staleBeforeDate, int minFlights = 20);
        List<DailyRevenuePoint> GetRunningRevenue(DateTime fromUtc, DateTime toUtc);
        List<BookingForecastDto> ForecastNextWeekBookings();
        List<OnTimePerformanceDto> GetOnTimePerformance(DateTime fromUtc, DateTime toUtc);
    }
}

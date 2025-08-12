using FlightSystemUsingAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightSystemUsingAPI.Service
{
    public interface IFlightService
    {
        Task<List<FlightManifestDto>> GetDailyFlightManifestAsync(DateTime date);
        Task<List<RouteRevenueDto>> GetTopRoutesByRevenueAsync(DateTime from, DateTime to);
        Task<List<CrewConflictDto>> GetCrewSchedulingConflictsAsync(DateTime from, DateTime to);
        Task<List<AvailableSeatDto>> GetAvailableSeatsForFlightAsync(int flightId);
        Task<List<FrequentFlierDto>> GetFrequentFliersAsync(int topN);
        Task<List<BaggageAlertDto>> GetBaggageOverweightAlertsAsync(decimal threshold);

        Task<decimal> GetOnTimePerformanceAsync(DateTime from, DateTime to, int minutesThreshold);
        Task<List<(int FlightId, double OccupancyRate)>> GetSeatOccupancyHeatmapAsync();
        Task<List<PassengerItineraryDto>> GetPassengersWithConnectionsAsync(int maxHoursBetweenFlights);
        Task<List<(int AircraftId, string TailNumber)>> GetMaintenanceAlertsAsync(double flightHoursThreshold, int daysSinceLastMaintenance);
        Task<(List<int> Union, List<int> Intersect, List<int> Except)> GetComplexSetExamplesAsync();
        Task<(Dictionary<string, int> FlightsDict, string[] TopRoutes, IEnumerable<string> EnumerableExample)> GetConversionOperatorsDemoAsync();
        Task<List<(DateTime Date, decimal RunningRevenue)>> GetRunningRevenueAsync();
        Task<int> ForecastBookingsAsync();
    }
}

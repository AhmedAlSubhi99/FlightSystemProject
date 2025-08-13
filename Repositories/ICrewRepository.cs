using FlightSystemUsingAPI.MODLES;
using System;
using System.Collections.Generic;

namespace FlightSystemUsingAPI.Repositories
{
    public interface ICrewRepository
    {
        IEnumerable<CrewMember> GetAll();
        CrewMember? GetById(int id);
        void Add(CrewMember entity);
        void Update(CrewMember entity);
        void Delete(int id);

        // Helpers
        IEnumerable<CrewMember> GetByRole(CrewRole role);
        IEnumerable<FlightCrew> GetAssignmentsByFlight(int flightId);
        IEnumerable<CrewMember> GetAvailable(DateTime from, DateTime to); // simple overlap check
    }
}

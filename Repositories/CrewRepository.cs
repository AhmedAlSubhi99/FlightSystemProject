using FlightSystemUsingAPI.Data;
using FlightSystemUsingAPI.MODLES;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightSystemUsingAPI.Repositories
{
    public class CrewRepository : ICrewRepository
    {
        private readonly FlightContext _ctx;
        public CrewRepository(FlightContext ctx) { _ctx = ctx; }

        public IEnumerable<CrewMember> GetAll() => _ctx.CrewMembers.ToList();
        public CrewMember? GetById(int id) => _ctx.CrewMembers.Find(id);
        public void Add(CrewMember e) { _ctx.CrewMembers.Add(e); _ctx.SaveChanges(); }
        public void Update(CrewMember e) { _ctx.CrewMembers.Update(e); _ctx.SaveChanges(); }
        public void Delete(int id) { var e = GetById(id); if (e != null) { _ctx.CrewMembers.Remove(e); _ctx.SaveChanges(); } }

        public IEnumerable<CrewMember> GetByRole(CrewRole role) => _ctx.CrewMembers.Where(c => c.Role == role).ToList();

        public IEnumerable<FlightCrew> GetAssignmentsByFlight(int flightId) =>
            _ctx.FlightCrews.Include(fc => fc.CrewMember).Where(fc => fc.FlightId == flightId).ToList();

        public IEnumerable<CrewMember> GetAvailable(DateTime from, DateTime to)
        {
            var busyIds = _ctx.Flights
                .Where(f => f.DepartureUtc < to && f.ArrivalUtc > from)
                .SelectMany(f => f.FlightCrews)
                .Select(fc => fc.CrewId)
                .Distinct()
                .ToList();

            return _ctx.CrewMembers.Where(c => !busyIds.Contains(c.CrewId)).ToList();
        }
    }
}

using FlightSystemUsingAPI.Data;
using FlightSystemUsingAPI.MODLES;
using System.Collections.Generic;
using System.Linq;

namespace FlightSystemUsingAPI.Repositories
{
    public class BaggageRepository : IBaggageRepository
    {
        private readonly FlightContext _ctx;
        public BaggageRepository(FlightContext ctx) { _ctx = ctx; }

        public IEnumerable<Baggage> GetAll() => _ctx.Baggage.ToList();
        public Baggage? GetById(int id) => _ctx.Baggage.Find(id);
        public void Add(Baggage e) { _ctx.Baggage.Add(e); _ctx.SaveChanges(); }
        public void Update(Baggage e) { _ctx.Baggage.Update(e); _ctx.SaveChanges(); }
        public void Delete(int id) { var e = GetById(id); if (e != null) { _ctx.Baggage.Remove(e); _ctx.SaveChanges(); } }

        public IEnumerable<Baggage> GetByTicket(int ticketId) => _ctx.Baggage.Where(b => b.TicketId == ticketId).ToList();
        public IEnumerable<Baggage> GetOverweight(decimal thresholdKg) => _ctx.Baggage.Where(b => b.WeightKg > thresholdKg).ToList();
    }
}

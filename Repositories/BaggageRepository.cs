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

        public IEnumerable<Baggage> GetAll() => _ctx.Baggages.ToList();
        public Baggage? GetById(int id) => _ctx.Baggages.Find(id);
        public void Add(Baggage e) { _ctx.Baggages.Add(e); _ctx.SaveChanges(); }
        public void Update(Baggage e) { _ctx.Baggages.Update(e); _ctx.SaveChanges(); }
        public void Delete(int id) { var e = GetById(id); if (e != null) { _ctx.Baggages.Remove(e); _ctx.SaveChanges(); } }

        public IEnumerable<Baggage> GetByTicket(int ticketId) => _ctx.Baggages.Where(b => b.TicketId == ticketId).ToList();
        public IEnumerable<Baggage> GetOverweight(decimal thresholdKg) => _ctx.Baggages.Where(b => b.WeightKg > thresholdKg).ToList();
    }
}

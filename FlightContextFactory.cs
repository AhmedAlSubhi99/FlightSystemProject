using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using FlightSystemUsingAPI.Data;

namespace FlightSystemUsingAPI
{
    public class FlightContextFactory : IDesignTimeDbContextFactory<FlightContext>
    {
        public FlightContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FlightContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=FlightDBase;Trusted_Connection=True;TrustServerCertificate=True");
            return new FlightContext(optionsBuilder.Options);
        }
    }
}
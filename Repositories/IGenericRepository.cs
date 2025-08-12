using System.Collections.Generic;
using FlightSystemUsingAPI.MODLES;
using FlightSystemUsingAPI.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace FlightSystemUsingAPI.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id); // Nullable return
        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}

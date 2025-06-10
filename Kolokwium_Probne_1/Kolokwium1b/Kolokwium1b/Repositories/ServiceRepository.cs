using Microsoft.EntityFrameworkCore;
using Kolokwium1b.Data;
using Kolokwium1b.Models;
using Kolokwium1b.Repositories;

namespace WorkshopAPI.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly WorkshopDbContext _context;

        public ServiceRepository(WorkshopDbContext context)
        {
            _context = context;
        }

        public async Task<Service?> GetServiceByNameAsync(string serviceName)
        {
            return await _context.Services
                .FirstOrDefaultAsync(s => s.Name == serviceName);
        }
    }
}
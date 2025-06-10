using Kolokwium1b.Data;
using Kolokwium1b.Models;
using Microsoft.EntityFrameworkCore;
using Kolokwium1b.Data;
using Kolokwium1b.Models;

namespace Kolokwium1b.Repositories
{
    public class MechanicRepository : IMechanicRepository
    {
        private readonly WorkshopDbContext _context;

        public MechanicRepository(WorkshopDbContext context)
        {
            _context = context;
        }

        public async Task<Mechanic?> GetMechanicByLicenceNumberAsync(string licenceNumber)
        {
            return await _context.Mechanics
                .FirstOrDefaultAsync(m => m.LicenceNumber == licenceNumber);
        }
    }
}
using Kolokwium1b.Data;
using Kolokwium1b.Models;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium1b.Repositories;


public class VisitRepository : IVisitRepository
    {
        private readonly WorkshopDbContext _context;

        public VisitRepository(WorkshopDbContext context)
        {
            _context = context;
        }

        public async Task<Visit?> GetVisitByIdAsync(int visitId)
        {
            return await _context.Visits
                .Include(v => v.Client)
                .Include(v => v.Mechanic)
                .Include(v => v.VisitServices)
                .ThenInclude(vs => vs.Service)
                .FirstOrDefaultAsync(v => v.VisitId == visitId);
        }

        public async Task<bool> VisitExistsAsync(int visitId)
        {
            return await _context.Visits.AnyAsync(v => v.VisitId == visitId);
        }

        public async Task<Visit> CreateVisitAsync(Visit visit)
        {
            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();
            return visit;
        }
}
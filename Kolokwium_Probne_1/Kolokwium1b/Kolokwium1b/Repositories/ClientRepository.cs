using Kolokwium1b.Data;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium1b.Repositories;

public class ClientRepository: IClientRepository
{
    private readonly WorkshopDbContext _context;

    public ClientRepository(WorkshopDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ClientExistsAsync(int clientId)
    {
        return await _context.Clients.AnyAsync(c => c.ClientId == clientId);
    }
}
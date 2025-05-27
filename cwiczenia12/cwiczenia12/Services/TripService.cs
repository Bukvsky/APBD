using Microsoft.EntityFrameworkCore;
using cwiczenia12.Models;
using cwiczenia12.DTOs; 

namespace cwiczenia12.Services 
{
    public class TripService : ITripService
    {
        private readonly Apbd12Context _context; 

        public TripService(Apbd12Context context) 
        {
            _context = context;
        }

        public async Task<TripListResponseDto> GetTripsAsync(int page = 1, int pageSize = 10)
        {
            var totalTrips = await _context.Trips.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalTrips / pageSize);

            var trips = await _context.Trips
                .Include(t => t.IdCountries) 
                .Include(t => t.ClientTrips)
                    .ThenInclude(ct => ct.IdClientNavigation)
                .OrderByDescending(t => t.DateFrom)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TripDto
                {
                    Name = t.Name,
                    Description = t.Description,
                    DateFrom = t.DateFrom,
                    DateTo = t.DateTo,
                    MaxPeople = t.MaxPeople,
                    Countries = t.IdCountries.Select(c => new CountryDto 
                    {
                        Name = c.Name
                    }).ToList(),
                    Clients = t.ClientTrips.Select(ct => new ClientDto
                    {
                        FirstName = ct.IdClientNavigation.FirstName,
                        LastName = ct.IdClientNavigation.LastName
                    }).ToList()
                })
                .ToListAsync();

            return new TripListResponseDto
            {
                PageNum = page,
                PageSize = pageSize,
                AllPages = totalPages,
                Trips = trips
            };
        }

        public async Task<bool> DeleteClientAsync(int idClient)
        {
            var client = await _context.Clients
                .Include(c => c.ClientTrips)
                .FirstOrDefaultAsync(c => c.IdClient == idClient);

            if (client == null)
                return false;

            if (client.ClientTrips.Any())
                throw new InvalidOperationException("Nie można usunąć klienta, który ma przypisane wycieczki.");

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddClientToTripAsync(AddClientToTripDto dto)
        {
            var existingClient = await _context.Clients
                .FirstOrDefaultAsync(c => c.Pesel == dto.Pesel);

            if (existingClient != null)
                throw new InvalidOperationException("Klient o podanym numerze PESEL już istnieje.");

            var trip = await _context.Trips
                .FirstOrDefaultAsync(t => t.IdTrip == dto.IdTrip);

            if (trip == null)
                throw new InvalidOperationException("Wycieczka o podanym ID nie istnieje.");

            if (trip.DateFrom <= DateTime.Now)
                throw new InvalidOperationException("Nie można zapisać się na wycieczkę, która już się odbyła.");

            var newClient = new Client
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Telephone = dto.Telephone,
                Pesel = dto.Pesel
            };

            _context.Clients.Add(newClient);
            await _context.SaveChangesAsync();


            var clientTrip = new ClientTrip
            {
                IdClient = newClient.IdClient,
                IdTrip = dto.IdTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = dto.PaymentDate
            };

            _context.ClientTrips.Add(clientTrip);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
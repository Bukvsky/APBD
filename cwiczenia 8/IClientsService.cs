using Tutorial8.Models.DTOs;

namespace Tutorial8.Services
{
    public interface IClientsService
    {
        
        Task<ClientTripDTO?> GetClientTrips(int clientId);

        Task<int> CreateClient(CreateClientDTO client);


        Task<(bool Success, string? ErrorMessage)> RegisterClientForTrip(int clientId, int tripId);

        Task<(bool Success, string? ErrorMessage)> UnregisterClientFromTrip(int clientId, int tripId);


        Task<bool> ClientExists(int clientId);
    }
}

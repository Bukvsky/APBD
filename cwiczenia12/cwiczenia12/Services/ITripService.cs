using cwiczenia12.DTOs; 

namespace cwiczenia12.Services 
{
    public interface ITripService
    {
        Task<TripListResponseDto> GetTripsAsync(int page = 1, int pageSize = 10);
        Task<bool> DeleteClientAsync(int idClient);
        Task<bool> AddClientToTripAsync(AddClientToTripDto dto);
    }
}
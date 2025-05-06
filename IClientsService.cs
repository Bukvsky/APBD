using Tutorial8.Models.DTOs;

namespace Tutorial8.Services
{
    public interface IClientsService
    {
        /// <summary>
        /// Pobiera wszystkie wycieczki powiązane z danym klientem
        /// </summary>
        /// <param name="clientId">ID klienta</param>
        /// <returns>Obiekt z danymi klienta i jego wycieczkami lub null jeśli klient nie istnieje</returns>
        Task<ClientTripDTO?> GetClientTrips(int clientId);

        /// <summary>
        /// Tworzy nowego klienta w bazie danych
        /// </summary>
        /// <param name="client">Dane nowego klienta</param>
        /// <returns>ID utworzonego klienta</returns>
        Task<int> CreateClient(CreateClientDTO client);

        /// <summary>
        /// Rejestruje klienta na wycieczkę
        /// </summary>
        /// <param name="clientId">ID klienta</param>
        /// <param name="tripId">ID wycieczki</param>
        /// <returns>True jeśli rejestracja się powiodła, false jeśli nie</returns>
        Task<(bool Success, string? ErrorMessage)> RegisterClientForTrip(int clientId, int tripId);

        /// <summary>
        /// Usuwa rejestrację klienta z wycieczki
        /// </summary>
        /// <param name="clientId">ID klienta</param>
        /// <param name="tripId">ID wycieczki</param>
        /// <returns>True jeśli usunięcie się powiodło, false jeśli nie</returns>
        Task<(bool Success, string? ErrorMessage)> UnregisterClientFromTrip(int clientId, int tripId);

        /// <summary>
        /// Sprawdza czy klient istnieje w bazie danych
        /// </summary>
        /// <param name="clientId">ID klienta</param>
        /// <returns>True jeśli klient istnieje, false jeśli nie</returns>
        Task<bool> ClientExists(int clientId);
    }
}
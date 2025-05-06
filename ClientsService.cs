using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services
{
    public class ClientsService : IClientsService
    {
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=APBD;Integrated Security=True;";

        public async Task<ClientTripDTO?> GetClientTrips(int clientId)
        {
            ClientTripDTO? result = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                // Sprawdzenie czy klient istnieje
                if (!await CheckClientExists(conn, clientId))
                    return null;

                // Pobranie danych klienta
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT IdClient, FirstName, LastName 
                    FROM Client 
                    WHERE IdClient = @ClientId", conn))
                {
                    cmd.Parameters.AddWithValue("@ClientId", clientId);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new ClientTripDTO
                            {
                                IdClient = reader.GetInt32(reader.GetOrdinal("IdClient")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Trips = new List<ClientTripDetailsDTO>()
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }

                // Pobranie wycieczek klienta
                string tripsQuery = @"
                    SELECT 
                        t.IdTrip, t.Name, t.Description, t.DateFrom, t.DateTo, t.MaxPeople, 
                        ct.RegisteredAt, ct.PaymentDate, ct.PaymentAmount,
                        c.IdCountry, c.Name as CountryName
                    FROM Client_Trip ct
                    JOIN Trip t ON ct.IdTrip = t.IdTrip
                    LEFT JOIN Country_Trip ctr ON t.IdTrip = ctr.IdTrip
                    LEFT JOIN Country c ON ctr.IdCountry = c.IdCountry
                    WHERE ct.IdClient = @ClientId
                    ORDER BY t.DateFrom";

                using (SqlCommand cmd = new SqlCommand(tripsQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ClientId", clientId);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        int lastTripId = -1;
                        ClientTripDetailsDTO? currentTrip = null;

                        while (await reader.ReadAsync())
                        {
                            int tripId = reader.GetInt32(reader.GetOrdinal("IdTrip"));

                            // Jeśli to nowa wycieczka, dodaj ją do listy
                            if (tripId != lastTripId)
                            {
                                currentTrip = new ClientTripDetailsDTO
                                {
                                    IdTrip = tripId,
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    DateFrom = reader.GetDateTime(reader.GetOrdinal("DateFrom")),
                                    DateTo = reader.GetDateTime(reader.GetOrdinal("DateTo")),
                                    Price = 0, // Zakładam, że cena może być pobierana z innego miejsca lub obliczana
                                    RegisteredAt = reader.GetDateTime(reader.GetOrdinal("RegisteredAt")),
                                    Countries = new List<CountryDTO>()
                                };

                                // Obsługa null-owych wartości dla płatności
                                if (!reader.IsDBNull(reader.GetOrdinal("PaymentAmount")))
                                    currentTrip.PaymentAmount = reader.GetDecimal(reader.GetOrdinal("PaymentAmount"));

                                if (!reader.IsDBNull(reader.GetOrdinal("PaymentDate")))
                                    currentTrip.PaymentDate = reader.GetDateTime(reader.GetOrdinal("PaymentDate"));

                                result.Trips.Add(currentTrip);
                                lastTripId = tripId;
                            }

                            // Dodaj kraj do wycieczki jeśli istnieje
                            if (!reader.IsDBNull(reader.GetOrdinal("IdCountry")))
                            {
                                currentTrip.Countries.Add(new CountryDTO
                                {
                                    Name = reader.GetString(reader.GetOrdinal("CountryName"))
                                });
                            }
                        }
                    }
                }
            }

            return result;
        }

        public async Task<int> CreateClient(CreateClientDTO client)
        {
            // Walidacja danych
            ValidateClientData(client);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                // Sprawdzenie czy klient z takim PESEL już istnieje
                using (SqlCommand checkCmd = new SqlCommand("SELECT COUNT(1) FROM Client WHERE Pesel = @Pesel", conn))
                {
                    checkCmd.Parameters.AddWithValue("@Pesel", client.Pesel);
                    var count = (int)await checkCmd.ExecuteScalarAsync();
                    if (count > 0)
                        throw new Exception("Klient z podanym numerem PESEL już istnieje");
                }

                // Dodanie klienta
                string insertQuery = @"
                    INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
                    VALUES (@FirstName, @LastName, @Email, @Telephone, @Pesel);
                    SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", client.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", client.LastName);
                    cmd.Parameters.AddWithValue("@Email", client.Email);
                    cmd.Parameters.AddWithValue("@Telephone", client.Telephone);
                    cmd.Parameters.AddWithValue("@Pesel", client.Pesel);

                    // Pobieramy ID nowo utworzonego klienta
                    var result = await cmd.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> RegisterClientForTrip(int clientId, int tripId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                
                // Sprawdzenie czy klient istnieje
                if (!await CheckClientExists(conn, clientId))
                    return (false, $"Klient o ID {clientId} nie istnieje.");

                // Sprawdzenie czy wycieczka istnieje
                if (!await CheckTripExists(conn, tripId))
                    return (false, $"Wycieczka o ID {tripId} nie istnieje.");

                // Sprawdzenie czy klient jest już zapisany na wycieczkę
                if (await IsClientRegisteredForTrip(conn, clientId, tripId))
                    return (false, "Klient jest już zapisany na tę wycieczkę.");

                // Sprawdzenie dostępności miejsc
                if (!await CheckTripAvailability(conn, tripId))
                    return (false, "Brak wolnych miejsc na wycieczce.");

                // Rejestracja klienta na wycieczkę
                string insertQuery = @"
                    INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt, PaymentDate, PaymentAmount)
                    VALUES (@IdClient, @IdTrip, @RegisteredAt, NULL, NULL)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@IdClient", clientId);
                    cmd.Parameters.AddWithValue("@IdTrip", tripId);
                    cmd.Parameters.AddWithValue("@RegisteredAt", DateTime.Now);

                    await cmd.ExecuteNonQueryAsync();
                    return (true, null);
                }
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> UnregisterClientFromTrip(int clientId, int tripId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                // Sprawdzenie czy rejestracja istnieje
                if (!await IsClientRegisteredForTrip(conn, clientId, tripId))
                    return (false, "Klient nie jest zapisany na tę wycieczkę.");

                // Sprawdzenie czy wycieczka jeszcze się nie rozpoczęła
                if (await HasTripStarted(conn, tripId))
                    return (false, "Nie można wyrejestrować klienta z wycieczki, która już się rozpoczęła.");

                // Usunięcie rejestracji
                string deleteQuery = @"
                    DELETE FROM Client_Trip 
                    WHERE IdClient = @IdClient AND IdTrip = @IdTrip";

                using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@IdClient", clientId);
                    cmd.Parameters.AddWithValue("@IdTrip", tripId);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return (rowsAffected > 0, null);
                }
            }
        }

        public async Task<bool> ClientExists(int clientId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                return await CheckClientExists(conn, clientId);
            }
        }

        #region Metody pomocnicze

        private void ValidateClientData(CreateClientDTO client)
        {
            if (string.IsNullOrWhiteSpace(client.FirstName))
                throw new ArgumentException("Imię jest wymagane");

            if (string.IsNullOrWhiteSpace(client.LastName))
                throw new ArgumentException("Nazwisko jest wymagane");

            if (string.IsNullOrWhiteSpace(client.Email))
                throw new ArgumentException("Email jest wymagany");

            if (!IsValidEmail(client.Email))
                throw new ArgumentException("Niepoprawny format adresu email");

            if (string.IsNullOrWhiteSpace(client.Telephone))
                throw new ArgumentException("Numer telefonu jest wymagany");

            if (string.IsNullOrWhiteSpace(client.Pesel))
                throw new ArgumentException("PESEL jest wymagany");

            if (!IsValidPesel(client.Pesel))
                throw new ArgumentException("Niepoprawny numer PESEL");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPesel(string pesel)
        {
            if (pesel.Length != 11 || !pesel.All(char.IsDigit))
                return false;

            // Tutaj można dodać bardziej zaawansowaną walidację PESEL
            return true;
        }

        private async Task<bool> CheckClientExists(SqlConnection conn, int clientId)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM Client WHERE IdClient = @ClientId", conn))
            {
                cmd.Parameters.AddWithValue("@ClientId", clientId);
                return (int)await cmd.ExecuteScalarAsync() > 0;
            }
        }

        private async Task<bool> CheckTripExists(SqlConnection conn, int tripId)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM Trip WHERE IdTrip = @TripId", conn))
            {
                cmd.Parameters.AddWithValue("@TripId", tripId);
                return (int)await cmd.ExecuteScalarAsync() > 0;
            }
        }

        private async Task<bool> IsClientRegisteredForTrip(SqlConnection conn, int clientId, int tripId)
        {
            using (SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Client_Trip WHERE IdClient = @ClientId AND IdTrip = @TripId", conn))
            {
                cmd.Parameters.AddWithValue("@ClientId", clientId);
                cmd.Parameters.AddWithValue("@TripId", tripId);
                return (int)await cmd.ExecuteScalarAsync() > 0;
            }
        }

        private async Task<bool> CheckTripAvailability(SqlConnection conn, int tripId)
        {
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT 
                    t.MaxPeople,
                    (SELECT COUNT(*) FROM Client_Trip WHERE IdTrip = @TripId) as RegisteredCount
                FROM Trip t
                WHERE t.IdTrip = @TripId", conn))
            {
                cmd.Parameters.AddWithValue("@TripId", tripId);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        int maxPeople = reader.GetInt32(reader.GetOrdinal("MaxPeople"));
                        int registeredCount = reader.GetInt32(reader.GetOrdinal("RegisteredCount"));
                        return registeredCount < maxPeople;
                    }
                    return false;
                }
            }
        }

        private async Task<bool> HasTripStarted(SqlConnection conn, int tripId)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT DateFrom FROM Trip WHERE IdTrip = @TripId", conn))
            {
                cmd.Parameters.AddWithValue("@TripId", tripId);
                var result = await cmd.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    DateTime startDate = (DateTime)result;
                    return startDate <= DateTime.Now;
                }
                return false;
            }
        }

        #endregion
    }
}
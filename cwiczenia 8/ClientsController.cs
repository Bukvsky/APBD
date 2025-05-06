using Microsoft.AspNetCore.Mvc;
using Tutorial8.Models.DTOs;
using Tutorial8.Services;

namespace Tutorial8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsService _clientsService;

        public ClientsController(IClientsService clientsService)
        {
            _clientsService = clientsService;
        }

        /// <summary>
        /// Pobiera wszystkie wycieczki dla danego klienta
        /// </summary>
        /// <param name="id">ID klienta</param>
        /// <returns>Lista wycieczek klienta</returns>
        [HttpGet("{id}/trips")]
        public async Task<IActionResult> GetClientTrips(int id)
        {
            var clientTrips = await _clientsService.GetClientTrips(id);
            if (clientTrips == null)
                return NotFound($"Klient o ID {id} nie istnieje lub nie ma przypisanych wycieczek.");

            return Ok(clientTrips);
        }

        /// <summary>
        /// Tworzy nowego klienta
        /// </summary>
        /// <param name="client">Dane klienta</param>
        /// <returns>ID utworzonego klienta</returns>
        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientDTO client)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var clientId = await _clientsService.CreateClient(client);
                return CreatedAtAction(nameof(GetClientTrips), new { id = clientId }, new { Id = clientId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // W środowisku produkcyjnym nie powinno się zwracać szczegółów wyjątku
                return StatusCode(500, "Wystąpił błąd podczas tworzenia klienta: " + ex.Message);
            }
        }

        /// <summary>
        /// Rejestruje klienta na wycieczkę
        /// </summary>
        /// <param name="id">ID klienta</param>
        /// <param name="tripId">ID wycieczki</param>
        /// <returns>Status operacji</returns>
        [HttpPut("{id}/trips/{tripId}")]
        public async Task<IActionResult> RegisterClientForTrip(int id, int tripId)
        {
            var result = await _clientsService.RegisterClientForTrip(id, tripId);
            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok("Klient został pomyślnie zarejestrowany na wycieczkę.");
        }

        /// <summary>
        /// Usuwa rejestrację klienta z wycieczki
        /// </summary>
        /// <param name="id">ID klienta</param>
        /// <param name="tripId">ID wycieczki</param>
        /// <returns>Status operacji</returns>
        [HttpDelete("{id}/trips/{tripId}")]
        public async Task<IActionResult> UnregisterClientFromTrip(int id, int tripId)
        {
            var result = await _clientsService.UnregisterClientFromTrip(id, tripId);
            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return Ok("Rejestracja klienta na wycieczkę została anulowana.");
        }
    }
}

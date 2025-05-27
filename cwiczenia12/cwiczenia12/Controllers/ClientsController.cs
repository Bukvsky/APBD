using Microsoft.AspNetCore.Mvc;
using cwiczenia12.Services;

namespace TripManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly ITripService _tripService;

        public ClientsController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpDelete("{idClient}")]
        public async Task<IActionResult> DeleteClient(int idClient)
        {
            try
            {
                var result = await _tripService.DeleteClientAsync(idClient);
                
                if (!result)
                    return NotFound(new { message = "Klient o podanym ID nie został znaleziony." });

                return Ok(new { message = "Klient został pomyślnie usunięty." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Wystąpił błąd podczas usuwania klienta.", error = ex.Message });
            }
        }
    }
}
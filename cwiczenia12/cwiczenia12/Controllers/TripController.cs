using Microsoft.AspNetCore.Mvc;
using cwiczenia12.DTOs; 
using cwiczenia12.Services;

namespace cwiczenia12.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripsController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet]
        public async Task<ActionResult<TripListResponseDto>> GetTrips([FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;

                var result = await _tripService.GetTripsAsync(page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new { message = "Wystąpił błąd podczas pobierania wycieczek.", error = ex.Message });
            }
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AddClientToTrip(int idTrip, [FromBody] AddClientToTripDto dto)
        {
            try
            {
                dto.IdTrip = idTrip;
                var result = await _tripService.AddClientToTripAsync(dto);

                if (result)
                    return Ok(new { message = "Klient został pomyślnie dodany do wycieczki." });

                return BadRequest(new { message = "Nie udało się dodać klienta do wycieczki." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new { message = "Wystąpił błąd podczas dodawania klienta do wycieczki.", error = ex.Message });
            }
        }
    }
}
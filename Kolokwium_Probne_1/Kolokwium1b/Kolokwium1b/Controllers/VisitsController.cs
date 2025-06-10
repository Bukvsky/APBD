using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Kolokwium1b.DTOs;
using Kolokwium1b.Services;

namespace Kolokwium1b.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitsController : ControllerBase
    {
        private readonly IVisitService _visitService;

        public VisitsController(IVisitService visitService)
        {
            _visitService = visitService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisitDetailsDto>> GetVisit(int id)
        {
            var visitDetails = await _visitService.GetVisitDetailsAsync(id);
            
            if (visitDetails == null)
                return NotFound();

            return Ok(visitDetails);
        }

        [HttpPost]
        public async Task<ActionResult> CreateVisit([FromBody] CreateVisitDto createVisitDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validation
            if (createVisitDto.VisitId <= 0)
                return BadRequest("Visit ID must be greater than 0");

            if (createVisitDto.ClientId <= 0)
                return BadRequest("Client ID must be greater than 0");

            if (string.IsNullOrWhiteSpace(createVisitDto.MechanicLicenceNumber))
                return BadRequest("Mechanic licence number is required");

            if (createVisitDto.Services == null || !createVisitDto.Services.Any())
                return BadRequest("At least one service is required");

            foreach (var service in createVisitDto.Services)
            {
                if (string.IsNullOrWhiteSpace(service.ServiceName))
                    return BadRequest("Service name cannot be empty");

                if (service.ServiceFee <= 0)
                    return BadRequest("Service fee must be greater than 0");
            }

            var result = await _visitService.CreateVisitAsync(createVisitDto);
            
            if (!result)
                return Conflict("Visit with given ID already exists, or referenced client/mechanic/service does not exist");

            return Created($"api/visits/{createVisitDto.VisitId}", null);
        }
    }
}
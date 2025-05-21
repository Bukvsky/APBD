using Microsoft.AspNetCore.Mvc;
using Cwiczenia11.DTOs;
using Cwiczenia11.Services;

namespace Cwiczenia11.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _service;

        public PrescriptionController(IPrescriptionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescription([FromBody] PrescriptionRequestDTO dto)
        {
            try
            {
                await _service.AddPrescriptionAsync(dto);
                return Ok("Prescription added.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("patient/{id}")]
        public async Task<IActionResult> GetPatientDetails(int id)
        {
            var result = await _service.GetPatientDetailsAsync(id);
            return result == null ? NotFound() : Ok(result);
        }
    }
}

using Kolos.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kolos.Controllers;
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IDbService _dbService;

        public AppointmentController(IDbService dbService)
        {
            _dbService = dbService;
        }
    }
    
}
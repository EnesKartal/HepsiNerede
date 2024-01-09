using HepsiNerede.Application.Services.TimeSimulation;
using HepsiNerede.WebApp.Common;
using Microsoft.AspNetCore.Mvc;

namespace HepsiNerede.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        private readonly ITimeSimulationService _timeSimulationService;

        public TimeController(ITimeSimulationService timeSimulationService)
        {
            _timeSimulationService = timeSimulationService;
        }

        [HttpPost("increaseTime")]
        public ActionResult<ApiResponse> IncreaseHour(int hour)
        {
            var increasedTime = _timeSimulationService.IncreaseTime(hour);
            return Ok(new ApiResponse(message: $"Time is {increasedTime.ToShortTimeString()}"));
        }
    }
}

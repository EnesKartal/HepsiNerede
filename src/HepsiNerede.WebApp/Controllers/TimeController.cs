using HepsiNerede.Application.Services.TimeSimulation;
using HepsiNerede.WebApp.Common;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HepsiNerede.WebApp.Controllers
{
    /// <summary>
    /// API controller for managing simulated time.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        private readonly ITimeSimulationService _timeSimulationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeController"/> class.
        /// </summary>
        /// <param name="timeSimulationService">The time simulation service.</param>
        public TimeController(ITimeSimulationService timeSimulationService)
        {
            _timeSimulationService = timeSimulationService;
        }

        /// <summary>
        /// Increases the simulated time by the specified number of hours.
        /// </summary>
        /// <param name="hour">The number of hours to increase the simulated time.</param>
        /// <returns>Returns an API response indicating the updated time.</returns>
        [HttpPost("increaseTime")]
        public ActionResult<ApiResponse> IncreaseHour(int hour)
        {
            var increasedTime = _timeSimulationService.IncreaseTime(hour);
            return Ok(new ApiResponse(message: $"Time is {increasedTime.ToShortTimeString()}"));
        }
    }
}

using HepsiNerede.Application.Services.TimeSimulation;
using HepsiNerede.WebApp.Common;
using HepsiNerede.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HepsiNerede.Tests.Controllers
{
    public class TimeControllerTests
    {
        [Fact]
        public void IncreaseHour_WithValidHour_ShouldReturnOkResult()
        {
            var timeSimulationServiceMock = new Mock<ITimeSimulationService>();
            var timeController = new TimeController(timeSimulationServiceMock.Object);

            var hour = 3;
            var increasedTime = timeSimulationServiceMock.Object.GetCurrentTime().AddHours(hour);
            timeSimulationServiceMock.Setup(x => x.IncreaseTime(hour)).Returns(increasedTime);

            var result = timeController.IncreaseHour(hour);

            var okResult = Assert.IsType<ActionResult<ApiResponse>>(result);
        }
    }
}
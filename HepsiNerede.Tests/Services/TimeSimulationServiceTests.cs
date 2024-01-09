using HepsiNerede.Services;

namespace HepsiNerede.Tests
{
    public class TimeSimulationServiceTests
    {
        [Fact]
        public void GetCurrentTime_ShouldReturnInitialTime()
        {

            var timeSimulationService = new TimeSimulationService();
            var dateTimeNow = DateTime.Now;
            var expectedTime = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 0, 0, 0);

            var currentTime = timeSimulationService.GetCurrentTime();

            Assert.Equal(expectedTime, currentTime);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public void IncreaseTime_ShouldReturnCorrectTimeAfterIncrease(int hours)
        {
            var timeSimulationService = new TimeSimulationService();

            var dateTimeNow = DateTime.Now;
            var expectedTime = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, hours, 0, 0);

            var increasedTime = timeSimulationService.IncreaseTime(hours);

            Assert.Equal(expectedTime, increasedTime);
        }
    }
}

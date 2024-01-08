using HepsiNerede.Services;

namespace HepsiNerede.Tests
{
    public class TimeSimulationServiceTests
    {
        [Fact]
        public void GetCurrentTime_ShouldReturnInitialTime()
        {
            var timeSimulationService = new TimeSimulationService();
            var expectedTime = new DateTime(2024, 1, 1, 0, 0, 0);

            var currentTime = timeSimulationService.GetCurrentTime();

            Assert.Equal(expectedTime, currentTime);
        }

        [Theory]
        [InlineData(1, 2024, 1, 1, 1, 0, 0)]
        [InlineData(3, 2024, 1, 1, 3, 0, 0)]
        public void IncreaseTime_ShouldReturnCorrectTimeAfterIncrease(int hours, int year, int month, int day, int hour, int minute, int second)
        {
            var timeSimulationService = new TimeSimulationService();
            var expectedTime = new DateTime(year, month, day, hour, minute, second);

            var increasedTime = timeSimulationService.IncreaseTime(hours);

            Assert.Equal(expectedTime, increasedTime);
        }
    }
}

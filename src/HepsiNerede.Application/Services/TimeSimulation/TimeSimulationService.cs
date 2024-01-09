namespace HepsiNerede.Application.Services.TimeSimulation
{
    public interface ITimeSimulationService
    {
        DateTime GetCurrentTime();
        DateTime IncreaseTime(int hours);
    }

    public class TimeSimulationService : ITimeSimulationService
    {
        private DateTime _currentTime;

        public TimeSimulationService()
        {
            var dateTimeNow = DateTime.Now;
            _currentTime = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 0, 0, 0);
        }

        public DateTime GetCurrentTime()
        {
            return _currentTime;
        }

        public DateTime IncreaseTime(int hours)
        {
            _currentTime = _currentTime.AddHours(hours);

            return _currentTime;
        }
    }
}

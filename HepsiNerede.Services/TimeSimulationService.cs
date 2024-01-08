namespace HepsiNerede.Services
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
            _currentTime = new DateTime(2024, 1, 1, 0, 0, 0);
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

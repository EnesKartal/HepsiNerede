using System;

namespace HepsiNerede.Application.Services.TimeSimulation
{
    /// <summary>
    /// Service for simulating time-related operations. This service built for the purpose of testing this project.
    /// </summary>
    public interface ITimeSimulationService
    {
        /// <summary>
        /// Gets the current simulated time.
        /// </summary>
        /// <returns>The current simulated time.</returns>
        DateTime GetCurrentTime();

        /// <summary>
        /// Increases the simulated time by the specified number of hours.
        /// </summary>
        /// <param name="hours">The number of hours to increase the simulated time.</param>
        /// <returns>The updated simulated time.</returns>
        DateTime IncreaseTime(int hours);
    }

    /// <summary>
    /// Implementation of <see cref="ITimeSimulationService"/> for simulating time-related operations.
    /// </summary>
    public class TimeSimulationService : ITimeSimulationService
    {
        private DateTime _currentTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSimulationService"/> class.
        /// </summary>
        public TimeSimulationService()
        {
            var dateTimeNow = DateTime.Now;
            _currentTime = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 0, 0, 0);
        }

        /// <inheritdoc/>
        public DateTime GetCurrentTime()
        {
            return _currentTime;
        }

        /// <inheritdoc/>
        public DateTime IncreaseTime(int hours)
        {
            _currentTime = _currentTime.AddHours(hours);
            return _currentTime;
        }
    }
}

using InvalidHourException = Hepsiburada.Application.Time.Exceptions.InvalidHourException;

namespace Hepsiburada.Application.Infrastructure
{
    public class TimeService : ITimeService
    {
        public int CurrentTime { get; private set; }
        public void IncreaseHour(int hour)
        {
            if (hour <= 0)
            {
                throw new InvalidHourException();
            }

            CurrentTime += hour;
        }
    }
}

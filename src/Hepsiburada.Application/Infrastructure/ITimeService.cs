namespace Hepsiburada.Application.Infrastructure
{
    public interface ITimeService
    {
        public int CurrentTime { get; }
        void IncreaseHour(int hour);
    }
}
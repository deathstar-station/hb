namespace Hepsiburada.Application.Time.Commands.IncreaseTime
{
    public class IncreaseTimeResponse
    {
        public IncreaseTimeResponse(int currentTime)
        {
            CurrentTime = currentTime;
        }
        public int CurrentTime { get; private set; }
    }
}

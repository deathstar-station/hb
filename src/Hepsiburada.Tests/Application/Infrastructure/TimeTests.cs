using Hepsiburada.Application.Infrastructure;
using Hepsiburada.Application.Time.Exceptions;
using Xunit;

namespace Hepsiburada.Tests.Application.Infrastructure
{
    public class TimeServiceTests
    {

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public void Time_Should_Increase_Hour_Correctly(int hour)
        {
            ITimeService timeService = new TimeService();
            timeService.IncreaseHour(hour);

            Assert.Equal(hour, timeService.CurrentTime);
        }

        [Fact]
        public void Time_Should_Show_Correct_Hour_After_A_Few_Increase()
        {
            ITimeService timeService = new TimeService();
            timeService.IncreaseHour(1);
            timeService.IncreaseHour(3);
            timeService.IncreaseHour(5);

            Assert.Equal(9, timeService.CurrentTime);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        [InlineData(-3)]
        public void Time_Should_Throw_Exception_If_Hour_Lower_Than_1(int hour)
        {
            ITimeService timeService = new TimeService();

            Assert.Throws<InvalidHourException>(() =>
            {
                timeService.IncreaseHour(hour);
            });
        }
    }
}

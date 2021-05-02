using FluentAssertions;
using Hepsiburada.Application.Infrastructure;
using Hepsiburada.Application.Time.Commands.IncreaseTime;
using Moq;
using System.Threading;
using Xunit;

namespace Hepsiburada.Tests.Application.IncreaseTime
{
    public class IncreaseTimeCommandTests
    {
        private readonly Mock<ITimeService> _timeServiceMock;
        private readonly IncreaseTimeHandler _handler;

        public IncreaseTimeCommandTests()
        {
            _timeServiceMock = new Mock<ITimeService>();
            _handler = new (_timeServiceMock.Object);
        }

        [Fact]
        public void IncreaseTime_Command_Should_Run_Correctly()
        {
            var hour = 1;
            
            var result = _handler.Handle(new IncreaseTimeCommand { Hour = hour }, CancellationToken.None).Result;

            result.Should().NotBeNull();

            _timeServiceMock.Verify(x => x.IncreaseHour(hour), Times.Once);
        }
    }
}

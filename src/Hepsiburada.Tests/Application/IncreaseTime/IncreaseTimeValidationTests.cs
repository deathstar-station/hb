using FluentAssertions;
using System.Linq;
using Hepsiburada.Application.Time.Commands.IncreaseTime;
using Xunit;

namespace Hepsiburada.Tests.Application.IncreaseTime
{
    public class IncreaseTimeValidationTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void IncreaseTime_Should_Not_Validated_When_Hours_Is_Lower_Than_1(int hour)
        {
            var increaseTimeCommand = new IncreaseTimeCommand { Hour = hour };
            var validator = new IncreaseTimeValidator();
            var validationResult = validator.Validate(increaseTimeCommand);

            validationResult.Errors.Any(x => x.PropertyName == nameof(increaseTimeCommand.Hour)).Should().Be(true);
        }
    }
}

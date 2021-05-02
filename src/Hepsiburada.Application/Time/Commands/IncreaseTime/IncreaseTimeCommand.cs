using FluentValidation;
using MediatR;

namespace Hepsiburada.Application.Time.Commands.IncreaseTime
{
    public class IncreaseTimeCommand : IRequest<IncreaseTimeResponse>
    {
        public int Hour { get; set; }
    }

    public class IncreaseTimeValidator : AbstractValidator<IncreaseTimeCommand>
    {
        public IncreaseTimeValidator()
        {
            RuleFor(x => x.Hour)
                .GreaterThan(0).WithMessage("Hour must be greater than zero");
        }
    }
}

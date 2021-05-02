using Hepsiburada.Application.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Hepsiburada.Application.Time.Commands.IncreaseTime
{
    public class IncreaseTimeHandler : IRequestHandler<IncreaseTimeCommand, IncreaseTimeResponse>
    {
        private readonly ITimeService _timeService;

        public IncreaseTimeHandler(ITimeService timeService)
        {
            _timeService = timeService;
        }

        public Task<IncreaseTimeResponse> Handle(IncreaseTimeCommand request, CancellationToken cancellationToken)
        {
            _timeService.IncreaseHour(request.Hour);

            return Task.FromResult(new IncreaseTimeResponse(_timeService.CurrentTime));
        }
    }
}

using System.Threading.Tasks;
using Hepsiburada.Application.Time.Commands.IncreaseTime;
using MediatR;

namespace Hepsiburada.Presentation.Requests
{
    public class IncreaseTimeRequest : Request
    {
        public IncreaseTimeRequest(string[] parameters, IMediator mediator) : base(parameters, mediator) { }

        public override bool IsValid()
        {
            if (_parameters.Length != 2)
                return false;

            if (_parameters[0] != "increase_time")
                return false;

            if (string.IsNullOrEmpty(_parameters[1]) && !int.TryParse(_parameters[1], out _))
                return false;

            return true;
        }

        public override async Task<string> Process()
        {
            var hour = int.Parse(_parameters[1]);
            var result = await _mediator.Send(new IncreaseTimeCommand { Hour = hour });

            return $"Time {result.CurrentTime}:00 \r\n";
        }
    }
}
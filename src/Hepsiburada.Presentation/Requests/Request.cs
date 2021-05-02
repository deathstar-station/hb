using System.Threading.Tasks;
using MediatR;

namespace Hepsiburada.Presentation.Requests
{
    public abstract class Request
    {
        protected readonly IMediator _mediator;
        protected readonly string[] _parameters;

        protected Request(string[] parameters, IMediator mediator)
        {
            _parameters = parameters;
            _mediator = mediator;
        }

        public abstract bool IsValid();

        public abstract Task<string> Process();
    }
}

using Hepsiburada.Domain.Shared.Abstractions;

namespace Hepsiburada.Domain.Shared.Exceptions
{
    public class EntityDoesNotExistsException : HepsiburadaExceptionBase
    {
        public EntityDoesNotExistsException() : base("Entity doesnt exists!") { }
    }
}

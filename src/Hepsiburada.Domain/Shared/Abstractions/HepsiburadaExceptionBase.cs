using System;

namespace Hepsiburada.Domain.Shared.Abstractions
{
    public abstract class HepsiburadaExceptionBase : Exception
    {
        protected HepsiburadaExceptionBase(string message) : base(message) { }
    }
}
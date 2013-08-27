using System;

namespace Pleiades.Application.Data
{
    public interface IUnitOfWork
    {
        Guid Tracer { get; }
        void SaveChanges();
    }
}

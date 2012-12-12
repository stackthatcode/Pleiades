using System;

namespace Pleiades.Data
{
    public interface IUnitOfWork
    {
        Guid Tracer { get; }
        void SaveChanges();
    }
}

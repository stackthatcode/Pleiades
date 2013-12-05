using System;

namespace Pleiades.App.Data
{
    public interface IUnitOfWork
    {
        Guid Tracer { get; }
        void SaveChanges();
    }
}

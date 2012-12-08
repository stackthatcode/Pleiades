using System;

namespace Pleiades.Data
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}

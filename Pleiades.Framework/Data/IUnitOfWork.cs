using System;

namespace Pleiades.Framework.Data
{
    public interface IUnitOfWork
    {
        void Execute(Action action);
    }
}

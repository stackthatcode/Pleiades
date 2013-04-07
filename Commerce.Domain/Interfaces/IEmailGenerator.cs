using System;
using Commerce.Persist.Model.Email;

namespace Commerce.Persist.Interfaces
{
    public interface IEmailGenerator
    {
        Message OrderReceived();
        Message OrderShipped();
    }
}

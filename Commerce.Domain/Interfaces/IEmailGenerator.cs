using System;
using Commerce.Application.Model.Email;

namespace Commerce.Application.Interfaces
{
    public interface IEmailGenerator
    {
        Message OrderReceived();
        Message OrderShipped();
    }
}

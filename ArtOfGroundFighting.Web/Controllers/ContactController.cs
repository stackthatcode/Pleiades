using System.Web.Mvc;
using ArtOfGroundFighting.Web.Models;
using Commerce.Application.Database;
using Commerce.Application.Email;
using Commerce.Application.Email.Model;
using Commerce.Application.Products;
using Commerce.Application.Shopping;
using Pleiades.Web.Json;

namespace ArtOfGroundFighting.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IAdminEmailBuilder _adminEmailBuilder;
        private readonly IEmailService _emailService;

        public ContactController(IEmailService emailService, IAdminEmailBuilder adminEmailBuilder)
        {
            _emailService = emailService;
            _adminEmailBuilder = adminEmailBuilder;
        }

        [HttpPost]
        [ActionName("action")]
        public JsonNetResult Post(string email, string body)
        {
            var message = _adminEmailBuilder.CustomerContact(
                new CustomerInquiry
                {
                    Email = email, MessageBody = body
                });
            _emailService.Send(message);
            return JsonNetResult.Success();
        }
    }
}

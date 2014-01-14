using System;
using System.Web;

namespace Commerce.Application.Shopping
{
    public class CartIdentificationService : ICartIdentificationService
    {
        private readonly HttpContextBase _context;
        private const string CookieName = "cis";

        public CartIdentificationService(HttpContextBase context)
        {
            _context = context;
        }

        public Guid ProvisionNewCartId()
        {
            var identifier = Guid.NewGuid();
            if (_context.Response.Cookies[CookieName] != null)
            {
                _context.Response.Cookies.Remove(CookieName);
            }
            var cookie = new HttpCookie(CookieName, identifier.ToString());
            _context.Response.Cookies.Add(cookie);
            return identifier;
        }

        public Guid? GetCurrentRequestCartId()
        {
            var cookie = _context.Request.Cookies[CookieName];
            Guid identifier;
            if (cookie != null && Guid.TryParse(cookie.Value, out identifier))
            {
                return identifier;
            }
            else
            {
                return null;
            }
        }
    }
}

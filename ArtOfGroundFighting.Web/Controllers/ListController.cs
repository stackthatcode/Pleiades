using System.Linq;
using System.Web.Mvc;
using Commerce.Application.Database;
using Pleiades.Web.Json;

namespace ArtOfGroundFighting.Web.Controllers
{
    public class ListController : Controller
    {
        private readonly PushMarketContext _pushMarketContext;

        public ListController(PushMarketContext pushMarketContext)
        {
            _pushMarketContext = pushMarketContext;
        }

        [HttpGet]
        [ActionName("action")]
        public JsonNetResult Get(string listId)
        {
            if (listId == "StatesAndTaxes")
            {
                return new JsonNetResult(_pushMarketContext.StateTaxes.OrderBy(x => x.Abbreviation).ToList());                
            }
            if (listId == "ShippingMethods")
            {
                return new JsonNetResult(_pushMarketContext.ShippingMethods.ToList());
            }
            return JsonNetResult.Success();
        }
    }
}

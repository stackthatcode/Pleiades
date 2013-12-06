using System.Collections.Generic;
using System.Web.Http;

namespace Commerce.ArtOfGroundFighting.Controllers
{
    public class SectionController : ApiController
    {
        // GET api/product
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/product/5
        public string Get(int id)
        {
            // TODO: return all Products belong to Section X
            return "value";
        }

        // POST api/product
        public void Post([FromBody]string value)
        {
        }

        // PUT api/product/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/product/5
        public void Delete(int id)
        {
        }
    }
}

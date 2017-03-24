using System.Collections.Generic;
using System.Web.Http;
using System.Threading;

namespace Lab01
{
    public class ValuesController : ApiController
    {
        private readonly IStorage _storage;

        // GET api/values 
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5 
        public string Get(string id)
        {
            return "value";
        }

        // POST api/values 
        public void Post([FromBody]string value)
        {
            Thread.Sleep(500);
        }

        // PUT api/values/5 
        public void Put(string id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(string id)
        {
        }
    }
}
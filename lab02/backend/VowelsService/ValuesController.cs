using System.Collections.Generic;
using System.Web.Http;
using System.Threading;
using System;

namespace VowelsService
{
    public class ValuesController : ApiController
    {
        MessageProducer _producer = new MessageProducer("hello");

        // GET api/values 
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5 
        public string Get(int id)
        {
            Console.WriteLine("Value: " + id);
            return "value";
        }

        // POST api/values 
        public IHttpActionResult Post([FromBody]Data data)
        {
            if (data.value == null)
            {
                Console.WriteLine("Incorrect value");
                throw new Exception("Incorrect value");
            }

            Thread.Sleep(500);
            Console.WriteLine("Writing Data: " + data.value);

            _producer.SendMessage(data.value);

            //_storage.Save(data.id, data.value);
            return StatusCode(System.Net.HttpStatusCode.OK);
        }
    }
}
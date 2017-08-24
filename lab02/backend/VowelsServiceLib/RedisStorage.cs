using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Configuration;

namespace VowelsServiceLib
{
    public class RedisStorage : IStorage
    {
        private readonly IDatabase _db;

        public RedisStorage()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");

            _db = redis.GetDatabase();
        }

        public void Save(string id, string value)
        {
            _db.StringSet(id, value);
        }

        public string Get(string id)
        {
            return _db.StringGet(id);
        }

        public void Delete(string id)
        {
            _db.KeyDelete(id);
        }
    }
}

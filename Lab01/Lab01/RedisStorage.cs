using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Configuration;

namespace Lab01
{
    class RedisStorage : IStorage
    {
        private readonly IDatabase _db;

        public RedisStorage(  )
        {
            var redisConnectionString = ConfigurationManager.AppSettings["redisConnectionString"];
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisConnectionString);            

            _db = redis.GetDatabase();
        }

        public void Save( string id, string value )
        {
            _db.StringSet( id, value );
        }

        public string Get(string id )
        {
            return _db.StringGet( id );
        }
    }
}

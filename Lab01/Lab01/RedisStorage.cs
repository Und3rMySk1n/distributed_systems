using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Lab01
{
    class RedisStorage : IStorage
    {
        private readonly IDatabase _db;

        public RedisStorage( IDatabase db )
        {
            if ( db == null ) throw new ArgumentNullException( nameof( db ) );

            _db = db;
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

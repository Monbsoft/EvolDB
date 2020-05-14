using Microsoft.Extensions.Configuration;
using System;

namespace Monbsoft.EvolDB.Couchbase
{
    public class CouchbaseConfig
    {
        public const string COUCHBASE_BUCKET = "COUCHBASE_BUCKET";
        public const string COUCHBASE_CONNECTIONSTRING = "COUCHBASE_CONNECTIONSTRING";
        public const string COUCHBASE_PASSWORD = "COUCHBASE_PASSWORD";
        public const string COUCHBASE_TYPE = "COUCHBASE_TYPE";
        public const string COUCHBASE_USERNAME = "COUCHBASE_USERNAME";
        public CouchbaseConfig(IConfigurationRoot configuration)
        {
            Bucket = configuration.GetValue<string>(COUCHBASE_BUCKET) ?? throw new ArgumentNullException(nameof(Bucket));
            ConnectionString = configuration.GetValue<string>(COUCHBASE_CONNECTIONSTRING) ?? throw new ArgumentNullException(nameof(ConnectionString));
            Password = configuration.GetValue<string>(COUCHBASE_PASSWORD) ?? throw new ArgumentNullException(nameof(Password));
            Username = configuration.GetValue<string>(COUCHBASE_USERNAME) ?? throw new ArgumentNullException(Username);
            Type = configuration.GetValue<string>(COUCHBASE_TYPE) ?? "type";
        }

        public string Bucket { get; }
        public string ConnectionString { get; }
        public string Password { get; set; }
        public string Type { get; }
        public string Username { get; set; }
    }
}
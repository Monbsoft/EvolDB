using Couchbase;
using Microsoft.Extensions.Configuration;
using Monbsoft.EvolDB.Data;
using System;
using System.Threading.Tasks;

namespace Monbsoft.EvolDB.Couchbase
{
    public class CouchbaseGateway : IDatabaseGateway
    {
        public const string Couchbase_Uri = "COUCHBASE_URI";
        public const string Couchbase_Username = "COUCHBASE_USERNAME";
        public const string Couchbase_Password = "COUCHBASE_PASSWORD";
        public const string Couchbase_Bucket = "COUCHBASE_BUCKET";
        private bool _disposed = false;
        private ICluster _cluster;
        private IBucket _bucket;

        public async Task OpenAsync(IConfigurationRoot configuration)
        {
            var cluster = await Cluster.ConnectAsync(
                configuration.GetValue<string>(Couchbase_Uri),
                configuration.GetValue<string>(Couchbase_Username),
                configuration.GetValue<string>(Couchbase_Password));

            _bucket = await cluster.BucketAsync(configuration.GetValue<string>(Couchbase_Bucket));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _bucket?.Dispose();
                _cluster?.Dispose();
            }

            _disposed = true;
        }

    }
}

using Couchbase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Data;
using System;
using System.Threading.Tasks;

namespace Monbsoft.EvolDB.Couchbase
{
    public class CouchbaseGateway : IDatabaseGateway
    {
        public const string COUCHBASE_BUCKET = "COUCHBASE_BUCKET";
        public const string COUCHBASE_CONNECTIONSTRING = "COUCHBASE_CONNECTIONSTRING";
        public const string COUCHBASE_PASSWORD = "COUCHBASE_PASSWORD";
        public const string COUCHBASE_USERNAME = "COUCHBASE_USERNAME";
        private readonly ILogger<CouchbaseGateway> _logger;
        private IBucket _bucket;
        private ICluster _cluster;
        private bool _disposed = false;

        public CouchbaseGateway(ILogger<CouchbaseGateway> logger)
        {
            _logger = logger;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public async Task OpenAsync(IConfigurationRoot configuration)
        {
            _logger.LogDebug("Opening couchbase...");
            var options = new ClusterOptions()
                .WithConnectionString(configuration.GetValue<string>(COUCHBASE_CONNECTIONSTRING))
                .WithCredentials(configuration.GetValue<string>(COUCHBASE_USERNAME), configuration.GetValue<string>(COUCHBASE_PASSWORD));

            _cluster = await Cluster.ConnectAsync(options);
            _logger.LogDebug($"Cluster {options.ConnectionString} is connected.");

            _bucket = await _cluster.BucketAsync(configuration.GetValue<string>(COUCHBASE_BUCKET));
            _logger.LogDebug($"Bucket {_bucket.Name} is opened.");
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
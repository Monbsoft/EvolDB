using Couchbase;
using Couchbase.Management.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Data;
using Monbsoft.EvolDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monbsoft.EvolDB.Couchbase
{
    public class CouchbaseGateway : IDatabaseGateway
    {
        private readonly CouchbaseConfig _config;
        private readonly ILogger<CouchbaseGateway> _logger;
        private IBucket _bucket;
        private ICluster _cluster;
        private bool _disposed = false;

        public CouchbaseGateway(CouchbaseConfig config, ILogger<CouchbaseGateway> logger)
        {
            _config = config;
            _logger = logger;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public async Task OpenAsync()
        {
            _logger.LogDebug("Opening couchbase...");
            var options = new ClusterOptions()
                .WithConnectionString(_config.ConnectionString)               
                .WithCredentials(_config.Username, _config.Password);

            _cluster = await Cluster.ConnectAsync(options);
            _logger.LogDebug($"Cluster {options.ConnectionString} is connected.");

            _bucket = await _cluster.BucketAsync(_config.Bucket);
            _logger.LogDebug($"Bucket {_bucket.Name} is opened.");
        }

        public async Task<List<Commit>> GetCommitsAsync()
        {
            var collection = _bucket.DefaultCollection();                  
            var commits = await _cluster.QueryAsync<Commit>($"SELECT * FROM {_bucket.Name} WHERE {_config.Type} = \"__commit\"");

            return await commits.Rows.ToListAsync();
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
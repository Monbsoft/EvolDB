using Couchbase;
using Couchbase.Core.Exceptions;
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
            Parser = new DefaultQueryParser();
        }

        public IQueryParser Parser { get; }
        public async Task AddMetadataAsync(CommitMetadata meta)
        {
            string id = Guid.NewGuid().ToString();
            var qb = new Query();
            qb.Insert(_config.Bucket)
                .Values(Guid.NewGuid().ToString(), x =>
                {
                    return x.WithKeyValue(_config.Type, "Commit")
                        .WithKeyValue("Prefix", meta.Prefix)
                        .WithKeyValue("Version", meta.Version)
                        .WithKeyValue("Message", meta.Message)
                        .WithKeyValue("Hash", meta.Hash)
                        .WithKeyValue("Applied", meta.Applied)
                        .WithKeyValue("CreationDate", meta.CreationDate);
                });

            await _cluster.QueryAsync<dynamic>(qb.Build());
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public async Task<List<CommitMetadata>> GetMetadataAsync()
        {
            try
            {
                var commits = await _cluster.QueryAsync<CommitMetadata>($"SELECT meta().id, Applied, CreationDate, `Hash`, Message, Prefix, Version FROM {_bucket.Name} WHERE {_config.Type} = \"Commit\" AND Applied = true");
                return await commits.Rows.ToListAsync();
            }
            catch (PlanningFailureException ex)
            {
                //index n'est pas créé.
                return new List<CommitMetadata>();
            }
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
        public Task PushAsync(QueryToken token)
        {
            return _cluster.QueryAsync<dynamic>(token.Text);
        }
        public Task RemoveMetadataAsync(CommitMetadata meta)
        {
            var collection = _bucket.DefaultCollection();
            return collection.RemoveAsync(meta.Id);
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
            _logger.LogDebug("Couchbase is disposed.");
            _disposed = true;
        }
    }
}
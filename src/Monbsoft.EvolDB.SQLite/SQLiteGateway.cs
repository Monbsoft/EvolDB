using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Data;
using Monbsoft.EvolDB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monbsoft.EvolDB.SQLite
{
    public class SQLiteGateway : IDatabaseGateway
    {
        private readonly SQLiteConfig _config;
        private readonly ILogger<SQLiteGateway> _logger;
        private SqliteConnection _connection;
        private bool _disposed = false;

        public SQLiteGateway(SQLiteConfig config, ILogger<SQLiteGateway> logger)
        {
            _config = config;
            Parser = new DefaultQueryParser();
            _logger = logger;
        }

        public IQueryParser Parser { get; }

        public Task AddMetadataAsync(CommitMetadata meta)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<List<CommitMetadata>> GetMetadataAsync()
        {
            SqliteCommand sqlCommand = null;
            try
            {
                sqlCommand = _connection.CreateCommand();
                sqlCommand.CommandText =
                @"
                    SELECT * FROM _commits;
                ";
                sqlCommand.ExecuteReader();
            }
            catch(SqliteException ex)
            {
                return new List<CommitMetadata>();
            }
            finally
            {
                sqlCommand?.Dispose();
            }
            return new List<CommitMetadata>();
        }
        public Task OpenAsync()
        {
            _logger.LogDebug("Opening SQLite...");
            _connection = new SqliteConnection(_config.ConnectionString);            
            _connection.Open();

            _logger.LogDebug("");
            return Task.CompletedTask;
        }
        public async Task PushAsync(QueryToken query)
        {
            using(var sqlCommand = _connection.CreateCommand())
            {
                sqlCommand.CommandText = query.Text;
                await sqlCommand.ExecuteReaderAsync();
            }

        }
        public Task RemoveMetadataAsync(CommitMetadata meta)
        {
            throw new NotImplementedException();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                _connection?.Dispose();
            }
            _disposed = true;
        }
    }
}
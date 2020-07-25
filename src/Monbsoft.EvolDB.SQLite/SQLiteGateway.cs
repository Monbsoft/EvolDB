﻿using Microsoft.Data.Sqlite;
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

        public async Task AddMetadataAsync(CommitMetadata meta)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS __Commits
                    (
                        CommitId INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        Prefix TEXT NOT NULL,
                        Version TEXT NOT NULL,
                        Message TEXT NOT NULL,
                        Hash TEXT NOT NULL,
                        Applied INTEGER NOT NULL,
                        CreationDate TEXT NOT NULL
                    );
                ";
                await command.ExecuteNonQueryAsync();

                command.CommandText =
                @"
                    INSERT INTO __Commits(Prefix, Version, Message, Hash, Applied, CreationDate)
                    VALUES (@Prefix, @Version, @Message, @Hash, @Applied, @CreationDate)
                ";
                command.Parameters.AddWithValue("@Prefix", meta.Prefix);
                command.Parameters.AddWithValue("@Version", meta.Version);
                command.Parameters.AddWithValue("@Message", meta.Message);
                command.Parameters.AddWithValue("@Hash", meta.Hash);
                command.Parameters.AddWithValue("@Applied", meta.Applied);
                command.Parameters.AddWithValue("@CreationDate", meta.CreationDate);

                await command.ExecuteNonQueryAsync();
            }
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
            catch (SqliteException)
            {
                return new List<CommitMetadata>();
            }
            finally
            {
                sqlCommand?.Dispose();
            }
            return new List<CommitMetadata>();
        }
        public async Task OpenAsync()
        {
            _logger.LogDebug("Opening SQLite...");
            _connection = new SqliteConnection(_config.ConnectionString);
            await _connection.OpenAsync();
            _logger.LogDebug($"{_connection.Database} SQLite is opened.");
        }
        public async Task PushAsync(QueryToken query)
        {
            using (var sqlCommand = _connection.CreateCommand())
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
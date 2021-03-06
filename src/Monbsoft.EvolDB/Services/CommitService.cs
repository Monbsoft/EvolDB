﻿using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Data;
using Monbsoft.EvolDB.Exceptions;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Repositories;
using Monbsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.CommandLine.Parsing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Monbsoft.EvolDB.Services
{
    public class CommitService : ICommitService
    {
        private readonly IDatabaseGateway _gateway;
        private readonly ILogger<CommitService> _logger;
        private readonly IReferenceParser _referenceParser;
        private readonly Repository _repository;
        public CommitService(
            Repository repository,
            IDatabaseGateway gateway,
            ICommitFactory commitFactory,
            ILogger<CommitService> logger)
        {
            if (commitFactory == null)
            {
                throw new ArgumentNullException(nameof(commitFactory));
            }
            _referenceParser = commitFactory.CreateParser(repository);
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates a commit with the specified reference.
        /// </summary>
        /// <param name="reference"></param>
        public void Create(string reference)
        {
            _logger.LogDebug($"Creating commit...");
            var commit = _referenceParser.CreateCommit(reference);

            if (!_repository.ValidateCommit(commit))
            {
                throw new CommitException("A higher version already exists.");
            }

            // créer le fichier du commit
            var commitFile = _repository.GetCommitFile(commit);
            if (commitFile.Exists)
            {
                throw new CommitException($"Commit {reference} already exists.");
            }
            commit.FullName = commitFile.FullName;
            commitFile.Create();
            _logger.LogDebug($"Commit {reference} is created.");
        }

        /// <summary>
        /// Gets the entries.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TreeEntry>> GetTreeEntries()
        {
            await _gateway.OpenAsync();
            var metadata = await _gateway.GetMetadataAsync();

            var tree = new RepositoryTree(_repository, metadata);
            return tree.TreeEntries;
        }

        /// <summary>
        /// Pushs the commits.
        /// </summary>
        /// <returns></returns>
        public async Task PushAsync()
        {
            var stopwatch = new Stopwatch();
            await _gateway.OpenAsync();
            var metadata = await _gateway.GetMetadataAsync();

            var tree = new RepositoryTree(_repository, metadata);
            var commitsToPush = tree.GetCommitsToPush();

            stopwatch.Start();
            foreach (var commit in commitsToPush)
            {
                await PushVersionedCommitAsync(commit).ConfigureAwait(false);
            }
            stopwatch.Stop();
            _logger.LogInformation($"{commitsToPush.Count()} commits are pushed in {stopwatch.ElapsedMilliseconds} ms.");
        }

        /// <summary>
        /// Resets the current commit.
        /// </summary>
        /// <returns></returns>
        public async Task ResetAsync()
        {
            var stopwatch = new Stopwatch();
            await _gateway.OpenAsync();
            var metadata = await _gateway.GetMetadataAsync();

            var tree = new RepositoryTree(_repository, metadata);
            var entry = tree.CurrentEntry;
            if (entry == null || entry.Repeatable == null)
            {
                throw new RepositoryException("No repeatable commit found.");
            }
            stopwatch.Start();
            await PushCommitAsync(entry.Repeatable);
            await _gateway.RemoveMetadataAsync(entry.Target);
            stopwatch.Stop();
            _logger.LogInformation($"Commit {entry.Repeatable.Message} is reset in {stopwatch.ElapsedMilliseconds} ms.");
        }

        private IFileInfo CreateCommitFileInfo(Commit commit)
        {
            return _repository.GetCommitFile(commit);
        }

        /// <summary>
        /// Pushs the commit.
        /// </summary>
        /// <param name="commit"></param>
        /// <returns></returns>
        private async Task PushCommitAsync(Commit commit)
        {
            var commitFile = _repository.GetCommitFile(commit);
            var queries = _gateway.StatementParser.ParseQueries(commitFile.ReadAll());
            foreach (var query in queries)
            {
                await _gateway.PushAsync(query);
            }
        }

        private async Task PushVersionedCommitAsync(Commit commit)
        {
            bool applied = false;
            try
            {
                await PushCommitAsync(commit);
                applied = true;
            }
            finally
            {
                var metadata = new CommitMetadata
                {
                    Prefix = commit.Prefix.ToString(),
                    Version = commit.Version.ToString(),
                    Message = commit.Message,
                    Hash = commit.Hash,
                    Applied = applied,
                    CreationDate = DateTime.UtcNow
                };
                await _gateway.AddMetadataAsync(metadata);
            }
            _logger.LogInformation($"Commit {commit.Message} is pushed.");
        }
    }
}
﻿using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Data;
using Monbsoft.EvolDB.Exceptions;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Repositories;
using Monbsoft.Extensions.FileProviders;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Monbsoft.EvolDB.Services
{
    public class CommitService : ICommitService
    {
        private readonly IDatabaseGateway _gateway;
        private readonly IFileService _fileService;
        private readonly IReferenceParser _referenceParser;
        private readonly Repository _repository;
        private readonly ILogger<CommitService> _logger;

        public CommitService(
            Repository repository,
            IDatabaseGateway gateway,
            IReferenceParser parser,
            IFileService fileService,
            ILogger<CommitService> logger)
        {

            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));
            _referenceParser = parser ?? throw new ArgumentNullException(nameof(parser));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates a commit with the specified reference.
        /// </summary>
        /// <param name="reference"></param>
        public void Create(string reference)
        {
            _logger.LogDebug($"Creating commit...");
            if(!_referenceParser.TryParse(reference, out Commit commit))
            {
                throw new CommitException("Unable to parser the reference.");
            }
            if(_repository.ValidateCommit(commit))
            {
                throw new CommitException("A higher version already exists.");
            }

            // créer le fichier du commit
            var commitFile = CreateCommitFile(_repository, commit);
            if (commitFile.Exists)
            {
                throw new CommitException($"Commit {reference} already exists.");
            }
            commit.FullName = commitFile.FullName;
            commitFile.Create().Close();
            _logger.LogDebug($"Commit {reference} is created.");
        }

        public async Task PushAsync()
        {
            var stopwatch = new Stopwatch();
            await _gateway.OpenAsync();
            var metadata = await _gateway.GetMetadataAsync();

            var tree = new RepositoryTree(_repository, metadata);
            var commitsToPush = tree.GetCommitsToPush();

            stopwatch.Start();
            foreach(var commit in commitsToPush)
            {
                await SendCommitAsync(commit).ConfigureAwait(false);
            }
            stopwatch.Stop();
            _logger.LogInformation($"{commitsToPush.Count()} commits are pushed in {stopwatch.ElapsedMilliseconds} ms.");
        }

        public async Task ResetAsync()
        {
            var stopwaatch = new Stopwatch();
            await _gateway.OpenAsync();
            var metadata = await _gateway.GetMetadataAsync();

            var tree = new RepositoryTree(_repository, metadata);
            
            if(tree.Current?.Value?.Repeatable == null)
            {
                return;
            }

            stopwaatch.Start();


            stopwaatch.Stop();

            _logger.LogInformation("");
        }

        private async Task SendCommitAsync(Commit commit)
        {
                var commitFile = _fileService.GetFile(commit.FullName);
                var lines = commitFile.ReadLines();
                var queries = _gateway.Parser.Parse(lines);
                foreach (var query in queries)
                {
                    await _gateway.PushAsync(query);
                }
        }

        private async Task PushCommitWithMetaAsync(Commit commit)
        {
            bool applied = false;
            try
            {
                await SendCommitAsync(commit);
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


        private IFileInfo CreateCommitFile(Repository repository, Commit commit)
        {
            string path = Path.Combine(repository.CommitFolder.PhysicalPath, commit.ToReference());
            return _fileService.GetFile(path);
        }

    }
}

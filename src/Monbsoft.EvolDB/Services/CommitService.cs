using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Data;
using Monbsoft.EvolDB.Exceptions;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Repositories;
using Monbsoft.Extensions.FileProviders;
using System;
using System.IO;
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

        public void Execute(Commit commit)
        {
            var commitFile = _fileService.GetFile(commit.FullName);
        }

        public async Task Push()
        {
            await _gateway.OpenAsync();
            var metadata = await _gateway.GetMetadataAsync();
            var tree = new RepositoryTree(_repository, metadata);
            foreach(var commit in tree.GetCommitsToApplied())
            {
                await PushCommit(commit).ConfigureAwait(false);
            }
           
        }

        private async Task PushCommit(Commit commit)
        {
            var commitFile = _fileService.GetFile(commit.FullName);
            var lines = commitFile.ReadLines();
            var queries = _gateway.Parser.Parse(lines);
            foreach (var query in queries)
            {
                await _gateway.PushAsync(query);
            }

            var metadata = new CommitMetadata
            {
                Prefix = nameof(commit.Prefix),
                Version = commit.Version.ToString(),
                Message = commit.Message,
                Hash = commit.Hash,
                CreationDate = DateTime.UtcNow
            };
            await _gateway.AddMetadataAsync(metadata);
            _logger.LogDebug($"Commit {commit.Message} is applied.");
        }


        private IFileInfo CreateCommitFile(Repository repository, Commit commit)
        {
            string path = Path.Combine(repository.CommitFolder.PhysicalPath, commit.ToReference());
            return _fileService.GetFile(path);
        }

    }
}

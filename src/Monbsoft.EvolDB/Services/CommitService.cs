using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Exceptions;
using Monbsoft.EvolDB.Models;
using Monbsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Monbsoft.EvolDB.Services
{
    public class CommitService : ICommitService
    {
        private readonly IFileService _fileService;
        private readonly IReferenceParser _referenceParser;
        private readonly Repository _repository;
        private readonly ICommitBuilder _commitBuilder;
        private readonly ILogger<CommitService> _logger;

        public CommitService(
            ICommitBuilder commitBuilder,   
            Repository repository, 
            IReferenceParser parser,
            IFileService fileService,
            ILogger<CommitService> logger)
        {
            _commitBuilder = commitBuilder ?? throw new ArgumentNullException(nameof(commitBuilder));
            _referenceParser = parser ?? throw new ArgumentNullException(nameof(parser));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
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
            if(!IsValid(_repository, commit))
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

        /// <summary>
        /// Validates the specified commit with the repository.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public bool IsValid(Repository repository, Commit commit)
        {
            var max = repository.Commits
                .Where(c => c.Prefix == commit.Prefix)
                .Max(c => c.Version);
            return commit.Version > max;
        }

        private IFileInfo CreateCommitFile(Repository repository, Commit commit)
        {
            string path = Path.Combine(repository.CommitFolder.PhysicalPath, commit.ToName());
            return _fileService.CreateFile(path);
        }

        private void Compare(Repository repository, List<Commit> commits)
        {

        }
    }
}

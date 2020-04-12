using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Data;
using Monbsoft.EvolDB.Exceptions;
using Monbsoft.EvolDB.Models;
using System;

namespace Monbsoft.EvolDB.Services
{
    public class CommitService : ICommitService
    {
        private readonly IMigrationParser _migrationParser;
        private readonly IRepository _repository;
        private readonly ICommitBuilder _commitBuilder;
        private readonly ILogger<CommitService> _logger;

        public CommitService(
            ICommitBuilder commitBuilder,   
            IRepository repository, 
            IMigrationParser parser,
            ILogger<CommitService> logger)
        {
            _commitBuilder = commitBuilder ?? throw new ArgumentNullException(nameof(commitBuilder));
            _migrationParser = parser ?? throw new ArgumentNullException(nameof(parser));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Create(string migration)
        {
            if(!_migrationParser.TryParse(migration, out Commit commit))
            {
                throw new CommitException("Unable to parser the migration.");
            }
            if(!_repository.Validate(commit))
            {
                throw new CommitException("A higher version already exists.");
            }

            // créer le fichier du commit
            var commitFile = _repository.CreateCommitFile(commit.GetName());
            if (commitFile.Exists)
            {
                throw new CommitException($"Commit {migration} already exists.");
            }
            commit.FullName = commitFile.PhysicalPath;
            commitFile.Create().Close();
        }
    }
}

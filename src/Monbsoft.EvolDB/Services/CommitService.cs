using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Data;
using Monbsoft.EvolDB.Excceptions;
using System;
using System.IO;

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
            var commit = _migrationParser.Parse(migration);
            commit.FullName = Path.Combine(_repository.CommitFolder.FullName, commit.GetName());
            if(!_repository.Validate(commit))
            {
                throw new CommitException("A higher version already exists.");
            }
            var commitFile = new FileInfo(commit.FullName);
            if (commitFile.Exists)
            {
                throw new CommitException($"Commit {migration} already exits.");
            }
            commitFile.Create().Close();
        }
    }
}

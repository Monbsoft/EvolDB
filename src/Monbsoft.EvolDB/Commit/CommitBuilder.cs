using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using m = Monbsoft.EvolDB.Models;


namespace Monbsoft.EvolDB.Commit
{
    public class CommitBuilder : ICommitBuilder
    {
        private readonly ILogger<CommitBuilder> _logger;
        private readonly IMigrationParser _parser;
        private readonly IHashService _hashService;

        public CommitBuilder(IMigrationParser parser, IHashService hashService, ILogger<CommitBuilder> logger)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public m.Commit Build(FileInfo file)
        {
            if (file == null || !file.Exists)
            {
                throw new ArgumentNullException(nameof(file));
            }
            _logger.LogDebug($"Building commit {file.Name}...");

            var commit = _parser.Parse(file.Name);
            commit.Hash = _hashService.ComputeHash(file.FullName);
            commit.FullName = file.FullName;

            _logger.LogInformation($"Commit {file.Name} is built.");
            return commit;

        }


    }
}

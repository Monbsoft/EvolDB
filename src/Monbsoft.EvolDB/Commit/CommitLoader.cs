using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Infrastructure;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Repository;
using Monbsoft.EvolDB.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.EvolDB.Migration
{
    public class CommitLoader : ICommitLoader
    {
        private readonly ILogger<CommitLoader> _logger;
        private readonly IHashService _hashService;

        public CommitLoader(IHashService hashService, ILogger<CommitLoader> logger)
        {
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public void Load(IRepository repository)
        {
            _logger.LogDebug("Loading commits...");
            var commitFiles = repository.GetCommitFiles();
            foreach (var commitFile in commitFiles)
            {
                var hash = _hashService.ComputeHash(commitFile.FullName);
                _logger.LogInformation($"{commitFile.Name} is checksum = {hash}.");
            }

            _logger.LogInformation("Commits are loaded.");
        }
    }
}

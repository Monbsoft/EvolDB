using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Models;
using Monbsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace Monbsoft.EvolDB.Repositories
{
    public class RepositoryBuilder : IRepositoryBuilder
    {
        private readonly IFileService _fileService;
        private readonly ICommitBuilder _commitBuilder;
        private readonly ILogger<RepositoryBuilder> _logger;

        public RepositoryBuilder(IFileService fileService, ICommitBuilder commitBuilder, ILogger<RepositoryBuilder> logger)
        {
            _fileService = fileService;
            _commitBuilder = commitBuilder;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Builds the migration repository.
        /// </summary>
        /// <returns></returns>
        public Repository Build()
        {
            _logger.LogDebug("Building repository...");
            var rootFolder = _fileService.GetFolder(Directory.GetCurrentDirectory());

            // configuration
            IConfigurationRoot configuration = null;
            var configFile = _fileService.GetFile(Path.Combine(rootFolder.PhysicalPath, Repository.Config_File));
            if (configFile.Exists)
            {
                configuration = new ConfigurationBuilder()
                    .AddJsonFile(configFile.FullName)
                    .Build();
            }

            var repository = new Repository(rootFolder, configuration);

            // commits
            foreach(var commitFile in repository.GetCommitFiles())
            {
                var commit = _commitBuilder.Build(commitFile);
                repository.Commits.Add(commit);
            }
            _logger.LogDebug($"Repository {repository.Name} is built with {repository.Commits.Count} commits found.");
            return repository;
        }
    }
}

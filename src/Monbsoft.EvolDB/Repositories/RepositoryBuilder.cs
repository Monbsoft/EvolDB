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
        private readonly IDirectoryInfo _rootFolder;
        private readonly ICommitFactory _commitFactory;
        private readonly ILogger<RepositoryBuilder> _logger;

        public RepositoryBuilder(IDirectoryInfo folder, ICommitFactory commitFactory , ILogger<RepositoryBuilder> logger)
        {
            _rootFolder = folder;
            _commitFactory = commitFactory;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Builds the migration repository.
        /// </summary>
        /// <returns></returns>
        public Repository Build()
        {
            _logger.LogDebug("Building repository...");

            // configuration
            IConfigurationRoot configuration = null;
            var configFile = _rootFolder.GetFile(Repository.Config_File);

            if(!configFile.Exists)
            {
                throw new FileNotFoundException("config.json");
            }

            configuration = new ConfigurationBuilder()
                .AddJsonFile(configFile.FullName)
                .Build();

            var repository = new Repository(_rootFolder, configuration);
            var commitBuilder = _commitFactory.CreateBuilder(repository);
            // commits
            foreach (var commitFile in repository.GetCommitFiles())
            {
                var commit = commitBuilder.Build(commitFile);
                repository.Commits.Add(commit);
            }
            _logger.LogDebug($"Repository {repository.Name} is built with {repository.Commits.Count} commits found.");
            return repository;
        }
    }
}
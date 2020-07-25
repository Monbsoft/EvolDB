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
        private readonly ICommitFactory _commitFactory;
        private readonly ILogger<RepositoryBuilder> _logger;

        public RepositoryBuilder(IFileService fileService, ICommitFactory commitFactory , ILogger<RepositoryBuilder> logger)
        {
            _fileService = fileService;
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
            var rootFolder = _fileService.GetFolder(Directory.GetCurrentDirectory());

            // configuration
            IConfigurationRoot configuration = null;
            var configFile = _fileService.GetFile(Path.Combine(rootFolder.PhysicalPath, Repository.Config_File));

            configuration = new ConfigurationBuilder()
                .AddJsonFile(configFile.FullName)
                .Build();

            var repository = new Repository(rootFolder, configuration);
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
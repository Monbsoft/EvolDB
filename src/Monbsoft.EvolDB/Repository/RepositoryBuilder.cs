﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Commit;
using Monbsoft.EvolDB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Monbsoft.EvolDB.Repository
{
    public class RepositoryBuilder : IRepositoryBuilder
    {
        private readonly CommitBuilder _commitBuilder;
        private readonly ILogger<RepositoryBuilder> _logger;
        private DirectoryInfo _folder;
        private DirectoryInfo _commitFolder;

        public RepositoryBuilder(CommitBuilder commitBuilder, ILogger<RepositoryBuilder> logger)
        {
            _commitBuilder = commitBuilder ?? throw new ArgumentNullException(nameof(commitBuilder));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IRepository Build()
        {
            _logger.LogDebug("Building repository...");
            _folder = new DirectoryInfo(Directory.GetCurrentDirectory());
            _commitFolder = GetCommitFolder();

            // configuration
            IConfigurationRoot configuration = null;
            var configFile = new FileInfo(Path.Combine(_folder.FullName, CommitRepository.ConfigFile));
            if (configFile.Exists)
            {
                configuration = new ConfigurationBuilder()
                    .AddJsonFile(configFile.FullName)
                    .Build();
            }

            // commits
            var repository = new CommitRepository(_folder, configuration);
            repository.CommitFolder = _commitFolder;
            repository.Commits = new List<Models.Commit>();
            foreach(var commitFolder in GetCommitFiles())
            {
                var commit = _commitBuilder.Build(commitFolder);
                repository.Commits.Add(commit);
            }
            _logger.LogInformation("Repository is built.");
            return repository;
        }

        private DirectoryInfo GetCommitFolder()
        {
            return _folder.GetDirectories(CommitRepository.Commit_Folder).First();
        }

        private List<FileInfo> GetCommitFiles()
        {
            return _commitFolder.GetFiles().ToList();
        }
    }
}

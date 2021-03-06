﻿using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Services;
using Monbsoft.Extensions.FileProviders;
using System;

namespace Monbsoft.EvolDB.Commits
{
    public class CommitBuilder : ICommitBuilder
    {
        private readonly ILogger<CommitBuilder> _logger;
        private readonly IReferenceParser _parser;
        private readonly IHashService _hashService;

        public CommitBuilder(IReferenceParser parser, IHashService hashService, ILogger<CommitBuilder> logger)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Commit Build(IFileInfo file)
        {
            if (file == null || !file.Exists)
            {
                throw new ArgumentNullException(nameof(file));
            }
            _logger.LogDebug($"Building commit {file.Name}...");

            var commit = _parser.CreateCommit(file.Name);
            commit.Hash = _hashService.ComputeHash(file);
            commit.FullName = file.FullName;

            _logger.LogDebug($"Commit {file.Name} is built.");
            return commit;
        }
    }
}
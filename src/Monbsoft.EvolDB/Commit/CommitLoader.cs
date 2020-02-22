using Monbsoft.EvolDB.Infrastructure;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Repository;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.EvolDB.Migration
{
    public class CommitLoader
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        public void Load(IRepository workspace)
        {
            _logger.Debug("Loading commits...");
            var commits = workspace.GetCommits();
            foreach(var commit in commits)
            {
                var hash = HashUtilities.ComputeHash(commit.FullName);
                _logger.Info($"{commit.Name} is checksum = {hash}.");
            }

            _logger.Info("Commits are loaded.");
        }
    }
}

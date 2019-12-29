using Monbsoft.EvolDB.Infrastructure;
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
            var scripts = workspace.GetScripts();
            foreach(var script in scripts)
            {
                var hash = HashUtilities.ComputeHash(script.FullName);
                _logger.Info($"{script.Name} is checksum = {hash}.");
            }

            _logger.Info("Commits are loaded.");
        }
    }
}

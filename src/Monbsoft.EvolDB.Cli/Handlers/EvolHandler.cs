using Monbsoft.EvolDB.Migration;
using Monbsoft.EvolDB.Repository;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.EvolDB.Cli.Handlers
{
    public static class EvolHandler
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public static void NewExecute(IRepository workspace)
        {
            _logger.Debug("Creating workspace...");
            workspace.Create();

            _logger.Info($"Workspace \"{workspace.Name}\" is created.");
        }

        public static void AddExecute(IRepository workspace)
        {
            _logger.Debug("Adding script...");
            var loader = new CommitLoader();
            loader.Load(workspace);
            _logger.Info("Script is added.");
        }
    }
}

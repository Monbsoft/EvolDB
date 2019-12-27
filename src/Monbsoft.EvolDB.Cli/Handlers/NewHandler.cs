using Monbsoft.EvolDB.Workspace;
using NLog;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;

namespace Monbsoft.EvolDB.Cli.Handlers
{
    public static class NewHandler
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public static void Execute(IWorkspace workspace)
        {
            _logger.Debug("Creating workspace...");
            workspace.Create();

            _logger.Info($"Workspace \"{workspace.Name}\" is created.");
        }
    }
}

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Monbsoft.EvolDB.Workspace
{
    public class WorkspaceBuilder : IWorkspaceBuilder
    {
        public IWorkspace Build()
        {
            var folder = new DirectoryInfo(Directory.GetCurrentDirectory());

            // configuration
            IConfigurationRoot configuration = null;
            var configFile = new FileInfo(Path.Combine(folder.FullName, Workspace.ConfigFile));
            if (configFile.Exists)
            {
                configuration = new ConfigurationBuilder()
                    .AddJsonFile(configFile.FullName)
                    .Build();
            }

            return new Workspace(folder.FullName, configuration);
        }
    }
}

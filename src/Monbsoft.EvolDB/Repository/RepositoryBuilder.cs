using Microsoft.Extensions.Configuration;
using Monbsoft.EvolDB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Monbsoft.EvolDB.Repository
{
    public class RepositoryBuilder : IRepositoryBuilder
    {
        public IRepository Build()
        {
            var folder = new DirectoryInfo(Directory.GetCurrentDirectory());

            // configuration
            IConfigurationRoot configuration = null;
            var configFile = new FileInfo(Path.Combine(folder.FullName, CommitRepository.ConfigFile));
            if (configFile.Exists)
            {
                configuration = new ConfigurationBuilder()
                    .AddJsonFile(configFile.FullName)
                    .Build();
            }

            return new CommitRepository(folder.FullName, configuration);
        }
    }
}

using Microsoft.Extensions.Configuration;
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
            var configFile = new FileInfo(Path.Combine(folder.FullName, MigrationRepository.ConfigFile));
            if (configFile.Exists)
            {
                configuration = new ConfigurationBuilder()
                    .AddJsonFile(configFile.FullName)
                    .Build();
            }

            return new MigrationRepository(folder.FullName, configuration);
        }
    }
}

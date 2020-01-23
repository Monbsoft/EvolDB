using Couchbase;
using Couchbase.Authentication;
using Couchbase.Configuration.Client;
using Microsoft.Extensions.Configuration;
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
        public static void InitExecute()
        {
            _logger.Debug("Creating repository...");


        }

        public static void CommitExecute(IRepository repository)
        {
            _logger.Debug("Creating commit...");
            var loader = new CommitLoader();
            loader.Load(repository);
            _logger.Info($"Commit is created.");
        }

        public static void TestExecute(IRepository repository)
        {
            var configuration = repository.Configuration;
            var test = configuration.GetConnectionString("test");

            var config = new ClientConfiguration
            {

                Servers = { new Uri("http://localhost:8091") },

                UseSsl = false
            };

            var authenticator = new PasswordAuthenticator("Administrator", "xxxx");
            config.SetAuthenticator(authenticator);


            ClusterHelper.Initialize(config);
            var bucket  = ClusterHelper.GetBucket("data");
        }
    }
}

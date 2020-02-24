using Couchbase;
using Couchbase.Authentication;
using Couchbase.Configuration.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using m = Monbsoft.EvolDB.Models;
using NLog;
using System;
using Monbsoft.EvolDB.Commit;
using Monbsoft.EvolDB.Services;

namespace Monbsoft.EvolDB.Cli.Handlers
{
    public static class EvolHandler
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public static void InitExecute()
        {
            _logger.Debug("Creating repository...");
            

        }

        public static void CommitExecute(string migration, IHost host)
        {
            _logger.Debug("Creating commit...");
            var commitService = host.Services.GetRequiredService<ICommitService>();
            commitService.Create(migration);
            _logger.Info($"Commit is created.");
        }

        public static void TestExecute(m.IRepository repository)
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

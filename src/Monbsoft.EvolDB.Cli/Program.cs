using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Cli.Handlers;
using Monbsoft.EvolDB.Extensions;
using Monbsoft.EvolDB.Migration;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Repository;
using Monbsoft.EvolDB.Services;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Monbsoft.EvolDB.Cli
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();

            var rootCommand = new RootCommand();
            rootCommand.Description = "EvolDB is a simple database migration tool for .Net Core.";

            // commande init
            var initCommand = new Command("init");
            initCommand.Description = "Create a migration repository";
            initCommand.Handler = CommandHandler.Create<string>(name =>
            {
                logger.Debug("Creating repository...");
                var directory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), name));                
                if(directory.Exists)
                {
                    return;
                }
                directory.Create();
                var repository = new CommitRepository(directory.FullName);
                repository.Create();
                logger.Info($"Repository {name} is created.");
            });
            initCommand.AddArgument(new Argument<string>("name"));
            rootCommand.AddCommand(initCommand);

            // commande commit
            var commitCommand = new Command("commit");
            commitCommand.Description = "Create a commit";
            commitCommand.AddArgument(new Argument<string>("message"));
            commitCommand.Handler = CommandHandler.Create<string, IHost, IRepository>(EvolHandler.CommitExecute);
            rootCommand.AddCommand(commitCommand);


            // commande test
            var testCommand = new Command("test");
            testCommand.Description = "Test a commit";
            testCommand.Handler = CommandHandler.Create<IRepository>(EvolHandler.TestExecute);
            rootCommand.AddCommand(testCommand);
            try
            {

                var parser = new CommandLineBuilder(rootCommand)
                    .UseHost(host =>
                    {
                        host.ConfigureLogging((context, loggingBuilder) =>
                        {
                            loggingBuilder.ClearProviders();
                            loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                            loggingBuilder.AddNLog();
                        });
                        host.ConfigureServices(services =>
                        {
                            services.AddSingleton<IHashService, HashService>();
                            services.AddSingleton<ICommitLoader, CommitLoader>();

                        });
                    })
                    .UseVersionOption()
                    .ConfigureRepository()
                    .UseHelp()
                    .Build();
                return await parser.InvokeAsync(args);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Cli.Handlers;
using Monbsoft.EvolDB.Commit;
using Monbsoft.EvolDB.Excceptions;
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
                CommitRepository.Create(name);
                logger.Info($"Repository {name} is created.");
            });
            initCommand.AddArgument(new Argument<string>("name"));
            rootCommand.AddCommand(initCommand);

            // commande commit
            var commitCommand = new Command("commit");
            commitCommand.Description = "Create a commit";
            commitCommand.AddArgument(new Argument<string>("migration"));
            commitCommand.Handler = CommandHandler.Create<string, IHost>(EvolHandler.CommitExecute);
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
                            services.AddSingleton<IMigrationParser, MigrationParser>();
                            services.AddSingleton<IRepositoryBuilder, RepositoryBuilder>();
                            services.AddSingleton<ICommitBuilder, CommitBuilder>();
                            services.AddSingleton<ICommitService, CommitService>();
                            services.AddSingleton<IRepository, CommitRepository>(services =>
                            {
                                var builder = services.GetRequiredService<IRepositoryBuilder>();
                                return (CommitRepository)builder.Build();
                            });


                        });
                    })
                    .UseVersionOption()
                    .UseHelp()
                    .Build();
                return await parser.InvokeAsync(args);
            }
            catch(Exception ex) when (ex.InnerException is CommitException)
            {
                logger.Error(ex.InnerException.Message);
                throw;
            }
            catch(Exception ex)
            {
                logger.Error(ex, $"Stopped program because of exception: {ex.Message}");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}

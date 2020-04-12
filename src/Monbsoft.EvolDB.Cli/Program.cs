using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Cli.Commands;
using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Exceptions;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Repositories;
using Monbsoft.EvolDB.Services;
using Monbsoft.Extensions.FileProviders;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
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

            try
            {

                var parser = new CommandLineBuilder(rootCommand)                   
                    .AddCommand(InitCommand.Create())
                    .AddCommand(CommitCommand.Create())
                    .AddCommand(PushCommand.Create())
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
                            services.AddSingleton<IFileService, PhysicaFileService>();
                            services.AddSingleton<IHashService, HashService>();
                            services.AddSingleton<IMigrationParser, MigrationParser>();
                            services.AddSingleton<IRepositoryBuilder, RepositoryBuilder>();
                            services.AddSingleton<ICommitBuilder, CommitBuilder>();
                            services.AddSingleton<ICommitService, CommitService>();
                            services.AddSingleton<IRepositoryService, RepositoryService>();
                            services.AddSingleton<Repository>(services =>
                            {
                                var builder = services.GetRequiredService<IRepositoryBuilder>();
                                return builder.Build();
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

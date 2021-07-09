using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Cli.Commands;
using Monbsoft.EvolDB.Cli.Data;
using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Data;
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
using System.CommandLine.Parsing;
using System.IO;
using System.Threading.Tasks;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Monbsoft.EvolDB.Cli
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();

            var rootCommand = new RootCommand
            {

                Description = "EvolDB is a simple database as code."
            };

            try
            {
                var parser = new CommandLineBuilder(rootCommand)
                    .AddCommand(InitCommand.Create())
                    .AddCommand(CommitCommand.Create())
                    .AddCommand(LogCommand.Create())
                    .AddCommand(PushCommand.Create())
                    .AddCommand(ResetCommand.Create())
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
                            services.AddSingleton<IFileService, PhysicalFileService>();
                            services.AddSingleton<ICommitFactory, CommitFactory>();
                            services.AddSingleton<IRepositoryBuilder, RepositoryBuilder>();
                            services.AddSingleton<GatewayFactory>();
                            services.AddSingleton<IHashService, HashService>();
                            services.AddSingleton<ICommitService, CommitService>();
                            services.AddSingleton<IRepositoryService, RepositoryService>();
                            services.AddSingleton<Repository>(services =>
                            {
                                var commitFactory = services.GetRequiredService<ICommitFactory>();
                                var logger = services.GetService<ILogger<RepositoryBuilder>>();
                                var folder = new PhysicalDirectoryInfo(Directory.GetCurrentDirectory());

                                var builder = new RepositoryBuilder(folder, commitFactory, logger);
                                return builder.Build();
                            });
                            services.AddSingleton<IDatabaseGateway>(services =>
                            {
                                var factory = services.GetRequiredService<GatewayFactory>();
                                return factory.CreateGateway();
                            });
                        });
                    })
                    .UseVersionOption()
                    .UseDefaults()
                    .UseHelp()
                    .Build();
                return await parser.InvokeAsync(args);
            }
            catch (Exception ex) when (ex.InnerException is CommitException)
            {
                logger.Error(ex.InnerException.Message);
                throw;
            }
            catch (Exception ex)
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
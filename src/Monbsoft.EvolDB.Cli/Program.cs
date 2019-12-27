using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Cli.Handlers;
using Monbsoft.EvolDB.Extensions;
using Monbsoft.EvolDB.Workspace;
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

            var newCommand = new Command("new");
            newCommand.Description = "Creates a migration workspace";
            newCommand.Handler = CommandHandler.Create<IWorkspace>(NewHandler.Execute);
            rootCommand.AddCommand(newCommand);

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
                    })
                    .UseVersionOption()
                    .ConfigureWorkspace()
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

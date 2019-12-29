using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Cli.Handlers;
using Monbsoft.EvolDB.Extensions;
using Monbsoft.EvolDB.Repository;
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

            var newCommand = new Command("init");
            newCommand.Description = "Creates a migration repository";
            newCommand.Handler = CommandHandler.Create<IRepository>(EvolHandler.NewExecute);
            rootCommand.AddCommand(newCommand);

            var addCommand = new Command("add");
            addCommand.Description = "Adds a commit";
            addCommand.Handler = CommandHandler.Create<IRepository>(EvolHandler.AddExecute);
            rootCommand.AddCommand(addCommand);

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

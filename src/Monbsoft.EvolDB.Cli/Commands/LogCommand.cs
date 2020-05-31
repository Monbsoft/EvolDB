using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Monbsoft.EvolDB.Services;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using System;
using Monbsoft.EvolDB.Repositories;

namespace Monbsoft.EvolDB.Cli.Commands
{
    public class LogCommand
    {
        private readonly IConsole _console;
        private readonly ICommitService _commitService;

        public LogCommand(IConsole console, ICommitService commitService)
        {
            _console = console;
            _commitService = commitService;
        }

        public static Command Create()
        {
            var command = new Command(
                name: "log",
                description: "Show commit logs.");


            command.Handler = CommandHandler.Create< IHost>(async (host) =>
            {
                var commitService = host.Services.GetRequiredService<ICommitService>();
                var console = host.Services.GetRequiredService<IConsole>();
                
                var logCommand = new LogCommand(console, commitService);
                await logCommand.ShowLogs();
            });

            return command;
        }

        public async Task ShowLogs()
        {
            var entries = await _commitService.GetTreeEntries();
            foreach(var entry in entries)
            {
                PrintEntry(entry);
            }
        }
        public void PrintEntry(TreeEntry entry)
        {
            if(entry == null)
            {
                return;
            }
            var commit = entry?.Source;
            if (commit == null)
            {
                return;
            }
            Console.WriteLine($"commit {commit.FullName}");
            Console.WriteLine($"Hash: {commit.Hash}");            
            Console.WriteLine($"Applied: {entry?.Target?.Applied ?? false}");
            if (entry.Target != null)
            {
                Console.WriteLine($"Date: {entry.Target.CreationDate.ToString("O")}");
            }
            Console.WriteLine();
        }
        
    }
}

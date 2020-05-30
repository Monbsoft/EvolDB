using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Monbsoft.EvolDB.Services;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Monbsoft.EvolDB.Cli.Commands
{
    public class PushCommand
    {
        private readonly ICommitService _commitService;

        public PushCommand(ICommitService commitService)
        {
            _commitService = commitService;
        }

        public static Command Create()
        {
            var command = new Command(
                name: "push",
                description: "Update remote commits using local commits")
            {
                Handler = CommandHandler.Create<IHost>(async (host) =>
                {
                    var commitService = host.Services.GetRequiredService<ICommitService>();

                    var pushCommand = new PushCommand(commitService);
                    await pushCommand.ExecuteAsync();

                })
            };

            return command;
        }

        public async Task ExecuteAsync()
        {
            await _commitService.Push().ConfigureAwait(false);

            
        }
    }
}

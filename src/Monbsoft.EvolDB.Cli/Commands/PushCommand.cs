using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Monbsoft.EvolDB.Data;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Services;
using Monbsoft.Extensions.FileProviders;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;

namespace Monbsoft.EvolDB.Cli.Commands
{
    public class PushCommand
    {
        private readonly IDatabaseGateway _databaseGateway;
        private readonly Repository _repository;
        private readonly IDifferenceService _differenceService;     

        public PushCommand(IDatabaseGateway gateway, Repository repository, IDifferenceService differenceService)
        {
            _databaseGateway = gateway;
            _repository = repository;
            _differenceService = differenceService;
        }

        public static Command Create()
        {
            var command = new Command(
                name: "push",
                description: "Update remote commits using local commits")
            {
                Handler = CommandHandler.Create<IHost>(async (host) =>
                {
                    var gateway = host.Services.GetRequiredService<IDatabaseGateway>();
                    var repository = host.Services.GetRequiredService<Repository>();
                    var diffService = host.Services.GetRequiredService<IDifferenceService>();

                    var pushCommand = new PushCommand(gateway, repository, diffService);
                    await pushCommand.ExecuteAsync();

                })
            };

            return command;
        }

        public async Task ExecuteAsync()
        {
            var commit = _repository.Commits.First();
            var parser = new DefaultQueryParser();
            var file = new PhysicalFileInfo(commit.FullName);
            var lines  = file.ReadLines();
            var tokens = parser.Parse(lines);

            
        }
    }
}

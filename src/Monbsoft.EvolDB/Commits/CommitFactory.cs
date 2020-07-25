using Microsoft.Extensions.Logging;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Services;

namespace Monbsoft.EvolDB.Commits
{
    public class CommitFactory : ICommitFactory
    {
        private readonly IHashService _hashService;
        private readonly ILoggerFactory _loggerFactory;

        public CommitFactory(IHashService hashService, ILoggerFactory loggerFactory)
        {
            _hashService = hashService;
            _loggerFactory = loggerFactory;
        }

        public ICommitBuilder CreateBuilder(Repository repository)
        {
            var parser = CreateParser(repository);

            return new CommitBuilder(parser, _hashService, _loggerFactory.CreateLogger<CommitBuilder>());
        }

        public IReferenceParser CreateParser(Repository repository)
        {
            return new ReferenceParser(repository.GetFileExtension());
        }

    }
}

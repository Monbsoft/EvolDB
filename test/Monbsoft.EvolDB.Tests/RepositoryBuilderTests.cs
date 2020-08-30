using Microsoft.Extensions.Logging.Abstractions;
using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Repositories;
using Monbsoft.EvolDB.Services;
using Monbsoft.EvolDB.Tests.Infrastructure;
using System.IO;
using System.Linq;
using Xunit;

namespace Monbsoft.EvolDB.Tests
{
    public class RepositoryBuilderTests
    {
        [Fact]
        public void Build_without_config_file()
        {
            using (var fs = new DisposableFileSystem())
            {
                var builder = InitializeTestBuilder(fs);
                Assert.Throws<FileNotFoundException>(() => builder.Build());
            }
        }

        [Fact]
        public void Build_repository()
        {
            using (var fs = new DisposableFileSystem()
                .CreateFileWithContent("config.json", "{ \"ConnectionType\": \"COUCHBASE\"}")
                .CreateFolder("commits")
                .CreateFile("commits/V1_0_0_0__init.n1ql"))
            {
                var builder = InitializeTestBuilder(fs);
                var repository = builder.Build();

                Assert.Equal("V1_0_0_0__init.n1ql", repository.Commits.First().ToReference());
            }
        }

        private static IRepositoryBuilder InitializeTestBuilder(DisposableFileSystem fs)
        {
            var commitFactory = new CommitFactory(new HashService(), NullLoggerFactory.Instance);

            return new RepositoryBuilder(fs.DirectoryInfo, commitFactory, NullLogger<RepositoryBuilder>.Instance);
        }
    }
}
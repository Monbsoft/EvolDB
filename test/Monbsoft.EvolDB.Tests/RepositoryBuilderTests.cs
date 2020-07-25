using Microsoft.Extensions.Logging.Abstractions;
using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Repositories;
using Monbsoft.EvolDB.Services;
using Monbsoft.EvolDB.Tests.Infrastructure;
using System.IO;
using Xunit;

namespace Monbsoft.EvolDB.Tests
{
    public class RepositoryBuilderTests
    {
        [Fact]
        public void Build_without_config_file()
        {
            var builder = CreateTestBuilder();

            Assert.Throws<FileNotFoundException>(() => builder.Build());
        }

        private static IRepositoryBuilder CreateTestBuilder()
        {
            var fileService = new TestFileService()
                .WithFolder(
                new TestDirectoryInfo("repository")
                {
                    Exists = true,
                    PhysicalPath = "/dev/repository",
                }.WithDirectory(
                    new TestDirectoryInfo("commits")
                    {
                        Exists = true
                    }))
                .WithFile(new TestFileInfo("config.json")
                {
                    FullName = "/dev/repository/config.json",
                    Exists = false
                });
            var commitFactory = new TestCommitFactory("n1ql");


            return new RepositoryBuilder(fileService, commitFactory, NullLogger<RepositoryBuilder>.Instance);
        }
    }
}
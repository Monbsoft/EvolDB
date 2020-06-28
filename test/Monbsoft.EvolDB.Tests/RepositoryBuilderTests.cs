using Castle.Core.Configuration;
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
            var commitBuilder = new CommitBuilder(new ReferenceParser(), new HashService(), NullLogger<CommitBuilder>.Instance);
            var builder = new RepositoryBuilder(fileService, commitBuilder, NullLogger<RepositoryBuilder>.Instance);

            Assert.Throws<FileNotFoundException>(() => builder.Build());
        }
    }
}
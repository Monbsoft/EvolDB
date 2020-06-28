using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Data;
using Monbsoft.EvolDB.Exceptions;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Services;
using Monbsoft.EvolDB.Tests.Infrastructure;
using Moq;
using Xunit;

namespace Monbsoft.EvolDB.Tests
{
    public class CommitServiceTests
    {
        [Fact]
        public void Create_commit()
        {
            (var commitService, var commitFile) = InitializeTest();

            commitService.Create("V1_0_0_0__init.n1ql");

            Assert.True(commitFile.Exists);
        }

        [Fact]
        public void Create_commit_with_bad_reference()
        {
            (var commitService, var commitFile) = InitializeTest();

            Assert.Throws<CommitException>(() => commitService.Create("V10000.n1ql"));
        }
        
        private (CommitService service, TestFileInfo commitFile) InitializeTest()
        {
            var testFolder = new TestDirectoryInfo("repository")
            {
                Exists = true,
                PhysicalPath = "/dev/repository"
            }.WithDirectory(new TestDirectoryInfo("commits")
            {
                Exists = true,
                PhysicalPath = "/dev/repository/commits"
            });

            var commitFile = new TestFileInfo("V1_0_0_0__init.n1ql")
            {
                Exists = false,
                FullName = "/dev/repository/commits/V1_0_0_0__init.n1ql"
            };

            var fileService = new TestFileService()
                .WithFile(commitFile);


            var mockConfig = new Mock<IConfigurationRoot>();
            mockConfig.Setup(config => config[It.IsAny<string>()]).Returns("COUCHBASE_TYPE");
            var repository = new Repository(testFolder, mockConfig.Object);
            var mockGateway = new Mock<IDatabaseGateway>();
            var commitService = new CommitService(
                repository,
                mockGateway.Object,
                new ReferenceParser(),
                fileService,
                NullLogger<CommitService>.Instance);

            return (commitService, commitFile);
        }
    }
}

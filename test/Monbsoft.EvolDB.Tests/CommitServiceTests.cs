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
            var folder = InitializeFolder();
            var commitFile = InitializeCommitFile();
            var commitService = InitializeService(folder, commitFile);

            commitService.Create("V1_0_0_0__init.n1ql");

            Assert.True(commitFile.Exists);
        }

        [Fact]
        public void Create_commit_exist()
        {
            var folder = InitializeFolder();
            var commitFile = InitializeCommitFile();
            commitFile.Exists = true;
            var commitService = InitializeService(folder, commitFile);

            var exeception = Assert.Throws<CommitException>(() => commitService.Create("V1_0_0_0__init.n1ql"));
            Assert.Equal("Commit V1_0_0_0__init.n1ql already exists.", exeception.Message);
        }

        [Fact]
        public void Create_commit_with_small_version()
        {
            var folder = InitializeFolder();
            var commitFile = InitializeCommitFile();
            ((TestDirectoryInfo)folder.GetFolder("commits")).WithFile(commitFile);
            commitFile.Exists = true;
            var fileService = new TestFileService()
                .WithFile(commitFile);
            var mockConfig = new Mock<IConfigurationRoot>();
            mockConfig.Setup(config => config[It.IsAny<string>()]).Returns("COUCHBASE_TYPE");
            var repository = new Repository(folder, mockConfig.Object);
            repository.Commits.Add(
                new Commit
                {
                    Prefix = Prefix.Versioned,
                    Version = new CommitVersion(1, 0, 0, 0),
                    Message = "init"
                });
            var mockGateway = new Mock<IDatabaseGateway>();
            var commitService = new CommitService(
                repository,
                mockGateway.Object,
                new ReferenceParser(),
                fileService,
                NullLogger<CommitService>.Instance);

            var exception = Assert.Throws<CommitException>(() => commitService.Create("V0_95_5_6__init.n1ql"));
            Assert.Equal("A higher version already exists.", exception.Message);
        }

        [Fact]
        public void Create_commit_with_bad_reference()
        {
            var folder = InitializeFolder();
            var commitFile = InitializeCommitFile();
            var commitService = InitializeService(folder, commitFile);

            var exception = Assert.Throws<CommitException>(() => commitService.Create("V10000.n1ql"));
            Assert.Equal("Commit reference is invalid.", exception.Message);
        }

        private TestDirectoryInfo InitializeFolder()
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
            return testFolder;
        }

        private TestFileInfo InitializeCommitFile()
        {
            var commitFile = new TestFileInfo("V1_0_0_0__init.n1ql")
            {
                Exists = false,
                FullName = "/dev/repository/commits/V1_0_0_0__init.n1ql"
            };
            return commitFile;
        }

        private CommitService InitializeService(TestDirectoryInfo folder, TestFileInfo commitFile)
        {
            var fileService = new TestFileService()
                .WithFile(commitFile);

            var mockConfig = new Mock<IConfigurationRoot>();
            mockConfig.Setup(config => config[It.IsAny<string>()]).Returns("COUCHBASE_TYPE");
            var repository = new Repository(folder, mockConfig.Object);
            var mockGateway = new Mock<IDatabaseGateway>();
            var commitService = new CommitService(
                repository,
                mockGateway.Object,
                new ReferenceParser(),
                fileService,
                NullLogger<CommitService>.Instance);

            return commitService;
        }
    }
}
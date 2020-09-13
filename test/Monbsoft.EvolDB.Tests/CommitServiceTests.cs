using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Data;
using Monbsoft.EvolDB.Exceptions;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Repositories;
using Monbsoft.EvolDB.Services;
using Monbsoft.EvolDB.Tests.Infrastructure;
using Monbsoft.Extensions.FileProviders;
using Moq;
using Xunit;

namespace Monbsoft.EvolDB.Tests
{
    public class CommitServiceTests
    {
        [Fact]
        public void Create_commit()
        {
            using (var fs = new DisposableFileSystem()
                .CreateFolder("commits"))
            {

                var commitService = InitializeService(fs);

                commitService.Create("V1_0_0_0__init.n1ql");
                var commitFile = fs.GetFile("commits/V1_0_0_0__init.n1ql");

                Assert.True(commitFile.Exists);
            }
        }
        

        [Fact]
        public void Create_commit_with_small_version()
        {
            using(var fs = new DisposableFileSystem()
                .CreateFolder("commits")
                .CreateFile("commits/V1_0_0_0__init.n1ql"))
            {
                var commitService = InitializeService(fs);

                var exception = Assert.Throws<CommitException>(() => commitService.Create("V0_95_5_6__init.n1ql"));
                Assert.Equal("A higher version already exists.", exception.Message);

            }
        }

        [Fact]
        public void Create_commit_with_bad_reference()
        {
            using (var fs = new DisposableFileSystem()
                .CreateFolder("commits"))
            {
                var commitService = InitializeService(fs);

                var exception = Assert.Throws<CommitException>(() => commitService.Create("V10000.n1ql"));
                Assert.Equal("Commit reference is invalid.", exception.Message);
            }
        }


        private CommitService InitializeService(DisposableFileSystem fs)
        {
            fs.CreateFileWithContent("config.json", "{ \"ConnectionType\": \"COUCHBASE\"}");

            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile(fs.GetFile("config.json").FullName);
            var config = configBuilder.Build();


            var repositoryBuilder = new RepositoryBuilder(
                fs.DirectoryInfo, 
                new CommitFactory(new HashService(), NullLoggerFactory.Instance),
                NullLogger<RepositoryBuilder>.Instance);
            var repository = repositoryBuilder.Build();

            var commitFactory = new CommitFactory(new HashService(), NullLoggerFactory.Instance);

            var mockGateway = new Mock<IDatabaseGateway>();

            var commitService = new CommitService(
                repository,
                mockGateway.Object,
                commitFactory,                
                NullLogger<CommitService>.Instance);

            return commitService;
        }
    }
}
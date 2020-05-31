using Microsoft.Extensions.Logging.Abstractions;
using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Services;
using Monbsoft.EvolDB.Tests.Infrastructure;
using Monbsoft.Extensions.FileProviders;
using Moq;
using Xunit;

namespace Monbsoft.EvolDB.Tests
{
    public class CommitBuilderTests
    {
        [Fact]
        public void Build_With_Versioned()
        {
            var testFile = new TestFileInfo
            {
                Name = "V1_0_0_0__init.n1ql"
            };
            var mockHashService = new Mock<IHashService>();
            mockHashService.Setup(hs => hs.ComputeHash(It.IsAny<IFileInfo>())).Returns("hash");
            var builder = new CommitBuilder(new ReferenceParser(), mockHashService.Object, NullLogger<CommitBuilder>.Instance);

            var commit = builder.Build(testFile);

            Assert.Equal("init", commit.Message);
            Assert.Equal(Prefix.Versioned, commit.Prefix);
            Assert.Equal(new CommitVersion(1, 0, 0, 0), commit.Version);
        }
    }
}
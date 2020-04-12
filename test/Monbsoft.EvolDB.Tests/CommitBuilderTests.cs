using Microsoft.Extensions.Logging.Abstractions;
using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Services;
using Monbsoft.Extensions.FileProviders;
using Moq;
using System.IO;
using Xunit;

namespace Monbsoft.EvolDB.Tests
{
    public class CommitBuilderTests
    {
        [Fact]
        public void Build_With_Versioned()
        {
            var mockFile = new Mock<IFileInfo>();
            mockFile.SetupGet(f => f.Name).Returns("V1_0_0_0__init.n1ql");
            mockFile.SetupGet(f => f.Exists).Returns(true);
            var mockHashService = new Mock<IHashService>();
            mockHashService.Setup(hs => hs.ComputeHash(It.IsAny<IFileInfo>())).Returns("hash");
            var builder = new CommitBuilder(new MigrationParser(), mockHashService.Object, NullLogger<CommitBuilder>.Instance);

            var commit = builder.Build(mockFile.Object);

            Assert.Equal("init", commit.Message);
            Assert.Equal(Prefix.Versioned, commit.Prefix);
            Assert.Equal(new CommitVersion(1, 0, 0, 0), commit.Version);
        }
    }
}
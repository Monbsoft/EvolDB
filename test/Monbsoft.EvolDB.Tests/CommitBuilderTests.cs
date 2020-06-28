using Microsoft.Extensions.Logging.Abstractions;
using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Exceptions;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Services;
using Monbsoft.EvolDB.Tests.Infrastructure;
using Monbsoft.Extensions.FileProviders;
using Moq;
using System;
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
                Name = "V1_0_0_0__init.n1ql",
                Exists = true,
            };
            var mockHashService = new Mock<IHashService>();
            mockHashService.Setup(hs => hs.ComputeHash(It.IsAny<IFileInfo>())).Returns("hash");
            var builder = new CommitBuilder(new ReferenceParser(), mockHashService.Object, NullLogger<CommitBuilder>.Instance);

            var commit = builder.Build(testFile);

            Assert.Equal("init", commit.Message);
            Assert.Equal(Prefix.Versioned, commit.Prefix);
            Assert.Equal(new CommitVersion(1, 0, 0, 0), commit.Version);
        }

        [Fact]
        public void Build_without_file()
        {
            var testFile = new TestFileInfo
            {
                Name = "V1_0_0_0__init.n1ql",
                Exists = false,
            };
            var mockHashService = new Mock<IHashService>();
            mockHashService.Setup(hs => hs.ComputeHash(It.IsAny<IFileInfo>())).Returns("hash");
            var builder = new CommitBuilder(new ReferenceParser(), mockHashService.Object, NullLogger<CommitBuilder>.Instance);

            Assert.Throws<ArgumentNullException>(() => builder.Build(testFile));
        }

        [Fact]
        public void Build_with_bad_file()
        {
            var testFile = new TestFileInfo
            {
                Name = "A1_0_0_0__init.n1ql",
                Exists = true,
            };
            var mockHashService = new Mock<IHashService>();
            mockHashService.Setup(hs => hs.ComputeHash(It.IsAny<IFileInfo>())).Returns("hash");
            var builder = new CommitBuilder(new ReferenceParser(), mockHashService.Object, NullLogger<CommitBuilder>.Instance);

            Assert.Throws<CommitException>(() => builder.Build(testFile));
        }
    }
}
using Microsoft.Extensions.Configuration;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Repositories;
using Monbsoft.EvolDB.Tests.Infrastructure;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using Xunit;

namespace Monbsoft.EvolDB.Tests
{
    public class RepositoryTreeTests
    {
        [Fact]
        public void Get_Entries()
        {
            var tree = new RepositoryTree(GetFakeRepository(), new List<CommitMetadata>());

            var first = tree.First;
            var second = first.Next;

            Assert.Equal("1_0_0_0", first.Value.Version);
            Assert.Equal("1_0_0_1", second.Value.Version);
        }

        private Repository GetFakeRepository()
        {
            var testFolder = new TestDirectoryInfo
            {
                Exists = true
            }
            .WithDirectory(
                new TestDirectoryInfo()
                {
                    Name = "commits",
                    Exists = true
                });

            var mockConfig = new Mock<IConfigurationRoot>();
            var repository = new Repository(testFolder, mockConfig.Object);
            repository.Commits.Add(new Commit { Version = new CommitVersion(1, 0, 0, 1), Prefix = Prefix.Versioned });
            repository.Commits.Add(new Commit { Version = new CommitVersion(1, 0, 0, 0), Prefix = Prefix.Versioned });



            return repository;
        }
    }
}
using Microsoft.Extensions.Configuration;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Repositories;
using Monbsoft.EvolDB.Tests.Infrastructure;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Monbsoft.EvolDB.Tests
{
    public class RepositoryTreeTests
    {
        [Fact]
        public void Get_Current_Commit()
        {
            var tree = new RepositoryTree(GetFakeRepository(), GetFakeMetadata());

            var current = tree.CurrentEntry;

            Assert.Equal("1_0_0_1", current.Version);
        }
        [Fact]
        public void Get_Entries()
        {
            var tree = new RepositoryTree(GetFakeRepository(), new List<CommitMetadata>());

            var first = tree.FirstNode;
            var second = first.Next;

            Assert.Equal("1_0_0_0", first.Value.Version);
            Assert.Equal("1_0_0_1", second.Value.Version);
        }

        [Fact]
        public void Get_first_commit()
        {
            var tree = new RepositoryTree(GetFakeRepository(), GetFakeMetadata());

            var last = tree.FirstNode;

            Assert.Equal("1_0_0_0", last.Value.Version);
        }
        [Fact]
        public void Get_last_commit()
        {
            var tree = new RepositoryTree(GetFakeRepository(), GetFakeMetadata());

            var last = tree.LastNode;

            Assert.Equal("1_0_0_2", last.Value.Version);
        }
        [Fact]
        public void Get_Repeatable_Commit()
        {
            var tree = new RepositoryTree(GetFakeRepository(), new List<CommitMetadata>());
            var secondEntry = tree.FirstNode.Next.Value;

            var repeatable = secondEntry.Repeatable;

            Assert.Equal("1_0_0_1", secondEntry.Version);
            Assert.Equal(new CommitVersion(1, 0, 0, 1), repeatable.Version);
        }
        private List<CommitMetadata> GetFakeMetadata()
        {
            return new List<CommitMetadata>()
            {
                new CommitMetadata { Applied = true, Version = "1_0_0_0", Prefix = Prefix.Versioned.ToString() },
                new CommitMetadata { Applied = true, Version = "1_0_0_1", Prefix = Prefix.Versioned.ToString()},
            };
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
            repository.Commits.Add(new Commit { Version = new CommitVersion(1, 0, 0, 2), Prefix = Prefix.Versioned });
            repository.Commits.Add(new Commit { Version = new CommitVersion(1, 0, 0, 1), Prefix = Prefix.Versioned });
            repository.Commits.Add(new Commit { Version = new CommitVersion(1, 0, 0, 1), Prefix = Prefix.Repeatable });
            repository.Commits.Add(new Commit { Version = new CommitVersion(1, 0, 0, 0), Prefix = Prefix.Versioned });
            return repository;
        }
    }
}
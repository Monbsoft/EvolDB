using Microsoft.Extensions.Configuration;
using Monbsoft.EvolDB.Models;
using Monbsoft.EvolDB.Tests.Infrastructure;
using Moq;
using System.IO;
using Xunit;

namespace Monbsoft.EvolDB.Tests
{
    public class RepositoryTests
    {
        [Fact]
        public void Get_repository_with_no_root_folder()
        {
            var testFolder = new TestDirectoryInfo
            {
                Exists = false
            };

            Assert.Throws<DirectoryNotFoundException>(() => new Repository(testFolder, null));
        }

        [Fact]
        public void Get_repository()
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
            mockConfig.Setup(config => config[It.IsAny<string>()]).Returns("COUCHBASE_TYPE");

            var repository = new Repository(testFolder, mockConfig.Object);

            Assert.NotNull(repository);
        }
    }
}
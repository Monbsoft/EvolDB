using Xunit;

namespace Monbsoft.EvolDB.Tests
{
    public class CommitVersionTests
    {        
        [Theory]
        [InlineData("1_0_0_0", "1_0_0_0", 0)]
        [InlineData("1_2_0_0", "1_1_0_0", 1)]
        [InlineData("1_0_0_1", "1_1_0_0", -1)]
        [InlineData("1_2_0_0", "1_2_0_1", -1)]
        [InlineData("1_4_0_1", "1_4_0_0", 1)]
        public void CommitVersionCompare(string ver1, string ver2, int result)
        {
            Assert.True(CommitVersion.TryParse(ver1, out CommitVersion v1));
            Assert.True(CommitVersion.TryParse(ver2, out CommitVersion v2));

            Assert.Equal(v1.CompareTo(v2), result);
        }

        [Theory]
        [InlineData("1.0.1", 1, 0, 1, 0)]
        [InlineData("2_0_12_1", 2, 0, 12, 1)]
        public void ParseVersion(string parsedVersion, int major, int minor, int patch, int revision)
        {
            var version = CommitVersion.Parse(parsedVersion);

            Assert.Equal(major, version.Major);
            Assert.Equal(minor, version.Minor);
            Assert.Equal(patch, version.Patch);
            Assert.Equal(revision, version.Revision);
        }
        [Fact]
        public void ParseFake()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() =>
                CommitVersion.Parse("1__2"));
        }
    }
}
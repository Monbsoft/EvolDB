using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Monbsoft.EvolDB.Tests
{
    public class CommitVersionTests
    {
        [Theory]
        [InlineData("1_0_0_0", "1_0_0_0",0 )]
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
    }
}

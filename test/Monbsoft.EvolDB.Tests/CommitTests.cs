using Monbsoft.EvolDB.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Monbsoft.EvolDB.Tests
{
    public class CommitTests
    {
        [Fact]
        public void Reference_with_versioned()
        {
            var commit = new Commit
            {
                Prefix = Prefix.Versioned,
                Message = "init",
                Version = new CommitVersion(1, 0, 0, 0),
                Extension = "n1ql"
            };

            Assert.Equal("V1_0_0_0__init.n1ql", commit.ToReference());
        }

        [Fact]
        public void Reference_with_repeatable()
        {
            var commit = new Commit
            {
                Prefix = Prefix.Repeatable,
                Message = "init",
                Version = new CommitVersion(1, 5, 0, 0),
                Extension = "sql"
            };

            Assert.Equal("R1_5_0_0__init.sql", commit.ToReference());
        }
    }
}

using Monbsoft.EvolDB.Commit;
using Monbsoft.EvolDB.Models;
using Xunit;

namespace Monbsoft.EvolDB.Tests
{
    public class MigrationParserTests
    {

        [Theory]
        [InlineData("V1_1_0_1__test_c_t.n1ql", Migration.Versioned, "1_1_0_1", "test c t")]
        [InlineData("V12_1_2__initial.n1ql", Migration.Versioned, "12_1_2_0", "initial")]
        [InlineData("R12_1_14_0__initial.n1ql", Migration.Repeatable, "12_1_14_0", "initial")]
        [InlineData("V2.3.5__test.n1ql", Migration.Versioned, "2_3_5_0", "test")]
        public void ParseMigration(string migration, Migration type, string version, string message)
        {
            var parser = new MigrationParser();

            var token = parser.Parse(migration);

            Assert.Equal(type, token.Migration);
            Assert.Equal(version, token.Version.ToString());
            Assert.Equal(message, token.Message);
        }


        [Theory]
        [InlineData("V1_8_21__initial.sql")]
        [InlineData("V1_0_0_1_initial.n1ql")]
        [InlineData("V1_0_0_2__.n1ql")]
        public void ParseFakeMigration(string migration)
        {
            var parser = new MigrationParser();

            var commit = parser.Parse(migration);

            Assert.Null(commit);
        }
    }
}

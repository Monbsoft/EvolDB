using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Exceptions;
using Monbsoft.EvolDB.Models;
using Xunit;

namespace Monbsoft.EvolDB.Tests
{
    public class MigrationParserTests
    {

        [Theory]
        [InlineData("V1_1_0_1__test_c_t.n1ql", Prefix.Versioned, "1_1_0_1", "test c t")]
        [InlineData("V12_1_2__initial.n1ql", Prefix.Versioned, "12_1_2_0", "initial")]
        [InlineData("R12_1_14_0__initial.n1ql", Prefix.Repeatable, "12_1_14_0", "initial")]
        [InlineData("V2.3.5__test.n1ql", Prefix.Versioned, "2_3_5_0", "test")]
        public void TryParseMigration(string migration, Prefix type, string version, string message)
        {
            var parser = CreateTestParser();

            var commit = parser.CreateCommit(migration);

            Assert.Equal(type, commit.Prefix);
            Assert.Equal(version, commit.Version.ToString());
            Assert.Equal(message, commit.Message);
        }


        [Theory]
        [InlineData("V1_8_21__initial.sql")]
        [InlineData("V1_0_0_1_initial.n1ql")]
        [InlineData("V1_0_0_2__.n1ql")]
        public void ParseFakeMigration(string migration)
        {
            var parser = CreateTestParser();

            Assert.Throws<CommitException>(() => parser.CreateCommit(migration));
        }

        private static IReferenceParser CreateTestParser()
        {
            return new ReferenceParser("n1ql");
        }
    }
}

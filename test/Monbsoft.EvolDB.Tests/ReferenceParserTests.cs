using Monbsoft.EvolDB.Commits;
using Monbsoft.EvolDB.Models;
using Sprache;
using Xunit;

namespace Monbsoft.EvolDB.Tests
{
    public class ReferenceParserTests
    {

        [Fact]
        public void Constructor_extension()
        {
            var parser = new ReferenceParser(".n1ql");

            Assert.NotNull(parser);
        }

        [Fact]
        public void Constructor_Fake_Extension()
        {
            Assert.Throws<ParseException>(() => new ReferenceParser("n1ql"));
        }

        [Theory]
        [InlineData("V1_1_0_1__test_c_t.n1ql", Prefix.Versioned, "1_1_0_1", "test c t", ".n1ql")]
        [InlineData("V12_1_2__initial.n1ql", Prefix.Versioned, "12_1_2_0", "initial", ".n1ql")]
        [InlineData("R12_1_14_0__initial.n1ql", Prefix.Repeatable, "12_1_14_0", "initial", ".n1ql")]
        [InlineData("V2.3.5__test.n1ql", Prefix.Versioned, "2_3_5_0", "test", ".n1ql")]
        public void TryParseMigration(string migration, Prefix type, string version, string message, string extension)
        {
            var parser = CreateTestParser();

            var commit = parser.CreateCommit(migration);

            Assert.Equal(type, commit.Prefix);
            Assert.Equal(version, commit.Version.ToString());
            Assert.Equal(message, commit.Message);
            Assert.Equal(extension, commit.Extension);
        }


        [Theory]
        [InlineData("V_1_0_0_0__initial.sql")]
        [InlineData("V2_8__initial.n1ql")]
        [InlineData("V4_21_2_0_1__initial.n1ql")]
        [InlineData("V5_0_0_1_initial.n1ql")]
        [InlineData("V6_0_0_2__.n1ql")]
        [InlineData("V7_4_0_0__test.")]
        public void ParseFakeMigration(string migration)
        {
            var parser = CreateTestParser();

            Assert.Throws<ParseException>(() => parser.CreateCommit(migration));
        }

        private static IReferenceParser CreateTestParser()
        {
            return new ReferenceParser(".n1ql");
        }
    }
}

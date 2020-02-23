using Monbsoft.EvolDB.Commit;
using Xunit;

namespace Monbsoft.EvolDB.Tests
{
    public class MigrationParserTests
    {

        [Fact]
        public void ParseTest()
        {
            var parser = new MigrationParser();

            parser.Parse("V1_1_0_1__test_c_t.n1ql");

            Assert.True(true);
        }
    }
}

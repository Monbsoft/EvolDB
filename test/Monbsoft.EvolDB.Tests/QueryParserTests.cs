using Monbsoft.EvolDB.Data;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Monbsoft.EvolDB.Tests
{
    public class QueryParserTests
    {
        [Fact]
        public void Parse_with_line()
        {
            string query = "SELECT * FROM data WHERE test  = 12;";
            var lines = new List<string>
            {
                query
            };
            var parser = new DefaultQueryParser();

            var tokens = parser.Parse(lines);
            var token = tokens.First();

            Assert.Single(tokens);
            Assert.Equal(1, token.Begin);
            Assert.Equal(1, token.End);
            Assert.Equal(query, token.Text);
        }
        [Fact]
        public void Parse_with_multiines()
        {
            string query = @"SELECT *
FROM data
WHERE test = 12;";
            var parser = new DefaultQueryParser();
            var lines = new List<string>
            {
                "SELECT *",
                "FROM data",
                "WHERE test = 12;"
            };

            var tokens = parser.Parse(lines);
            var token = tokens.First();

            Assert.Single(tokens);
            Assert.Equal(1, token.Begin);
            Assert.Equal(3, token.End);
            Assert.Equal(query, token.Text);
        }
        [Fact]
        public void Parse_without_delimeter()
        {
            var lines = new List<string>
            {
                "SELECT * FROM data WHERE test"
            };
            var parser = new DefaultQueryParser();

            var tokens = parser.Parse(lines);

            Assert.Empty(tokens);
        }

        [Fact]
        public void Parse_without_line()
        {
            var parser = new DefaultQueryParser();
            var lines = new List<string>();

            var tokens = parser.Parse(lines);

            Assert.Empty(tokens);            
        }
    }
}
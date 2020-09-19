using Monbsoft.EvolDB.Data;
using Monbsoft.EvolDB.Tests.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Monbsoft.EvolDB.Tests
{
    public class StatementParserTests
    {
        [Fact]
        public void Parse_with_line()
        {
            string content = "SELECT * FROM data WHERE test  = 12;";

            var parser = new StatementParser();

            var statements = parser.ParseQueries(content);
            var statement = statements.First();

            Assert.Single(statements);
            Assert.Equal("SELECT * FROM data WHERE test  = 12;", statement);
        }
        [Fact]
        public void Parse_with_multiines()
        {
            string content = @"SELECT *
FROM data
WHERE test = 12;";
            var parser = new StatementParser();
            var lines = new List<string>
            {
                "SELECT *",
                "FROM data",
                "WHERE test = 12;"
            };

            var statements = parser.ParseQueries(content);
            var statement = statements.First();

            Assert.Single(statements);
            Assert.Equal(content, statement);
        }
        [Fact]
        public void Parse_without_delimeter()
        {
            string content = "SELECT * FROM data WHERE test";
            var parser = new StatementParser();

            var statements = parser.ParseQueries(content);

           Assert.Empty(statements);
        }

        [Fact]
        public void Parse_with_several_statements()
        {
            string content = @"SELECT * FROM data
WHERE name = Test;

SELECT TOP(10) Name
FROM authors;";
            var parser = new StatementParser();

            var statements = parser.ParseQueries(content);

            Assert.Equal(2, statements.Count());
            Assert.Equal("SELECT * FROM data WHERE name = Test;", StatementHelper.Transform(statements.ElementAt(0)));
            Assert.Equal("SELECT TOP(10) Name FROM authors;", StatementHelper.Transform(statements.ElementAt(1)));
        }

        [Fact]
        public void Parse_with_bad_statement()
        {
            string content = @"SELECT Name
FROM authors;

SELECT TOP(10) * FROM authors";
            var parser = new StatementParser();

            var statements = parser.ParseQueries(content);

            Assert.Single(statements);
            Assert.Equal("SELECT Name FROM authors;", StatementHelper.Transform(statements.First()));
        }
    }
}
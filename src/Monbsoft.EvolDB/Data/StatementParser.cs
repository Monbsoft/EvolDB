using Monbsoft.EvolDB.Models;
using Sprache;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.EvolDB.Data
{
    public class StatementParser : IStatementParser
    {
        static readonly Parser<char> Delimeter = Parse.Char(';');

        static readonly Parser<string> Statement =
             from content in Parse.AnyChar.Except(Delimeter).Many().Text()
             from delimeter in Delimeter
             select content.Trim() + delimeter;

        static readonly Parser<IEnumerable<string>> Statements =
            from statement in Statement.Many()
            select statement;

        public IEnumerable<string> Evaluate(string content)
        {
            return Statements.Parse(content);
        }
    }
}

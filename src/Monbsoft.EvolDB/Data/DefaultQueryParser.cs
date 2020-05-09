using Monbsoft.EvolDB.Models;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.EvolDB.Data
{
    public class DefaultQueryParser : IQueryParser
    {
        private const string Delimiter = ";";
        public List<QueryToken> Parse(List<string> lines)
        {
            int start = 0;
            StringBuilder sb = new StringBuilder();
            List<QueryToken> tokens = new List<QueryToken>();

            for (int i = 0; i < lines.Count; i++)
            {
                sb.Append(lines[i]);
                if (lines[i].EndsWith(Delimiter))
                {
                    var token = new QueryToken(start + 1, i + 1, sb.ToString());
                    tokens.Add(token);
                    start = i;
                }
            }
            return tokens;
        }
    }
}

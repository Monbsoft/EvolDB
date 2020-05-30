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
            bool first = true;

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                if(string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if(!first)
                {
                    sb.AppendLine();
                }
                else
                {
                    first = false;
                }

                sb.Append(line);
                if (lines[i].EndsWith(Delimiter))
                {
                    var token = new QueryToken(start + 1, i + 1, sb.ToString());
                    tokens.Add(token);
                    sb.Clear();
                    start = i;
                }
            }
            return tokens;
        }
    }
}

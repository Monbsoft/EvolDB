using System;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.EvolDB.Models
{
    public sealed class QueryToken
    {
        public QueryToken(int begin, int end, string text)
        {
            Begin = begin;
            End = end;
            Text = text;
        }

        public int Begin { get; }

        public int End { get; }

        public string Text { get; }
    }
}

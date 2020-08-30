using System;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.EvolDB.Tests.Infrastructure
{
    public static class StatementHelper
    {
        public static string Transform(string statement)
        {
            return statement.Replace(Environment.NewLine, " ");
        }
    }
}

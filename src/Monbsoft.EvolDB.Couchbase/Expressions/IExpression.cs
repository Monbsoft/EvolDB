using System;
using System.Collections.Generic;
using System.Text;

namespace Monbsoft.EvolDB.Couchbase.Expressions
{
    public interface IExpression
    {
        string Build();
    }
}

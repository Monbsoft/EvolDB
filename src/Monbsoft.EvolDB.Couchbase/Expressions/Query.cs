using Monbsoft.EvolDB.Couchbase.Expressions;
using System;
using System.Linq;
using System.Text;

namespace Monbsoft.EvolDB.Couchbase
{
    public class Query : IExpression
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public Query Insert(string bucket)
        {
            _sb.AppendLine($"INSERT INTO {WithBucket(bucket)} (KEY, VALUE)");
            return this;
        }

        public Query Values(string key, Func<Value,Value> buildValue)
        {
            _sb.AppendLine($"VALUES({WithExpression(key)}, {buildValue(new Value()).Build()} )");
            return this;
        }

        public Query Select(string fields)
        {
            _sb.AppendLine($"SELECT {fields}");
            return this;
        }

        public Query From(string bucket)
        {
            _sb.AppendLine($"FROM {WithBucket(bucket)}");
            return this;
        }
        
        public string Build()
        {
            return _sb.ToString();
        }

        private string WithExpression(string exp)
        {
            return $"\"{exp}\"";
        }

        private string WithBucket(string bucket)
        {
            return $"`{bucket}`";
        }
    }
}
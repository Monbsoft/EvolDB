using System;
using System.Text;

namespace Monbsoft.EvolDB.Couchbase
{
    public class CouchbaseQuery
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public CouchbaseQuery Insert(string bucket)
        {
            _sb.AppendLine($"INSERT INTO {WithBucket(bucket)} (KEY, VALUE)");
            return this;
        }

        public CouchbaseQuery Values(string key, Func<string> onValues)
        {
            _sb.AppendLine($"VALUES({WithExpression(key)}, {onValues()} )");
            return this;
        }

        public CouchbaseQuery Select(string fields)
        {
            _sb.AppendLine($"SELECT {fields}");
            return this;
        }

        public CouchbaseQuery From(string bucket)
        {
            _sb.AppendLine($"FROM {WithBucket(bucket)}");
            return this;
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
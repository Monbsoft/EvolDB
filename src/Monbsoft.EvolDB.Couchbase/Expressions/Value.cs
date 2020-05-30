using System;
using System.Text;

namespace Monbsoft.EvolDB.Couchbase.Expressions
{
    public class Value : IExpression
    {
        private readonly StringBuilder _builder;
        private bool _first;

        public Value()
        {
            _first = true;
            _builder = new StringBuilder();
        }

        public string Build()
        {
            return $"{{ {_builder} }}";
        }

        public Value WithKeyValue<T>(string key, T value)
        {
            if (!_first)
            {
                _builder.Append(", ");
            }
            else
            {
                _first = false;
            }
            _builder.Append($"{WithValue(key)}: {WithValue(value)}");
            return this;
        }

        private string WithValue<T>(T value)
        {
            switch (value)
            {
                case string v:
                    return $"\"{v}\"";
                case DateTime v:
                    return $"\"{v.ToString("yyyy-MM-dd HH:mm:ss")}\"";

                default:
                    return $"{value}";
            }
        }
    }
}
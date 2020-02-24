using System;
using System.Text;

namespace Monbsoft.EvolDB.Models
{
    public class Commit
    {
        public string FullName { get; set; }
        public string Hash { get; set; }
        public string Message { get; set; }
        public Migration Migration { get; set; }
        public CommitVersion Version { get; set; }

        public string GetName()
        {
            StringBuilder sb = new StringBuilder();
            
            switch(Migration)
            {
                case Migration.Versioned:
                    {
                        sb.Append("V");
                        break;
                    }
                case Migration.Repeatable:
                    {
                        sb.Append("R");
                        break;
                    }
                default:
                    {
                        throw new InvalidCastException(nameof(Migration));
                    }
            }

            sb.Append(Version.ToString());
            sb.Append("__");
            sb.Append(Message.Replace(" ", "_"));
            sb.Append(".n1ql");
            return sb.ToString();
        }
    }
}
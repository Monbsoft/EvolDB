﻿using System;
using System.Text;

namespace Monbsoft.EvolDB.Models
{
    public class Commit
    {
        public string FullName { get; set; }
        public string Hash { get; set; }
        public string Message { get; set; }
        public Prefix Prefix { get; set; }
        public Repository Repository { get; internal set; }
        public CommitVersion Version { get; set; }

        public string ToName()
        {
            StringBuilder sb = new StringBuilder();
            
            switch(Prefix)
            {
                case Prefix.Versioned:
                    {
                        sb.Append("V");
                        break;
                    }
                case Prefix.Repeatable:
                    {
                        sb.Append("R");
                        break;
                    }
                default:
                    {
                        throw new InvalidCastException(nameof(Prefix));
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
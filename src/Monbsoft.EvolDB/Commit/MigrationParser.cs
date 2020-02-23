using Monbsoft.EvolDB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Monbsoft.EvolDB.Commit
{
    public class MigrationParser : IMigrationParser
    {
        private static Regex _versionRegex = new Regex(
            @"^(?<prefix>[V|R])(?<version>[0-9\\._]+)__(?<message>\w+)[\\.]{1}n1ql$");


        public Token Parse(string migration)
        {
            var match = _versionRegex.Match(migration);
            if(match == null || match.Groups.Count <4 || match.Groups.Count > 4)
            {
                return null;
            }

            return new Token
            {
                Migration = ParseMigration(match.Groups[""])
                Message = match.Groups["message"].Value,
                Version = match.Groups["version"].Value,
            };
        }

        private Migration ParsePrefix(string prefix)
        {
            switch(prefix)
            {
                case "V":
                    return Migration.Versioned;
                case "R":
                    return Migration.Repeatable;
                default:
                    throw new ArgumentException(
                        message: "Migration is not recognized.",
                        paramName: nameof(prefix));
            }
        }
    }
}

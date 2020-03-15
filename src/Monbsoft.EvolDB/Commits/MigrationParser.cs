using m = Monbsoft.EvolDB.Models;
using System;
using System.Text.RegularExpressions;
using Monbsoft.EvolDB.Excceptions;

namespace Monbsoft.EvolDB.Commits
{
    public class MigrationParser : IMigrationParser
    {
        private static Regex _migrationRegex = new Regex(
            @"^(?<prefix>[V|R])(?<version>[0-9\\._]+)__(?<message>\w+)[\\.]{1}n1ql$");

        public m.Commit Parse(string migration)
        {
            var match = Match(migration);

            return new m.Commit
            {
                Migration = ParsePrefix(match.Groups["prefix"].Value),
                Message = ParseMessage(match.Groups["message"].Value),
                Version = ParseVersion(match.Groups["version"].Value),
            };
        }

        public string ParseName(string migration)
        {
            var match = Match(migration);

            string prefix = match.Groups["prefix"].Value;
            var version = ParseVersion(match.Groups["version"].Value);
            string message = match.Groups["message"].Value;

            return $"{prefix}{version.ToString()}__{message}.n1ql";
        }

        private Match Match(string migration)
        {
            var match = _migrationRegex.Match(migration.Trim());
            if (match == null || match.Groups.Count < 4 || match.Groups.Count > 4)
            {
                throw new CommitException("Migration name is invalid.");
            }
            return match;
        }

        private string ParseMessage(string message)
        {
            return message.Replace('_', ' ');
        }

        private m.Migration ParsePrefix(string prefix)
        {
            switch (prefix)
            {
                case "V":
                    return m.Migration.Versioned;

                case "R":
                    return m.Migration.Repeatable;

                default:
                    throw new ArgumentException(
                        message: "Migration is not recognized.",
                        paramName: nameof(prefix));
            }
        }
        private CommitVersion ParseVersion(string version)
        {
            return CommitVersion.Parse(version);
        }
    }
}
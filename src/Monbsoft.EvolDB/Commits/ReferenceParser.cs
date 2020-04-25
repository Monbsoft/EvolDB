using System;
using System.Text.RegularExpressions;
using Monbsoft.EvolDB.Exceptions;
using Monbsoft.EvolDB.Models;

namespace Monbsoft.EvolDB.Commits
{
    public class ReferenceParser : IReferenceParser
    {
        private static Regex _referenceRegex = new Regex(
            @"^(?<prefix>[V|R])(?<version>[0-9\\._]+)__(?<message>\w+)[\\.]{1}n1ql$");

        /// <summary>
        /// Converts the commit reference. A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public bool TryParse(string reference, out Commit commit)
        {
            var match = Match(reference);

            if(!match.Success)
            {
                commit = null;
                return false;
            }
            
            commit = new Commit
            {
                Prefix = ParsePrefix(match.Groups["prefix"].Value),
                Message = ParseMessage(match.Groups["message"].Value),
                Version = ParseVersion(match.Groups["version"].Value),
            };
            return true;
        }

        private Match Match(string reference)
        {
            var match = _referenceRegex.Match(reference.Trim());
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

        private Prefix ParsePrefix(string prefix)
        {
            switch (prefix)
            {
                case "V":
                    return Prefix.Versioned;

                case "R":
                    return Prefix.Repeatable;

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
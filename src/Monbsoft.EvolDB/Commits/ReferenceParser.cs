using System;
using System.Text.RegularExpressions;
using Monbsoft.EvolDB.Exceptions;
using Monbsoft.EvolDB.Models;

namespace Monbsoft.EvolDB.Commits
{
    public class ReferenceParser : IReferenceParser
    {
        private readonly Regex _referenceRegex;

        public ReferenceParser(string extension)
        {
            string pattern = @"^(?<prefix>[V|R])(?<version>[0-9\\._]+)__(?<message>\w+)[\\.]{1}" + extension + "$";
            _referenceRegex = new Regex(pattern);
        }

        /// <summary>
        /// Converts the commit reference.
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public Commit CreateCommit(string reference)
        {
            var match = Match(reference);
            
            return new Commit
            {
                Prefix = ParsePrefix(match.Groups["prefix"].Value),
                Message = ParseMessage(match.Groups["message"].Value),
                Version = ParseVersion(match.Groups["version"].Value),
            };

        }

        private Match Match(string reference)
        {
            var match = _referenceRegex.Match(reference.Trim());
            if (match == null || !match.Success)
            {
                throw new CommitException("Commit reference is invalid.");
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
                        message: "Reference prefix is not recognized.",
                        paramName: nameof(prefix));
            }
        }
        private CommitVersion ParseVersion(string version)
        {
            return CommitVersion.Parse(version);
        }
    }
}
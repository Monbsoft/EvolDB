using Monbsoft.EvolDB.Exceptions;
using Monbsoft.EvolDB.Models;
using Sprache;
using System;
using System.CommandLine.Parsing;
using System.Linq;

namespace Monbsoft.EvolDB.Commits
{
    public class ReferenceParser : IReferenceParser
    {
        // prefix
        private static readonly Parser<char> VersionedPrefix = Parse.Char('V');

        private static readonly Parser<char> RepeatablePrefix = Parse.Char('R');

        private static readonly Parser<Prefix> PrefixToken =
            Parse.Or(VersionedPrefix, RepeatablePrefix).Select(p => ParsePrefix(p));

        // version
        private static readonly Parser<int> NumberToken =
            Parse.Number.Select(n => int.Parse(n));

        private static readonly Parser<int[]> VersionNumberToken =
            from numbers in NumberToken.DelimitedBy(Parse.Or(Parse.Char('_'), Parse.Char('.')), 3, 4)
            select numbers.ToArray();

        private static readonly Parser<CommitVersion> VersionToken =
            from numbers in VersionNumberToken
            select new CommitVersion(numbers);

        // message
        private static readonly Parser<string> MessageToken =
            from delimeter in Parse.Char('_').Repeat(2)
            from message in Parse.Or(Parse.LetterOrDigit, Parse.Char('_')).AtLeastOnce().Text()
            select message.Replace('_', ' ');

        // extension
        private static readonly Parser<char> ExtensionDelimeter = Parse.Char('.');

        private static readonly Parser<string> ExtensionToken =
            from delimeter in ExtensionDelimeter
            from extension in Parse.LetterOrDigit.AtLeastOnce().Text()
            select string.Concat(delimeter, extension);

        // commit
        private static readonly Parser<Commit> Commit =
            from prefix in PrefixToken
            from version in VersionToken
            from message in MessageToken
            from extension in ExtensionToken
            select new Commit { Prefix = prefix, Version = version, Message = message, Extension = extension };

        private readonly string _extension;

        /// <summary>
        ///
        /// </summary>
        /// <param name="extension"></param>
        /// <exception cref="Sprache.ParseException" />
        public ReferenceParser(string extension)
        {
            _extension = ExtensionToken.Parse(extension);
        }

        /// <summary>
        /// Converts the commit reference.
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public Commit CreateCommit(string reference)
        {
            Commit commit;
            try
            {
                commit = Commit.Parse(reference);               
            }
            catch(ParseException pe)
            {
                throw new CommitException("Commit reference is invalid.", pe);
            }
            if (!_extension.Equals(commit.Extension))
            {
                throw new CommitException("Unknown extension.");
            }
            return commit;
        }

        private static Prefix ParsePrefix(char prefixToken)
        {
            switch (prefixToken)
            {
                case 'V':
                    return Prefix.Versioned;

                case 'R':
                    return Prefix.Repeatable;

                default:
                    throw new CommitException("Unknown prefix.");
            }
        }
    }
}
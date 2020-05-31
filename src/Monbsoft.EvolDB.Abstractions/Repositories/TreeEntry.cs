using Monbsoft.EvolDB.Models;
using System;

namespace Monbsoft.EvolDB.Repositories
{
    public class TreeEntry
    {
        public TreeEntry(string version)
        {
            if (string.IsNullOrEmpty(version))
            {
                throw new ArgumentNullException(nameof(Version));
            }
            Version = version;
        }

        public Commit Repeatable { get; set; }
        public Commit Source { get; set; }
        public CommitMetadata Target { get; set; }
        public string Version { get; set; }

        public void AddCommit(Commit commit)
        {
            if (Version != commit.Version.ToString())
            {
                throw new InvalidOperationException("Version is not the same.");
            }
            switch (commit.Prefix)
            {
                case Prefix.Versioned:
                    {
                        Source = commit;
                        break;
                    }
                case Prefix.Repeatable:
                    {
                        Repeatable = commit;
                        break;
                    }
                default:
                    {
                        throw new ArgumentException(nameof(commit));
                    }
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as TreeEntry);
        }

        public bool Equals(TreeEntry other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(Version, other.Version);
        }

        public override int GetHashCode()
        {
            return Version.GetHashCode();
        }
    }
}
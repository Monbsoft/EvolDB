using Monbsoft.EvolDB.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Monbsoft.EvolDB.Repositories
{
    public class RepositoryTree : IEnumerable<TreeEntry>
    {
        private readonly LinkedList<TreeEntry> _entries;
        public RepositoryTree(Repository repository, List<CommitMetadata> metadata)
        {
            _entries = new LinkedList<TreeEntry>();

            LoadRepository(repository);
            LoadMetadata(metadata);
        }

        public LinkedListNode<TreeEntry> Current { get; private set; }
        public LinkedListNode<TreeEntry> First => _entries.First;
        public LinkedListNode<TreeEntry> Last => _entries.Last;
        public IEnumerator<TreeEntry> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        private void LoadMetadata(List<CommitMetadata> metadata)
        {
            var map = _entries.ToDictionary(e => e.Version, e => e);
            var orderedMetadata = metadata.OrderBy(m => m.CreationDate);

            Current = First;

            foreach (var orderedMeta in orderedMetadata)
            {
                if (!orderedMeta.Applied)
                {
                    continue;
                }

                if (Current.Value.Version == orderedMeta.Version)
                {
                    if (orderedMeta.Prefix == nameof(Prefix.Versioned))
                    {
                        Current = Current.Next;
                    }
                    else if (orderedMeta.Prefix == nameof(Prefix.Repeatable))
                    {
                        Current = Current.Previous;
                    }
                }
            }
        }

        public IEnumerable<Commit> GetCommitsToApplied()
        {
            var current = Current;
            while(current != null)
            {                
                yield return current.Value.Source;
                current = current.Next;
            }
        }

        private void LoadRepository(Repository repository)
        {
            var map = new Dictionary<CommitVersion, TreeEntry>();
            var orderedCommits = repository.Commits.OrderBy(c => c.Version);

            foreach (var commit in orderedCommits)
            {
                if (!map.TryGetValue(commit.Version, out TreeEntry entry))
                {
                    entry = new TreeEntry(commit.Version.ToString());
                    _entries.AddLast(entry);
                    map.Add(commit.Version, entry);
                }
                entry.AddCommit(commit);
            }
        }
    }
}
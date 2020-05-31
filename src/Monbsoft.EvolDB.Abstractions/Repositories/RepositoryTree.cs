using Monbsoft.EvolDB.Models;
using System.Collections.Generic;
using System.Linq;

namespace Monbsoft.EvolDB.Repositories
{
    public class RepositoryTree
    {
        private readonly LinkedList<TreeEntry> _entries;
        public RepositoryTree(Repository repository, List<CommitMetadata> metadata)
        {
            _entries = new LinkedList<TreeEntry>();

            LoadRepository(repository);
            LoadMetadata(metadata);
        }
        public LinkedListNode<TreeEntry> First => _entries.First;
        public TreeEntry CurrentEntry => CurrentNode?.Value;
        public LinkedListNode<TreeEntry> CurrentNode { get; set; }
        public LinkedListNode<TreeEntry> Last => _entries.Last;
        public IEnumerable<Commit> GetCommitsToPush()
        {
            return _entries
                .Where(e => e.Target == null)
                .OrderBy(e => e.Version)
                .Select(e => e.Source);
        }

        private void LoadMetadata(List<CommitMetadata> metadata)
        {
            LinkedListNode<TreeEntry> current = _entries.First;
            if (current == null)
            {
                CurrentNode = null;
                return;
            }

            var orderedMetadata = metadata.OrderBy(m => m.CreationDate);
            foreach (var orderedMeta in orderedMetadata)
            {
                if (!orderedMeta.Applied)
                {
                    continue;
                }

                if (current.Value.Version == orderedMeta.Version)
                {
                    if (orderedMeta.Prefix == nameof(Prefix.Versioned))
                    {
                        current.Value.Target = orderedMeta;
                        CurrentNode = current;
                        current = current.Next;
                    }
                    else if (orderedMeta.Prefix == nameof(Prefix.Repeatable))
                    {
                        current.Value.Target = null;
                        current = current.Previous;
                        CurrentNode = current;
                    }
                }
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
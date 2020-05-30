using Monbsoft.EvolDB.Models;
using System.Collections.ObjectModel;

namespace Monbsoft.EvolDB.Collections
{
    public class CommitCollection : Collection<Commit>
    {
        private readonly Repository _repository;

        public CommitCollection(Repository repository)
        {
            _repository = repository;
        }

        protected override void ClearItems()
        {
            foreach (Commit commit in this)
            {
                commit.Repository = null;
            }
            base.ClearItems();
        }
        protected override void InsertItem(int index, Commit item)
        {
            base.InsertItem(index, item);
            item.Repository = _repository;
        }
        protected override void RemoveItem(int index)
        {
            this[index].Repository = null;
            base.RemoveItem(index);
        }
        protected override void SetItem(int index, Commit item)
        {
            base.SetItem(index, item);
            item.Repository = _repository;
        }
    }
}
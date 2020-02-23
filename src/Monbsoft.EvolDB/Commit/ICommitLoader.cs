﻿using Monbsoft.EvolDB.Models;

namespace Monbsoft.EvolDB.Commit
{
    public interface ICommitLoader
    {
        void Load(IRepository workspace);
    }
}
﻿using Monbsoft.EvolDB.Models;
using System.Threading.Tasks;

namespace Monbsoft.EvolDB.Services
{
    public interface ICommitService
    {
        void Create(string migration);

        void Execute(Commit commit);

        Task Push();
    }
}
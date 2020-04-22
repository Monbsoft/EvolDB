using Microsoft.Extensions.Configuration;
using Monbsoft.EvolDB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monbsoft.EvolDB.Data
{
    public interface IDatabaseGateway : IDisposable
    {
        Task OpenAsync();

        Task<List<Commit>> GetCommitsAsync();
    }
}
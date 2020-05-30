using Monbsoft.EvolDB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monbsoft.EvolDB.Data
{
    public interface IDatabaseGateway : IDisposable
    {
        IQueryParser Parser { get; }
        Task AddMetadataAsync(CommitMetadata meta);
        Task<List<CommitMetadata>> GetMetadataAsync();
        Task OpenAsync();
        Task PushAsync(QueryToken query);
    }
}
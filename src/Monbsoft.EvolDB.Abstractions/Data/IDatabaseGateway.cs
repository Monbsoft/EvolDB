using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Monbsoft.EvolDB.Data
{
    public interface IDatabaseGateway : IDisposable
    {
        Task OpenAsync(IConfigurationRoot configuration);
    }
}
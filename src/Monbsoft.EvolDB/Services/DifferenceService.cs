using Monbsoft.EvolDB.Models;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;

namespace Monbsoft.EvolDB.Services
{
    public class DifferenceService : IDifferenceService
    {
        private readonly Repository _repository;

        public DifferenceService(Repository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public List<DiffResult> Compare(List<CommitMetadata> metadata)
        {
            List<DiffResult> resuls = new List<DiffResult>();

            foreach (var commit in _repository.Commits)
            {
                var result = new DiffResult
                {
                    Reference = commit.ToReference(),
                    Hash = commit.Hash,
                    IsSource = true
                };


                var commitMeta = metadata.FirstOrDefault(cm => cm.Hash == commit.Hash);
                result.IsTarget = commitMeta != null;
                if (metadata != null)
                {
                    metadata.Remove(commitMeta);
                }

                resuls.Add(result);
            }

            return resuls;
        }
    }
}

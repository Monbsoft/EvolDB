using System;

namespace Monbsoft.EvolDB.Models
{
    public class CommitMetadata
    {
        public string Prefix { get; set; }

        public string Version { get; set; }

        public string Message { get; set; }

        public string Hash { get; set; }

        public bool Applied { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
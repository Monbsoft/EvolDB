using Microsoft.Extensions.Configuration;
using Monbsoft.EvolDB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Monbsoft.EvolDB.Data
{
    public class CommitRepository : IRepository
    {
        #region Champs
        public const string Commit_Folder = "commits";
        public const string ConfigFile = "config.json";
        private readonly DirectoryInfo _directory;
        private IConfigurationRoot _configuration;
        #endregion

        #region Constructeurs
        public CommitRepository(DirectoryInfo folder)
            : this(folder, null)
        {
        }

        public CommitRepository(DirectoryInfo folder, IConfigurationRoot configuration)
        {
            _directory = folder ?? throw new ArgumentNullException(nameof(folder));

            if (!_directory.Exists)
            {
                throw new DirectoryNotFoundException();
            }
            _configuration = configuration;
        }
        #endregion

        #region Propriétés
        public DirectoryInfo CommitFolder { get; set; }
        public List<Commit> Commits { get; set; }
        public IConfigurationRoot Configuration => _configuration;
        public string Name => _directory.Name;
        #endregion

        #region Méthodes
        public static void Create(string name)
        {
            var folder = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), name));
            if(!folder.Exists)
            {
                folder.Create();
            }
            // créer l'arborescence
            if (folder.GetFiles().Any())
            {
                throw new InvalidOperationException("The directory is not empty.");
            }
            folder.CreateSubdirectory(Commit_Folder);
            FileInfo configFile = new FileInfo(Path.Combine(folder.FullName, ConfigFile));
            using (var sw = configFile.CreateText())
            {
                sw.WriteLine("{");
                sw.WriteLine("}");
            }
        }

        public bool Validate(Models.Commit commit)
        {
            var max = Commits.Max(c => c.Version);
            return commit.Version > max;
        }

        #endregion
    }
}
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Monbsoft.EvolDB.Repository
{
    public class MigrationRepository : IRepository
    {
        #region Champs
        public const string CommitFolder = "commits";
        public const string ConfigFile = "config.json";
        private readonly DirectoryInfo _directory;
        private IConfigurationRoot _configuration;
        #endregion

        #region Constructeurs
        public MigrationRepository(string path)
            : this(path, null)
        {

        }

        public MigrationRepository(string path, IConfigurationRoot configuration)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException();
            }

            _directory = new DirectoryInfo(path);
            _configuration = configuration;
        }
        #endregion

        #region Propriétés
        public IConfigurationRoot Configuration => _configuration;
        public string Name => _directory.Name;
        #endregion

        #region Méthodes
        public void Create()
        {
            if (_directory.GetDirectories().Any())
            {
                throw new InvalidOperationException("The directory is not empty.");
            }
            _directory.CreateSubdirectory(CommitFolder);
            FileInfo configFile = new FileInfo(Path.Combine(_directory.FullName, ConfigFile));
            using (var sw = configFile.CreateText())
            {
                sw.WriteLine("{");
                sw.WriteLine("}");
            }
        }

        public void Load()
        {
            var scripts = GetScripts();
            foreach(var script in scripts)
            {
                
            }
        }

        public List<FileInfo> GetScripts()
        {
            var scriptFolder = _directory.GetDirectories(CommitFolder).First();
            
            return scriptFolder.GetFiles().ToList();
        }
        #endregion
    }
}
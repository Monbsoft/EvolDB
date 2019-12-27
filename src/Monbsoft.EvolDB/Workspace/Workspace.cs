using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace Monbsoft.EvolDB.Workspace
{
    public class Workspace : IWorkspace
    {
        #region Champs
        public const string ScriptFolder = "scripts";
        public const string ConfigFile = "config.json";
        private readonly DirectoryInfo _directory;
        private IConfigurationRoot _configuration;
        #endregion

        #region Constructeurs
        public Workspace(string path)
            : this(path, null)
        {

        }

        public Workspace(string path, IConfigurationRoot configuration)
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
            _directory.CreateSubdirectory(ScriptFolder);
            FileInfo configFile = new FileInfo(Path.Combine(_directory.FullName, ConfigFile));
            using (var sw = configFile.CreateText())
            {
                sw.WriteLine("{");
                sw.WriteLine("}");
            }
        }
        #endregion
    }
}
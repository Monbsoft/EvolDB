using System;
using System.IO;
using System.Linq;

namespace Monbsoft.EvolDB
{
    public class Workspace : IWorkspace
    {

        #region Champs
        public const string ScriptFolder = "scripts";
        public const string ConfigFile = "config.json";
        private readonly DirectoryInfo _directory;
        #endregion

        #region Constructeurs
        public Workspace(string path)
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
        }
        #endregion

        #region Propriétés
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
            configFile.Create();
        }
        #endregion
    }
}

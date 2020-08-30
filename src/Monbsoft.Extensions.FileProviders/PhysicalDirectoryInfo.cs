using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Monbsoft.Extensions.FileProviders
{
    public class PhysicalDirectoryInfo : IDirectoryInfo
    {

        #region Champs
        private readonly DirectoryInfo _info;
        #endregion

        #region Constructeurs
        public PhysicalDirectoryInfo(string path)
        {
            _info = new DirectoryInfo(path);          
        }

        public PhysicalDirectoryInfo(DirectoryInfo directoryInfo)
        {
            _info = directoryInfo;
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Gets a value whether the directory exists.
        /// </summary>
        public bool Exists => _info.Exists;

        /// <summary>
        /// Gets the name of this directory.
        /// </summary>
        public string Name => _info.Name;

        /// <summary>
        /// Gets the full path of the directory.
        /// </summary>
        public string PhysicalPath => _info.FullName;
        #endregion

        #region Méthodes
        /// <summary>
        /// Creates a directory.
        /// </summary>
        public void Create()
        {
            _info.Create();
        }

        /// <summary>
        /// Returns the file of the current directory.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IFileInfo GetFile(string name)
        {
            return new PhysicalFileInfo(Path.Combine(PhysicalPath, name));
        }

        /// <summary>
        /// Returns the file list of the current directory.
        /// </summary>
        /// <returns></returns>
        public IList<IFileInfo> GetFiles()
        {
            var files = new List<IFileInfo>();
            foreach(var file in _info.GetFiles())
            {
                files.Add(new PhysicalFileInfo(file));
            }
            return files;

        }

        public IDirectoryInfo GetFolder(string name)
        {
            return new PhysicalDirectoryInfo(Path.Combine(PhysicalPath, name));
        }

        #endregion
    }
}

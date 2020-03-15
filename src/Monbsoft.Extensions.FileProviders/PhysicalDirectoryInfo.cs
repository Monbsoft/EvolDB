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
        public PhysicalDirectoryInfo(DirectoryInfo info)
        {
            _info = info;
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
        /// Creates a new instance on the specified path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IDirectoryInfo Create(string path)
        {
            return new PhysicalDirectoryInfo(new DirectoryInfo(path));
        }

        /// <summary>
        /// Creates a new file on the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IFileInfo CreateFile(string name)
        {
            return PhysicalFileInfo.Create(Path.Combine(_info.FullName, name));
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
            var subDirectory = _info.GetDirectories(name).FirstOrDefault();
            if(subDirectory != null)
            {
                return new PhysicalDirectoryInfo(subDirectory);
            }
            return null;
        }

        #endregion
    }
}

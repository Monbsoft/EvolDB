using System.Collections.Generic;
using System.IO;

namespace Monbsoft.Extensions.FileProviders
{
    public class PhysicalFileInfo : IFileInfo
    {
        #region Champs
        private readonly FileInfo _info;
        #endregion

        #region Constructeurs
        public PhysicalFileInfo(string path)
        {
            _info = new FileInfo(path);
        }

        public PhysicalFileInfo(FileInfo fileInfo)
        {
            _info = fileInfo;
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Gets a value whether a file exists
        /// </summary>
        public bool Exists => _info.Exists;

        /// <summary>
        /// Gets the full path of the file.
        /// </summary>
        public string FullName => _info.FullName;
        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public string Name => _info.Name;
        #endregion

        #region Méthodes
        /// <summary>
        /// Creates a file.
        /// </summary>
        /// <returns></returns>
        public FileStream Create()
        {
            return _info.Create();
        }

        /// <summary>
        /// Creates a read-only stream.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        public FileStream OpenRead() => _info.OpenRead();

        public List<string> ReadLines(string path)
        {
            var lines = new List<string>();
            string line;
            using (var reader = new StreamReader(path))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line.Trim());
                }
            }
            return lines;
        }
        #endregion
    }
}
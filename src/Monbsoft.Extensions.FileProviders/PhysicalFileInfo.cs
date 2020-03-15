using System.IO;

namespace Monbsoft.Extensions.FileProviders
{
    public class PhysicalFileInfo : IFileInfo
    {
        #region Champs
        private readonly FileInfo _info;
        #endregion

        #region Constructeurs
        public PhysicalFileInfo(FileInfo info)
        {
            _info = info;
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Gets a value whether a file exists
        /// </summary>
        public bool Exists => _info.Exists;

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public string Name => _info.Name;

        /// <summary>
        /// Gets the full path of the file.
        /// </summary>
        public string PhysicalPath => _info.FullName;
        #endregion

        #region Méthodes
        /// <summary>
        /// Creates a new file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IFileInfo Create(string path)
        {
            return new PhysicalFileInfo(new FileInfo(path));
        }

        /// <summary>
        /// Creates a file.
        /// </summary>
        /// <returns></returns>
        public FileStream Create() => _info.Create();

        /// <summary>
        /// Creates a read-only stream.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        public FileStream OpenRead() => _info.OpenRead();
        #endregion
    }
}
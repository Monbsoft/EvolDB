using System.Collections.Generic;

namespace Monbsoft.Extensions.FileProviders
{
    public interface IDirectoryInfo
    {
        /// <summary>
        /// Gets a value whether a file exits.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the path to the file.
        /// </summary>
        string PhysicalPath { get; }

        /// <summary>
        /// Creates a new file on the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IFileInfo CreateFile(string name);

        /// <summary>
        /// Gets the file list fo the current directory.
        /// </summary>
        /// <returns></returns>
        IList<IFileInfo> GetFiles();

        IDirectoryInfo GetFolder(string name);
    }
}
using System.IO;

namespace Monbsoft.Extensions.FileProviders
{
    public interface IFileInfo
    {
        /// <summary>
        /// Gets a value whether a file exists.
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
        /// Creates a file.
        /// </summary>
        /// <returns></returns>
        FileStream Create();

        /// <summary>
        /// Creates a read-only stream.
        /// </summary>
        /// <returns></returns>
        FileStream OpenRead();
    }
}
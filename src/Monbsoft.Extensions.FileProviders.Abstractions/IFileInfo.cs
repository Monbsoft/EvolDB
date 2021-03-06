﻿using System.Collections.Generic;
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
        /// Gets the path to the file.
        /// </summary>
        string FullName { get; }
        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Creates a file.
        /// </summary>
        /// <returns></returns>
        void Create();

        /// <summary>
        /// Reads all characters  from the current position to the end of the stream.
        /// </summary>
        /// <returns></returns>
        string ReadAll();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<string> ReadLines();

        /// <summary>
        /// Creates a read-only stream.
        /// </summary>
        /// <returns></returns>
        FileStream OpenRead();
    }
}
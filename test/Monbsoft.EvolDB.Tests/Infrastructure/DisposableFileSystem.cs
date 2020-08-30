using Monbsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace Monbsoft.EvolDB.Tests.Infrastructure
{
    public class DisposableFileSystem : IDisposable
    {
        private bool _disposed = false;

        public DisposableFileSystem()
        {
            RootPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            DirectoryInfo = new PhysicalDirectoryInfo(RootPath);
            DirectoryInfo.Create();

        }

        public IDirectoryInfo DirectoryInfo { get; }
        public string RootPath { get; }

        public DisposableFileSystem CreateFile(string path)
        {
            File.WriteAllText(Path.Combine(RootPath, path), "test");
            return this;
        }
        public DisposableFileSystem CreateFile(FileInfo fileInfo)
        {
            using (var stream = fileInfo.Create())
            using (var writer = new StreamWriter(stream))
            {
                writer.Write("test");
            }
            return this;
        }

        public DisposableFileSystem CreateFileWithContent(string path, string content)
        {
            return CreateFileWithContent(new FileInfo(Path.Combine(RootPath, path)), content);
        }

        public DisposableFileSystem CreateFileWithContent(FileInfo fileInfo, string content)
        {
            using(var writer  = fileInfo.CreateText())
            {
                writer.Write(content);
            }
            return this;
        }

        public DisposableFileSystem CreateFiles(params string[] fileRelativePaths)
        {
            foreach (var path in fileRelativePaths)
            {
                string fullPath = Path.Combine(RootPath, path);
                File.WriteAllText(fullPath, "generated for testing");
            }
            return this;
        }
        public DisposableFileSystem CreateFolder(string path)
        {
            Directory.CreateDirectory(Path.Combine(RootPath, path));
            return this;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public DirectoryInfo GetDirectory(string path)
            => new DirectoryInfo(Path.Combine(RootPath, path));

        public IFileInfo GetFile(string path)
            => new PhysicalFileInfo(Path.Combine(RootPath, path));


        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                try
                {
                    Directory.Delete(RootPath, true);
                }
                catch
                {
                    // Don't throw if this fails.
                }
            }
        }
    }
}
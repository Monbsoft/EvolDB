using Monbsoft.EvolDB.Models;
using System;
using System.IO;
using System.Linq;

namespace Monbsoft.EvolDB.Services
{
    public class RepositoryService : IRepositoryService
    {
        /// <summary>
        /// Creates a migration repository with the specified name.
        /// </summary>
        /// <param name="name"></param>
        public void Create(string name)
        {
            var folder = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), name));
            if (!folder.Exists)
            {
                folder.Create();
            }

            // détecte que le dossier est bien vide.
            if (folder.GetFiles().Any())
            {
                throw new InvalidOperationException("The directory is not empty.");
            }
            // créer l'arborescence
            folder.CreateSubdirectory(Repository.Commit_Folder);
            FileInfo configFile = new FileInfo(Path.Combine(folder.FullName, Repository.Config_File));
            using (var sw = configFile.CreateText())
            {
                sw.WriteLine(
                    @"{
    ""ConnectionType"": "" ""
}");
            }
        }
    }
}
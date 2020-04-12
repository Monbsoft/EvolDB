﻿using Microsoft.Extensions.Configuration;
using Monbsoft.EvolDB.Collections;
using Monbsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;

namespace Monbsoft.EvolDB.Models
{
    public class Repository
    {
        public const string Commit_Folder = "commits";
        public const string ConfigFile = "config.json";     


        public Repository(IDirectoryInfo folder, IConfigurationRoot configuration)
        {
            RootFolder = folder ?? throw new ArgumentNullException(nameof(folder));
            if (!RootFolder.Exists)
            {
                throw new DirectoryNotFoundException(nameof(RootFolder));
            }
            CommitFolder = RootFolder.GetFolder(Commit_Folder);
            if(!CommitFolder.Exists)
            {
                throw new DirectoryNotFoundException(nameof(CommitFolder));
            }
            Configuration = configuration;
            Commits = new CommitCollection(this);
        }

        public IDirectoryInfo CommitFolder { get; }
        public CommitCollection Commits { get; }
        public IConfigurationRoot Configuration { get; }
        public string Name => RootFolder.Name;
        public IDirectoryInfo RootFolder { get; }

        public IList<IFileInfo> GetCommitFiles()
        {
            return CommitFolder.GetFiles();
        }
    }
}
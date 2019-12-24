using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Monbsoft.EvolDB.Cli.Models
{
    public class Workspace
    {

        #region Champs
        private string _path;
        #endregion

        #region Constructeurs
        public Workspace(string path)
        {
            if(string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if(!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException();
            }
            _path = path;
        }
        #endregion

        #region Propriétés
        #endregion

        #region Méthodes
        public void Create()
        {

        }
        #endregion
    }
}

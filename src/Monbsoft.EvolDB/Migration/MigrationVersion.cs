using System;

namespace Monbsoft.EvolDB
{
    public class MigrationVersion : IComparable
    {
        #region Champs
        private readonly int _hashCode;
        #endregion

        #region Constructeurs
        public MigrationVersion(int major, int minor, int patch, int revision)
        {
            _hashCode = major ^ minor ^ patch ^ revision;
        }
        #endregion

        #region Propriétés
        public int Major { get; set; }

        public int Minor { get; set; }

        public int Patch { get; set; }

        public int Revision { get; set; }
        #endregion

        #region Méthodes
        /// <summary>
        /// Détermine si 2 versions de migration ne sont pas égales.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(MigrationVersion left, MigrationVersion right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Détermine si la première version est inférieure à la seconde version.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(MigrationVersion left, MigrationVersion right)
        {
            if (left is null)
            {
                return !(right is null);
            }

            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Détermine si la première version est inférieure ou égale à la seconde.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(MigrationVersion left, MigrationVersion right)
        {
            if(left is null)
            {
                return !(right is null);
            }
            return left.CompareTo(right) <= 0; 
        }

        /// <summary>
        /// Détermine si 2 versions de migration sont égales.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(MigrationVersion left, MigrationVersion right)
        {
            return !(left is null) && left.Equals(right);
        }
        /// <summary>
        /// Détermine si la première version est supérieure qu'à la seconde version.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(MigrationVersion left, MigrationVersion right)
        {
            if(left is null)
            {
                return !(right is null);
            }

            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Détermine si la première version est supérieure ou égale qu'à la seconde version.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(MigrationVersion left, MigrationVersion right)
        {
            if(left is null)
            {
                return !(right is null);
            }

            return left.CompareTo(right) >= 0;
        }

        /// <summary>
        /// Compare l'instance actuelle avec un autre objet.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            MigrationVersion other = obj as MigrationVersion;
            return CompareTo(other);
        }

        /// <summary>
        /// Compare l'instance actuelle avec une autre version.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(MigrationVersion other)
        {
            if (other is null)
            {
                return 1;
            }

            int majorCompare = Major.CompareTo(other.Major);

            if (majorCompare != 0)
            {
                return majorCompare;
            }

            int minorCompare = Minor.CompareTo(other.Minor);

            if (minorCompare != 0)
            {
                return minorCompare;
            }

            int patchCompare = Patch.CompareTo(other.Patch);

            if (patchCompare != 0)
            {
                return patchCompare;
            }

            int revisionCompare = Revision.CompareTo(other.Revision);

            if (revisionCompare != 0)
            {
                return revisionCompare;
            }

            return 0;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as MigrationVersion);
        }

        public bool Equals(MigrationVersion other)
        {
            return !ReferenceEquals(other, null) &&
                Major == other.Major &&
                Minor == other.Minor &&
                Patch == other.Patch &&
                Revision == other.Revision;
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }
        #endregion
    }
}
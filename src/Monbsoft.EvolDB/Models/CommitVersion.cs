using System;
using System.Globalization;

namespace Monbsoft.EvolDB
{
    public class CommitVersion : IEquatable<CommitVersion>, IComparable
    {
        #region Champs
        private readonly int _hashCode;
        #endregion

        #region Constructeurs
        public CommitVersion(int major, int minor, int patch, int revision)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Revision = revision;
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
        public static bool operator !=(CommitVersion left, CommitVersion right)
        {
            return !(left == right);
        }
        /// <summary>
        /// Détermine si la première version est inférieure à la seconde version.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(CommitVersion left, CommitVersion right)
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
        public static bool operator <=(CommitVersion left, CommitVersion right)
        {
            if (left is null)
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
        public static bool operator ==(CommitVersion left, CommitVersion right)
        {
            return !(left is null) && left.Equals(right);
        }
        /// <summary>
        /// Détermine si la première version est supérieure qu'à la seconde version.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(CommitVersion left, CommitVersion right)
        {
            if (left is null)
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
        public static bool operator >=(CommitVersion left, CommitVersion right)
        {
            if (left is null)
            {
                return !(right is null);
            }

            return left.CompareTo(right) >= 0;
        }
        public static bool TryParse(string source, out CommitVersion version)
        {
            if(string.IsNullOrWhiteSpace(source))
            {
                version = null;
                return false;
            }

            source = source.Trim();
            string[] tab = source.Split('_');

            if(tab.Length != 4)
            {
                version = null;
                return false;
            }
            int major = int.Parse(tab[0], NumberStyles.None, CultureInfo.InvariantCulture);
            int minor = int.Parse(tab[1], NumberStyles.None, CultureInfo.InvariantCulture);
            int patch = int.Parse(tab[2], NumberStyles.None, CultureInfo.InvariantCulture);
            int revision = int.Parse(tab[3], NumberStyles.None, CultureInfo.InvariantCulture);

            version = new CommitVersion(major, minor, patch, revision);
            return true;
        }
        /// <summary>
        /// Compare l'instance actuelle avec un autre objet.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            CommitVersion other = obj as CommitVersion;
            return CompareTo(other);
        }

        /// <summary>
        /// Compare l'instance actuelle avec une autre version.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(CommitVersion other)
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
            return Equals(obj as CommitVersion);
        }

        public bool Equals(CommitVersion other)
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

        public override string ToString()
        {
            return $"{Major}_{Minor}_{Patch}_{Revision}";
        }
        #endregion
    }
}
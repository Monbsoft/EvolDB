using System;
using System.Globalization;

namespace Monbsoft.EvolDB
{
    public class CommitVersion : IEquatable<CommitVersion>, IComparable
    {
        #region Champs
        private static char[] SeparatorArray = new char[] { '_', '.' };
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

        public CommitVersion(int[] numbers)
        {
            if(numbers.Length > 4 && numbers.Length < 3)
            {
                throw new ArgumentException(nameof(numbers));
            }
            Major = numbers[0];
            Minor = numbers[1];
            Patch = numbers[2];
            if (numbers.Length == 4)
            {
                Revision = numbers[3];
            }
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
        public static CommitVersion Parse(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input));
            }
            CommitVersion version;
            if (!TryParse(input, out version))
            {
                throw new ArgumentOutOfRangeException("version");
            }
            return version;
        }

        public static bool TryParse(string version, out CommitVersion result)
        {
            int major, minor, patch = 0, revision = 0;
            result = null;

            var parsedComponents = version.Split(SeparatorArray);
            int length = parsedComponents.Length;
            if ((length < 2) || (length > 4))
            {
                result = null;
                return false;
            }

            if (!TryParseComponent(parsedComponents[0], out major))
            {
                return false;
            }
            if (!TryParseComponent(parsedComponents[1], out minor))
            {
                return false;
            }
            length -= 2;
            if (length > 0)
            {
                if (!TryParseComponent(parsedComponents[2], out patch))
                {
                    return false;
                }
                length--;
                if (length > 0)
                {
                    if (!TryParseComponent(parsedComponents[3], out revision))
                    {
                        return false;
                    }
                }
            }

            result = new CommitVersion(major, minor, patch, revision);
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
        private static bool TryParseComponent(string component, out int result)
        {
            return Int32.TryParse(component, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
        }
        #endregion
    }
}
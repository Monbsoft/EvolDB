using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Monbsoft.EvolDB.Infrastructure
{
    public static class HashUtilities
    {

        public static string ComputeHash(string fileName)
        {
            string md5Hash = null;

            using (Stream stream = File.OpenRead(fileName))
            using (var bufferedStream = new BufferedStream(stream, 1024 * 32))
            {

                byte[] checksum;

                using (var md5Cng = MD5.Create())
                {
                    checksum = md5Cng.ComputeHash(bufferedStream);
                    md5Hash = BitConverter.ToString(checksum).Replace("-", string.Empty);
                }
            }

            return md5Hash;
        }
    }
}

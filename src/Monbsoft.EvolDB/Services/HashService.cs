﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Monbsoft.EvolDB.Services
{
    public class HashService : IHashService
    {
        public string ComputeHash(string fileName)
        {
            string shaHash = null;

            using (Stream stream = File.OpenRead(fileName))
            using (var bufferedStream = new BufferedStream(stream, 1024 * 32))
            {

                byte[] checksum;

                using (var shaCng = SHA1.Create())
                {
                    checksum = shaCng.ComputeHash(bufferedStream);
                    shaHash = BitConverter.ToString(checksum).Replace("-", string.Empty);
                }
            }

            return shaHash;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawPadIO.Hub.Auth.Options
{
    public class Argon2iPasswordHashingServiceOptions
    {
        public int DegreeOfParallelism { get; set; }
        public int Iterations { get; set; }
        public byte[] KnownSecret { get; set; }
        public int MemorySize { get; set; }
        public int HashSize { get; set; }
        public int SaltSize { get; set; }
    }
}

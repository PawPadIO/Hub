using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PawPadIO.Hub.Auth.Models;
using PawPadIO.Hub.Auth.Options;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace PawPadIO.Hub.Auth.Services
{
    public class Argon2iPasswordHashingService : IPasswordHasher<ApplicationUser>
    {
        private readonly Argon2iPasswordHashingServiceOptions _options;

        public Argon2iPasswordHashingService(IOptions<Argon2iPasswordHashingServiceOptions> options)
        {
            _options = options.Value;
        }

        public string HashPassword(ApplicationUser user, string password)
        {
            byte[] salt = new byte[_options.SaltSize];
            var csprng = RandomNumberGenerator.Create();
            csprng.GetBytes(salt);

            var argon2i = new Argon2i(Encoding.UTF8.GetBytes(password))
            {
                AssociatedData = GetAssociatedData(user),
                DegreeOfParallelism = _options.DegreeOfParallelism,
                Iterations = _options.Iterations,
                KnownSecret = _options.KnownSecret,
                MemorySize = _options.MemorySize,
                Salt = salt,
            };
            var hash = argon2i.GetBytes(_options.HashSize);

            return $"{Convert.ToBase64String(hash)}:{Convert.ToBase64String(argon2i.Salt)}:{argon2i.Iterations.ToString(CultureInfo.InvariantCulture)}:{argon2i.MemorySize.ToString(CultureInfo.InvariantCulture)}:{argon2i.DegreeOfParallelism.ToString(CultureInfo.InvariantCulture)}";
        }

        public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
        {
            var storedHashParams = hashedPassword.Split(':');
            var storedDegreeOfParallelism = Convert.ToInt32(storedHashParams[4], CultureInfo.InvariantCulture);
            var storedIterations = Convert.ToInt32(storedHashParams[2], CultureInfo.InvariantCulture);
            var storedMemorySize = Convert.ToInt32(storedHashParams[3], CultureInfo.InvariantCulture);
            var storedSalt = Convert.FromBase64String(storedHashParams[1]);
            var storedHash = Convert.FromBase64String(storedHashParams[0]);

            var argon2i = new Argon2i(Encoding.UTF8.GetBytes(providedPassword))
            {
                AssociatedData = GetAssociatedData(user),
                DegreeOfParallelism = storedDegreeOfParallelism,
                Iterations = storedIterations,
                KnownSecret = _options.KnownSecret,
                MemorySize = storedMemorySize,
                Salt = storedSalt,
            };
            var providedhash = argon2i.GetBytes(_options.HashSize);
            if (!ByteArraysEqual(providedhash, storedHash))
            {
                return PasswordVerificationResult.Failed;
            }
            if (storedDegreeOfParallelism != _options.DegreeOfParallelism || 
                storedIterations != _options.Iterations || 
                storedMemorySize != _options.MemorySize ||
                storedSalt.Length != _options.SaltSize ||
                providedhash.Length != _options.HashSize)
            {
                return PasswordVerificationResult.SuccessRehashNeeded;
            }
            return PasswordVerificationResult.Success;
        }

        private byte[] GetAssociatedData(ApplicationUser user)
        {
            byte[] userId;
            Guid userGuid;
            if (Guid.TryParse(user.Id, out userGuid))
            {
                userId = userGuid.ToByteArray();
            }
            else
            {
                userId = Encoding.UTF8.GetBytes(user.Id);
            }
            return userId;
        }

        // Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }
    }
}

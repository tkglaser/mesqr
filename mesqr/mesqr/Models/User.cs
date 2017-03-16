using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace mesqr.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(150)]
        public string UserName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Password 
        {
            set
            {
                EncryptedPassword = EncryptPassword(value);
            }
        }

        [Required]
        [MaxLength(250)]
        public string EncryptedPassword { get; set; }

        [MaxLength(250)]
        public string EMail { get; set; }

        public bool IsPasswordValid(string password)
        {
            return VerifyHashedPassword(password, EncryptedPassword);
        }

        private static readonly int saltSize = 4; // since the string might be concatenated, the salt must be of fixed size and at the beginning

        private static string EncryptPassword(string password, byte[] saltBytes = null)
        {
            if (saltBytes == null)
            {
                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[saltSize];

                // Initialize a random number generator.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);
            }

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] passwordWithSaltBytes = saltBytes.Concat(passwordBytes).ToArray();

            HashAlgorithm hash = new SHA256Managed();

            byte[] hashBytes = hash.ComputeHash(passwordWithSaltBytes);
            byte[] hashWithSaltBytes = saltBytes.Concat(hashBytes).ToArray();

            // limit the string to the length of the password column in the db, but multiple of 4
            return Convert.ToBase64String(hashWithSaltBytes);
        }

        private static bool VerifyHashedPassword(string plainText, string hashValue)
        {
            try
            {
                byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);
                byte[] saltBytes = hashWithSaltBytes.Take(saltSize).ToArray();

                string expectedHashString = EncryptPassword(plainText, saltBytes);

                return (hashValue == expectedHashString);
            }
            catch
            {
            }
            return false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace mesqr.Models
{
    public class Session
    {
        public int SessionId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Start { get; set; }

        public DateTime Expiry { get; set; }

        [Required]
        [MaxLength(25)]
        public string Key { get; set; }

        public static string RandomString(int length)
        {
            // Allocate a byte array, which will hold the salt.
            var saltBytes = new byte[length];

            // Initialize a random number generator.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            // Fill the salt with cryptographically strong byte values.
            rng.GetNonZeroBytes(saltBytes);

            var itresult = Convert.ToBase64String(saltBytes).Substring(0, length);
            var result = itresult;

            foreach (var c in itresult.ToCharArray().Where(c => !char.IsLetterOrDigit(c)).Distinct())
            {
                result = result.Replace(c, 'x');
            }

            return result;
        }

    }
}
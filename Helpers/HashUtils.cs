using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace CAEV.PagoLinea.Helpers
{
    public class HashUtils {

        public static string GetHash( string data, string saltString )
        {
            byte[] salt = Encoding.UTF8.GetBytes( saltString );
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: data,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100,
                numBytesRequested: 100)
            );
           return hashed;
        }

        public static string GetHash2( string data, string key )
        {
            // Convert the key and message to byte arrays
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var messageBytes = Encoding.UTF8.GetBytes(data);
            
            // Create an HMACSHA256 instance with the key
            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                // Compute the hash
                var hashBytes = hmacsha256.ComputeHash(messageBytes);
                
                // Convert the hash to a hexadecimal string
                var hashHex = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hashHex;
            }
        }

    }
}
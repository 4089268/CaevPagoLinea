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

    }
}
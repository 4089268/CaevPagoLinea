using System;
using System.Collections.Generic;
using BCrypt.Net;
using CAEV.PagoLinea.Data;
using CAEV.PagoLinea.Models;

namespace CAEV.PagoLinea.Services {

    public class UserService {

        private readonly PagoLineaContext pagoLineaContext;

        public UserService(PagoLineaContext context) {
            pagoLineaContext = context;
        }

        // public async Task CreateUserAsync(string username, string plainPassword){
        //     var hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

        //     var user = new User {
        //         Username = username,
        //         Password = hashedPassword
        //     };

        //     _context.Users.Add(user);
        //     await _context.SaveChangesAsync();
        // }

        public bool VerifyPassword(string enteredPassword, string storedHashedPassword) {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHashedPassword);
        }
    }
}
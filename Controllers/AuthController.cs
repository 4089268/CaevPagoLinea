using System;
using System.Collections;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using CAEV.PagoLinea.Models;
using CAEV.PagoLinea.Services;
using CAEV.PagoLinea.Data;
using CAEV.PagoLinea.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace CAEV.PagoLinea.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly PagoLineaContext pagoLineaContext;

    public AuthController(ILogger<AuthController> logger, PagoLineaContext context){
        _logger = logger;
        this.pagoLineaContext = context;
    }

    [HttpGet]
    public ActionResult Login(){
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(string username, string password) {
        
        var user = IsValidUser(username, password);
        if(user == null){
            ViewBag.Message = "Invalid credentials";
            return View();
        }
        await LogInUser(user!);
        return RedirectToAction("Dashboard"); // Redirect after login

    }

    public async Task<ActionResult> Logout() {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }


    [Authorize]
    [HttpGet]
    public ActionResult Dashboard(){
        return View();
    }


    #region private functions
    private User? IsValidUser(string username, string password) {

        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        Console.WriteLine($"{username}, {password}, {hashedPassword}");

        return this.pagoLineaContext.Users
            .Where( item => item.Email!.ToLower() == username.ToLower())
            .ToList()
            .FirstOrDefault( item => BCrypt.Net.BCrypt.Verify( password, item.Password));
    }

    private async Task LogInUser(User user){
        var claims = new List<Claim> {
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.Name!),
            new(ClaimTypes.Role, "Administrator"),
            new(ClaimTypes.Actor, $"https://ui-avatars.com/api/?name={user.Name!.Replace(" ", "+")}&color=333&rounded=true"),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            //AllowRefresh = <bool>,
            // Refreshing the authentication session should be allowed.

            //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            // The time at which the authentication ticket expires. A 
            // value set here overrides the ExpireTimeSpan option of 
            // CookieAuthenticationOptions set with AddCookie.

            IsPersistent = true,
            // Whether the authentication session is persisted across 
            // multiple requests. When used with cookies, controls
            // whether the cookie's lifetime is absolute (matching the
            // lifetime of the authentication ticket) or session-based.

            //IssuedUtc = <DateTimeOffset>,
            // The time at which the authentication ticket was issued.

            //RedirectUri = <string>
            // The full path or absolute URI to be used as an http 
            // redirect response value.
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        _logger.LogInformation("User {Email} logged in at {Time}.", user.Email, DateTime.UtcNow);
    }
    #endregion

}

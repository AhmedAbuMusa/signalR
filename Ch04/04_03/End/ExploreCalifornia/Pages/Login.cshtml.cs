﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExploreCalifornia.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public IActionResult OnPost([FromQuery] string returnUrl = null)
        {
            if (!ModelState.IsValid) return Page();

            var username = Request.Form["username"];
            var password = Request.Form["password"];

            // DO NOT do this in production!
            // You will have a bad time
            if (password != "explore") return Page();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Support"),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var properties = new AuthenticationProperties
            {
                RedirectUri = returnUrl ?? Url.Content("~/")
            };

            return SignIn(
                new ClaimsPrincipal(identity),
                properties,
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}

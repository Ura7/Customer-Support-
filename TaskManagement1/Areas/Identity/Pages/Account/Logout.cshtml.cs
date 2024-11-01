// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TaskManagement1.Models;

namespace TaskManagement1.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public LogoutModel(SignInManager<AppUser> signInManager, ILogger<LogoutModel> logger, IHttpContextAccessor contextAccessor)
        {
            _signInManager = signInManager;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            _contextAccessor.HttpContext.Session.Remove("AccessToken");
            _contextAccessor.HttpContext.Session.Remove("RefreshToken");
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await _signInManager.SignOutAsync();
            
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}

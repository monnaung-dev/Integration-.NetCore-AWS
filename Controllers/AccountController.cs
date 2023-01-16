using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NY.SmartParking.Web.Controllers;
using SmartParking.Admin.Models.AccountViewModels;

namespace NY.SmartParking.Web.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly ILogger _logger;

        public AccountController(
            SignInManager<CognitoUser> _signInManager,            
            ILogger<AccountController> logger)
        {
            this._signInManager = _signInManager;
            _logger = logger;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            try
            {
                
                ViewData["ReturnUrl"] = returnUrl;
                if (ModelState.IsValid)
                {
                    
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                      

                        _logger.LogInformation("User logged in.");
                        return RedirectToLocal(returnUrl);
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToAction(nameof(Lockout));
                    }
                    else if (result.IsCognitoSignInResult())
                    {
                        
                        if (result is CognitoSignInResult cognitoResult)
                        {
                            
                            if (cognitoResult.RequiresPasswordChange)
                            {
                                _logger.LogWarning("User password needs to be changed");
                                return RedirectToAction("ChangePassword");
                            }
                            else if (cognitoResult.RequiresPasswordReset)
                            {
                                _logger.LogWarning("User password needs to be reset");
                                return RedirectToPage("./ResetPassword");
                            }
                        }

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(string returnUrl = null)
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_signInManager.UserManager.GetUserId(User)}'.");
            }

            
            var changePasswordResult = await _signInManager.UserManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }
            else
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation("User changed their password successfully.");
                StatusMessage = "Your password has been changed.";
            }

            
            
            
            return RedirectToAction("Index", "Home");
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

       

        
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(NY.SmartParking.Web.Controllers.HomeController.Index), "Home");
        }

      




        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}

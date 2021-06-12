using EcommerceMVC.Auth;
using EcommerceMVC.Models;
using EcommerceMVC.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EcommerceMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signManager,
            IJwtAuthManager jwtAuthManager
            )
        {
            _jwtAuthManager = jwtAuthManager;
            _userManager = userManager;
            _signManager = signManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            try
            {
                ViewBag.Message = "User already register";

                ApplicationUser user = await _userManager.FindByNameAsync(registerViewModel.Name);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = registerViewModel.Name,
                        Email = registerViewModel.Email
                    };

                    IdentityResult result = await _userManager.CreateAsync(user, registerViewModel.Password);

                    if (result.Succeeded)
                    {
                        //IdentityResult result1 = await _userManager.AddToRoleAsync(user, "Admin");

                        //if(result1.Succeeded)
                        //{
                        //    return RedirectToAction("Login");
                        //}
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.UserName),
                            new Claim(ClaimTypes.Email, user.Email)
                        };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return RedirectToAction("Login");
                    }
                }

                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            return RedirectToAction("Register");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (returnUrl != null)
            {
                ViewData["ReturnUrl"] = returnUrl;
            }

            LoginViewModel loginViewModel = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
                ExternalLogin = (await _signManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(loginViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            var result = await _signManager.PasswordSignInAsync(loginViewModel.userName, loginViewModel.Password, false, false);
            if (result.Succeeded)
            {
                //var token = _jwtAuthManager.Authenticate(loginViewModel.userName, loginViewModel.Password);
                //if(token != null)
                //{
                //    return RedirectToAction("Index", "Product");
                //}
                if (returnUrl != null)
                {
                    var claims = new List<Claim>();
                   


                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);

                    return LocalRedirect(returnUrl);
                }

                return RedirectToAction("Index", "Product");
            }
            else
            {
                ViewBag.Result = "result is : " + result.ToString();
            }

            return RedirectToAction("Login");
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider,string returnUrl)
        {
            var redirectUrl = Url.Action("ExteralLoginCallback", "Account", new { RetrunUrl = returnUrl });
            var properties = _signManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExteralLoginCallback(string returnUrl = null ,string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("/");

            LoginViewModel loginViewModel = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
                ExternalLogin = (await _signManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if(remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error From External Provider : {remoteError}");

                return View("Login", loginViewModel);
            }

            var info = await _signManager.GetExternalLoginInfoAsync();
            if(info == null)
            {
                ModelState.AddModelError(string.Empty, $"Error loading external login information");

                return View("Login", loginViewModel);
            }

            var signResult = await _signManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                                                                    isPersistent: false, bypassTwoFactor: true);

            if(signResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if(email != null)
                {
                    var user =await _userManager.FindByEmailAsync(email);
                    if(user == null)
                    {
                        user = new ApplicationUser()
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                        };

                        await _userManager.CreateAsync(user);
                    }
                    await _userManager.AddLoginAsync(user, info);
                    await _signManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            await _signManager.SignOutAsync();
            return RedirectToAction("index");
        }


    }
}

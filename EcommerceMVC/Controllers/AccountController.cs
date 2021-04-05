using EcommerceMVC.Models;
using EcommerceMVC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EcommerceMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager)
        {
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

                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index", "Product");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            return RedirectToAction("Register");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var result = await _signManager.PasswordSignInAsync(loginViewModel.userName, loginViewModel.Password,false,false);
            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Product");
            }
            else
            {
                ViewBag.Result = "result is : " + result.ToString();
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("index");
        }
    }
}

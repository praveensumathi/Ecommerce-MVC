using EcommerceMVC.Models;
using EcommerceMVC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EcommerceMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel role)
        {
            var roleExist = await _roleManager.RoleExistsAsync(role.RoleName);
            if(ModelState.IsValid && !roleExist)
            {
                AppRole appRole = new AppRole()
                {
                    Name = role.RoleName,
                };

                IdentityResult result = await _roleManager.CreateAsync(appRole);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }
            }

            return View();
        }
    }
}

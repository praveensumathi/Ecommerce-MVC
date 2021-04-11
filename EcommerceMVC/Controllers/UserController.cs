using EcommerceMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace EcommerceMVC.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IConfiguration configuration, ApplicationDbContext applicationDbContext,
            UserManager<ApplicationUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Product> userProduct = new List<Product>();

            ApplicationUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            _applicationDbContext.Entry(user).Collection(x => x.Products).Load();

            foreach (Product product in user.Products)
            {
                userProduct.Add(product);
            }

            ViewBag.MyCartsCount = userProduct.Count();
            return View(userProduct);
        }

        [HttpGet("AddToCart")]
        public IActionResult AddToCart(int id)
        {
            var product = _applicationDbContext.Product.FirstOrDefault((x) => x.Id == id);

            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            _applicationDbContext.Users.FirstOrDefault((x) => x.Id == user.Id).Products.Add(product);
            _applicationDbContext.SaveChangesAsync();
            
            return RedirectToAction("Index", "User");
        }
    }
}

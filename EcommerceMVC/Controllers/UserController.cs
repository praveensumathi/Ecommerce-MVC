using EcommerceMVC.Models;
using EcommerceMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace EcommerceMVC.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string connectionString;
        private readonly UserRepository repository;

        public UserController(IConfiguration configuration,ApplicationDbContext applicationDbContext,
            UserManager<ApplicationUser> userManager)
        {
            connectionString = configuration.GetConnectionString("connection_string");
            repository = new UserRepository(connectionString);
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ApplicationUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            var product = _applicationDbContext.Users.FirstOrDefault((x) => x.Id == user.Id).Products.ToList();
            return View(product);
        }

        [HttpGet("AddToCart")]
        [AllowAnonymous]
        public IActionResult AddToCart(int id)
        {
            var product = _applicationDbContext.Product.FirstOrDefault((x) => x.Id == id);

            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            //user.AddProduct(product);
            _applicationDbContext.Users.FirstOrDefault((x) => x.Id == user.Id).Products.Add(product);
            _applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index", "User");
        }
    }
}

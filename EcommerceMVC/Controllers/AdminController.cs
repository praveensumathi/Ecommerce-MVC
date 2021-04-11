using EcommerceMVC.Models;
using EcommerceMVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EcommerceMVC.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly string connectionString;

        private readonly ProductRepository repository;
        public ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(IConfiguration configuration,
            ApplicationDbContext applicationDbContext, 
            UserManager<ApplicationUser> userManager
            )
        {
            connectionString = configuration.GetConnectionString("connection_string");
            repository = new ProductRepository(connectionString);
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var products = _applicationDbContext.Product.ToList();
            return View(products);
        }

        [HttpGet("CreateProduct")]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            _applicationDbContext.Add<Product>(product);
            await _applicationDbContext.SaveChangesAsync();
            return View("Index");
        }

        [HttpGet("UpdateProduct")]
        public IActionResult UpdateProduct(int id)
        {
            Product product = _applicationDbContext.Product.FirstOrDefault((x) => x.Id == id);
            return View(product);
        }

        [HttpPost("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            Product currentProduct = _applicationDbContext.Product.FirstOrDefault((x) => x.Id == product.Id);

            currentProduct.ProductName = product.ProductName;
            currentProduct.Price = product.Price;
            currentProduct.Image = product.Image;

            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product product = _applicationDbContext.Product.FirstOrDefault((x) => x.Id == id);
            if (product == null)
            {
                return NotFound("No Products Found");
            }

            _applicationDbContext.Remove(product);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

using EcommerceMVC.Models;
using EcommerceMVC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace EcommerceMVC.Controllers
{
    
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ProductController(IConfiguration configuration,
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var products = _applicationDbContext.Product.ToList();
            return View(new ProductViewModel()
            {
                Products = products
            });
        }
    }
}

using EcommerceMVC.Repositories;
using EcommerceMVC.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace EcommerceMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly string connectionString;

        private readonly ProductRepository repository;

        public ProductController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("connection_string");
            repository = new ProductRepository(connectionString);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var products = repository.GetAll();
            return View(new ProductViewModel()
            {
                Products = products.ToList()
            });
        }
    }
}

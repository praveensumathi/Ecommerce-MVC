using EcommerceMVC.Models;
using EcommerceMVC.Repositories;
using EcommerceMVC.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Drawing;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace EcommerceMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly string connectionString;

        private readonly ProductRepository repository;

        public AdminController(IConfiguration configuration)
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

        [HttpGet]
        public IActionResult CreateProduct()
        {

            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody]Product product)
        {
            repository.Create(product);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            Product product = repository.GetById(id);
            return View(product);
        }

        [HttpPut]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            Product currentProduct = repository.GetById(id);

            if (currentProduct.Equals(product))
            {
                return View("Index");
            }

            currentProduct.ProductName = product.ProductName;
            currentProduct.Price = product.Price;
            currentProduct.Image = product.Image;

            repository.Update(currentProduct);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            Product product = repository.GetById(id);
            if (product == null)
            {
                return NotFound("No Products Found");
            }

            repository.Delete(product);
            return RedirectToAction("Index");
        }
    }
}

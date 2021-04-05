﻿using EcommerceMVC.Models;
using EcommerceMVC.Repositories;
using EcommerceMVC.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EcommerceMVC.Controllers
{
    [Authorize]
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
            var products = _applicationDbContext.Products.ToList();
            //var products = repository.GetAll();
            return View(new ProductViewModel()
            {
                Products = products
            });
        }

        [HttpGet("CreateProduct")]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(string id, string role)
        {
            ApplicationUser user = await _userManager.FindByIdAsync("1");

            if (user == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userManager.AddToRoleAsync(user, "Admin");

            if (!result.Succeeded)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            _applicationDbContext.Products.Add(product);
            await _applicationDbContext.SaveChangesAsync();
            //repository.Create(product);
            return View("Index");
        }

        [HttpGet("UpdateProduct")]
        public IActionResult UpdateProduct(int id)
        {
            Product product = repository.GetById(id);
            return View(product);
        }

        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProduct(Product product)
        {
            Product currentProduct = repository.GetById(product.Id);

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


        [HttpGet("DeleteProduct")]
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

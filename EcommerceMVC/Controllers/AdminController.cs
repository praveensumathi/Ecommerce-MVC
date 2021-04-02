using EcommerceMVC.Repositories;
using EcommerceMVC.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult Create()
        {
            return View(new ProductInpuModel());
        }
    }
}

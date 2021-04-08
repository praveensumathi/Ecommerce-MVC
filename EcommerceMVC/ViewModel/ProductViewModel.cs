using EcommerceMVC.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceMVC.ViewModel
{
    public class ProductViewModel : ProductInpuModel
    {
        public List<Product> Products { get; set; }
    }

    public class ProductInpuModel
    {

        [Required]
        [MinLength(5)]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string Image { get; set; }
    }
}

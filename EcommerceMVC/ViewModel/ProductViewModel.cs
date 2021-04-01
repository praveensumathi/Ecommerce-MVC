using EcommerceMVC.Models;
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
        public string Name { get; set; }
        public string Price { get; set; }
        public string Image { get; set; }
    }
}

using FluentNHibernate.Mapping;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace EcommerceMVC.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public virtual IList<Product> Products { get; set; }

        public ApplicationUser()
        {
            Products = new List<Product>();
        }

        public virtual void AddProduct(Product product)
        {
            Products.Add(product);
        }
    }
}

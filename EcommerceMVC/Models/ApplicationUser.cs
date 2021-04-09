using FluentNHibernate.Mapping;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EcommerceMVC.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [JsonIgnore]
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

    public class ApplicationUserMapping : ClassMap<ApplicationUser>
    {
        public ApplicationUserMapping()
        {
            Id(x => x.Id)
               .GeneratedBy.Native();
            HasManyToMany(x => x.Products)
                 .Table("user_product");
            Table("users");
        }
    }
}

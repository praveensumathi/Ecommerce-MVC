using FluentNHibernate.Data;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EcommerceMVC.Models
{
    public class User : Entity
    {
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        [JsonIgnore]
        public virtual IList<Product> Products { get; protected set; }

        public User()
        {
            Products = new List<Product>();
        }

        public virtual void AddProduct(Product product)
        {
            Products.Add(product);
        }
    }
}

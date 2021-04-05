using FluentNHibernate.Data;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceMVC.Models
{
    public class Product : Entity
    {
        public virtual string ProductName { get; set; }
        public virtual float Price { get; set; }
        public virtual string Image { get; set; }
        public virtual IList<ApplicationUser> Users { get; protected set; }

    }

    public class ActivityMapping : ClassMap<Product>
    {
        public ActivityMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Native();
            Map(x => x.ProductName)
                .Not.Nullable();
            Map(x => x.Price)
                .Not.Nullable();
            Map(x => x.Image)
                .Not.LazyLoad();
            HasManyToMany(x => x.Users)
                 .Table("user_product");

            Table("products");
        }
    }
}

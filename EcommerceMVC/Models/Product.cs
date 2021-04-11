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
}

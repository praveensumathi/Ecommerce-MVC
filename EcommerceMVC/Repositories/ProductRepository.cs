using EcommerceMVC.Models;
using EcommerceMVC.NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceMVC.Repositories
{
    public class ProductRepository : Repository<Product>
    {
        public ProductRepository(string connectionString) :
           base(Helper.OpenSession(connectionString))
        {

        }
    }
}

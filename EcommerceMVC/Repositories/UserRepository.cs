using EcommerceMVC.Models;
using EcommerceMVC.NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceMVC.Repositories
{
    public class UserRepository : Repository<ApplicationUser>
    {
        public UserRepository(string connectionString) :
           base(Helper.OpenSession(connectionString))
        {

        }

        public ApplicationUser updateUserProduct(long id, Product product)
        {
            ApplicationUser user = this.GetById(id);

            user.Products.Add(product);
            Session.Update(user);
            Session.Flush();
            return user;
        }
    }
}

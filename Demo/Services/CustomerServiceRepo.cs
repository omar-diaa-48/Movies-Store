using Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Services
{
    public class CustomerServiceRepo : IApplicationUserRepository
    {
        private readonly MovieStoreDBContext context;

        public CustomerServiceRepo(MovieStoreDBContext _context)
        {
            context = _context;
        }
        public void AddCustomer(ApplicationUser customer)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationUser> AllCustomers()
        {
            throw new NotImplementedException();
        }

        public ApplicationUser CustomerDetails(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteCustomer(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomer(int id)
        {
            throw new NotImplementedException();
        }
    }
}
